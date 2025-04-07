namespace PedAi.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int OrdemVisualizacao { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QtdeMax { get; set; }
        public string Categoria { get; set; }
        public string CodigoPdv { get; set; }
        public string Imagem { get; set; }
        
    }
}
