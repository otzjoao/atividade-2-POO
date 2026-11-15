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

    // LISTAR MATERIAIS COM FILTROS E PAGINAÇÃO
    // Ex: GET /api/Materiais?titulo=Clean&tipo=Livro&ano=2008&pagina=1&quantidade=5
    [HttpGet]
    public IActionResult ObterTodos(
        [FromQuery] string? titulo = null,
        [FromQuery] string? tipo = null,
        [FromQuery] int? ano = null,
        [FromQuery] int pagina = 1,
        [FromQuery] int quantidade = 10)
    {
        if (!string.IsNullOrWhiteSpace(tipo) &&
            !tipo.Equals("Livro", StringComparison.OrdinalIgnoreCase) &&
            !tipo.Equals("Revista", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Tipo de material inválido. Use 'Livro' ou 'Revista'.");
        }

        if (ano.HasValue && ano.Value <= 0)
            return BadRequest("Ano de publicação inválido.");
        
         // VALIDAR PAGINAÇÃO
        if (pagina < 1)
            return BadRequest("A página deve ser maior ou igual a 1.");

        if (quantidade < 1 || quantidade > 100)
        {
            return BadRequest("A quantidade deve estar entre 1 e 100.");
        }

        return Ok(_repository.ObterTodos(titulo, tipo, ano, pagina, quantidade));
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

        if (string.IsNullOrWhiteSpace(livro.ISBN))
        return BadRequest("O ISBN é obrigatório.");

        if (string.IsNullOrWhiteSpace(livro.Editora))
        return BadRequest("A editora é obrigatória.");

        if (string.IsNullOrWhiteSpace(livro.Categoria))
        return BadRequest("A categoria é obrigatória.");

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


        if (string.IsNullOrWhiteSpace(revista.ISSN))
        return BadRequest("O ISSN é obrigatório.");

        if (string.IsNullOrWhiteSpace(revista.Editora))
        return BadRequest("A editora é obrigatória.");

        if (string.IsNullOrWhiteSpace(revista.Categoria))
        return BadRequest("A categoria é obrigatória.");

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

        if (string.IsNullOrWhiteSpace(livro.ISBN))
        return BadRequest("O ISBN é obrigatório.");

        if (string.IsNullOrWhiteSpace(livro.Editora))
        return BadRequest("A editora é obrigatória.");

        if (string.IsNullOrWhiteSpace(livro.Categoria))
        return BadRequest("A categoria é obrigatória.");

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


        if (string.IsNullOrWhiteSpace(revista.ISSN))
        return BadRequest("O ISSN é obrigatório.");

        if (string.IsNullOrWhiteSpace(revista.Editora))
        return BadRequest("A editora é obrigatória.");

        if (string.IsNullOrWhiteSpace(revista.Categoria))
        return BadRequest("A categoria é obrigatória.");

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
        {
            return NotFound("Material não encontrado.");
        }

        if (material is not IEmprestimo emprestimo)
        {
            return BadRequest(new {mensagem = "Este material não possui capacidade de empréstimo."});
        }
        string mensagem = emprestimo.Emprestar();
        return Ok(mensagem);
    }
}