using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedAi.Data;
using PedAi.Models;
using PedAi.DTO;

namespace PedAi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] PedidoDTO pedidoDto)
        {
            if (pedidoDto.Itens == null || !pedidoDto.Itens.Any())
                return BadRequest("O pedido deve conter ao menos um item.");

            var pedido = new Pedido
            {
                DataPedido = DateTime.UtcNow,
                Status = "Recebido", // status inicial
                Itens = pedidoDto.Itens.Select(i => new ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(new { pedido.Id });
        }

        // GET: api/pedidos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            return Ok(new
            {
                pedido.Id,
                pedido.DataPedido,
                pedido.Status,
                Itens = pedido.Itens.Select(i => new
                {
                    i.ProdutoId,
                    Produto = i.Produto.Nome,
                    i.Quantidade,
                    i.PrecoUnitario
                })
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Itens) // Inclui os itens do pedido
                .ThenInclude(i => i.Produto) // Inclui os produtos relacionados aos itens
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();

            var resultado = pedidos.Select(p => new
            {
                p.Id,
                p.DataPedido,
                p.Status,
                Itens = p.Itens.Select(i => new
                {
                    i.ProdutoId,
                    Produto = i.Produto?.Nome, 
                    i.Quantidade,
                    i.PrecoUnitario
                })
            });

            return Ok(resultado);
        }

        // PUT: api/pedidos/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] StatusDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NovoStatus))
                return BadRequest("O novo status é obrigatório.");

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            pedido.Status = dto.NovoStatus;
            await _context.SaveChangesAsync();

            return Ok(new { pedido.Id, pedido.Status });
        }
    }
}
