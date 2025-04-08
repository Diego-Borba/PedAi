namespace PedAi.DTO
{

    public class PedidoDTO
    {
        public DateTime DataPedido { get; set; } = DateTime.UtcNow;
        public List<ItemPedidoDTO> Itens { get; set; } = new List<ItemPedidoDTO>();
    }

    public class ItemPedidoDTO
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

    // DTO separado fora da classe
    public class StatusDTO
    {
        public string NovoStatus { get; set; }
    }
}