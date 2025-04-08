using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedAi.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime DataPedido { get; set; }
        public string Status { get; set; }


        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public decimal Total => Itens?.Sum(i => i.Quantidade * i.PrecoUnitario) ?? 0;

    }
}
