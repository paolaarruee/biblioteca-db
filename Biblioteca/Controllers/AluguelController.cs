using Biblioteca.Data.Dtos.AluguelDto;
using Biblioteca.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AluguelController : ControllerBase
    {
        private readonly AluguelService _service;

        public AluguelController(AluguelService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaAluguel([FromBody] CreateAluguelDto aluguelDto)
        {
            var aluguel = _service.AdicionaAluguel(aluguelDto);
            return CreatedAtAction(nameof(RecuperaAluguelPorId), new { id = aluguel.Id }, aluguel);
        }

        [HttpGet]
        public IEnumerable<ReadAluguelDto> RecuperaAluguel([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _service.RecuperaAlugueis(skip, take);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaAluguelPorId(int id)
        {
            var aluguel = _service.RecuperaAluguelPorId(id);
            if (aluguel == null) return NotFound();
            var aluguelDto = _service.Mapper.Map<ReadAluguelDto>(aluguel);
            return Ok(aluguelDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaAluguel(int id, [FromBody] UpdateAluguelDto aluguelDto)
        {
            var result = _service.AtualizaAluguel(id, aluguelDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizaAluguelParcial(int id, [FromBody] JsonPatchDocument<UpdateAluguelDto> patch)
        {
            if (!_service.AtualizaAluguelParcial(id, patch, out var aluguelParaAtualizar))
                return NotFound();

            if (!TryValidateModel(aluguelParaAtualizar))
                return ValidationProblem(ModelState);

            _service.Mapper.Map(aluguelParaAtualizar, _service.RecuperaAluguelPorId(id));
            _service.Context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaAluguel(int id)
        {
            var result = _service.DeletaAluguel(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
