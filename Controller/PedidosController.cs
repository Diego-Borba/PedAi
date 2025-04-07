using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedAi.Data;
using PedAi.Models;

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
        public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
        {
            if (pedido.Itens == null || !pedido.Itens.Any())
                return BadRequest("O pedido deve conter ao menos um item.");

             //Calcula total do pedido
            //pedido.Total = pedido.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            return pedido;
        }
    }
}
