using Microsoft.EntityFrameworkCore;
using PedAi.Models;

namespace PedAi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
       public DbSet<Pedido> Pedidos { get; set; }
public DbSet<ItemPedido> ItensPedido { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // mantém o comportamento padrão

            // Relacionamento: Pedido tem muitos Itens
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId);

            // Relacionamento: ItemPedido está relacionado a um Produto
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany() // Produto não tem navegação reversa aqui
                .HasForeignKey(i => i.ProdutoId);
        }
    }
}
