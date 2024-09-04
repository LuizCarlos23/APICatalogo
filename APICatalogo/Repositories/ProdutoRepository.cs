using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
        {
            return _context.Produtos.Where(p => p.CategoriaId == idCategoria).ToList();
        }
    }
}
