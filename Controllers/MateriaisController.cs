using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MateriaisController : ControllerBase
{
    private readonly IMaterialRepository _repository;

    public MateriaisController(IMaterialRepository repository)
    {
        _repository = repository;
    }

    // LISTAR TODOS OS MATERIAIS
    [HttpGet]
    public IActionResult ObterTodos()
    {
        return Ok(_repository.ObterTodos());
    }

    // BUSCAR MATERIAL PELO ID
    [HttpGet("{id:int}")]
    public IActionResult ObterPorId(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        return Ok(material);
    }

    // CADASTRAR LIVRO
    [HttpPost("livro")]
    public IActionResult CadastrarLivro([FromBody] Livro livro)
    {
        if (string.IsNullOrWhiteSpace(livro.Titulo))
            return BadRequest("O título é obrigatório.");

        if (livro.AnoPublicacao < 0)
            return BadRequest("Ano de publicação inválido.");

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest("O autor é obrigatório.");

        if (livro.Paginas <= 0)
            return BadRequest("O número de páginas deve ser maior que zero.");

        var cadastrado = _repository.Adicionar(livro);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = cadastrado.Id },
            cadastrado
        );
    }

    // CADASTRAR REVISTA
    [HttpPost("revista")]
    public IActionResult CadastrarRevista([FromBody] Revista revista)
    {
        if (string.IsNullOrWhiteSpace(revista.Titulo))
            return BadRequest("O título é obrigatório.");

        if (revista.AnoPublicacao < 0)
            return BadRequest("Ano de publicação inválido.");

        if (revista.Edicao <= 0)
            return BadRequest("A edição deve ser maior que zero.");

        if (revista.MesPublicacao < 1 ||
            revista.MesPublicacao > 12)
            return BadRequest("O mês deve estar entre 1 e 12.");

        var cadastrado = _repository.Adicionar(revista);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = cadastrado.Id },
            cadastrado
        );
    }

    // ATUALIZAR LIVRO
    [HttpPut("livro/{id:int}")]
    public IActionResult AtualizarLivro(int id, [FromBody] Livro livro)
    {
        var existente = _repository.ObterPorId(id);

        if (existente == null)
            return NotFound("Livro não encontrado.");

        if (existente is not Livro)
            return BadRequest("O material informado não é um livro.");

        if (string.IsNullOrWhiteSpace(livro.Titulo))
            return BadRequest("O título é obrigatório.");

        if (livro.AnoPublicacao < 0)
            return BadRequest("Ano de publicação inválido.");

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest("O autor é obrigatório.");

        if (livro.Paginas <= 0)
            return BadRequest("O número de páginas deve ser maior que zero.");

        livro.Id = id;

        _repository.Atualizar(id, livro);

        return Ok(livro);
    }

    // ATUALIZAR REVISTA
    [HttpPut("revista/{id:int}")]
    public IActionResult AtualizarRevista(int id, [FromBody] Revista revista)
    {
        var existente = _repository.ObterPorId(id);

        if (existente == null)
            return NotFound("Revista não encontrada.");

        if (existente is not Revista)
            return BadRequest("O material informado não é uma revista.");

        if (string.IsNullOrWhiteSpace(revista.Titulo))
            return BadRequest("O título é obrigatório.");

        if (revista.AnoPublicacao < 0)
            return BadRequest("Ano de publicação inválido.");

        if (revista.Edicao <= 0)
            return BadRequest("A edição deve ser maior que zero.");

        if (revista.MesPublicacao < 1 ||
            revista.MesPublicacao > 12)
            return BadRequest("O mês deve estar entre 1 e 12.");

        revista.Id = id;

        _repository.Atualizar(id, revista);

        return Ok(revista);
    }

    // EXCLUIR MATERIAL
    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        bool removido = _repository.Remover(id);

        if (!removido)
            return NotFound("Material não encontrado.");

        return NoContent();
    }

    // CONSULTAR DESCRIÇÃO E PRAZO
    [HttpGet("{id:int}/descricao")]
    public IActionResult ObterDescricao(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        return Ok(new
        {
            id = material.Id,
            tipo = material.GetType().Name,
            descricao = material.Descricao(),
            prazoEmprestimo = material.CalcularPrazo()
        });
    }

    // REALIZAR EMPRÉSTIMO
    [HttpPost("{id:int}/emprestar")]
    public IActionResult Emprestar(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        if (material is not IEmprestimo emprestimo)
            return BadRequest(
                "Este material não possui capacidade de empréstimo."
            );

        return Ok(emprestimo.Emprestar());
    }
}