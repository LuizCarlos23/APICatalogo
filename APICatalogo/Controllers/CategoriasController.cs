using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController: ControllerBase
    {

        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> get()
        {
            return _context.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> get(int id)
        {
            var categoria = _context.Categorias?.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (categoria is null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias?.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<Categoria> Put(int id,  Categoria categoria)
        {
            if (id != categoria.Id)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias?.FirstOrDefault(c => c.Id == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada");

            _context.Categorias?.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
