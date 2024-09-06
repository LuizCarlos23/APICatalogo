using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController: ControllerBase
    {

        //private readonly AppDbContext _context;
        //private readonly IRepository<Categoria> _categoriaRepository;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.Id == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada");


            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);


            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var categoriaCriadaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriadaDto.Id }, categoriaCriadaDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.Id)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoriaDto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.Id == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada");

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }
    }
}
