using System.ComponentModel.DataAnnotations;

namespace PedAi.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;

        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public decimal Total => Itens?.Sum(i => i.Quantidade * i.PrecoUnitario) ?? 0;

    }
}
