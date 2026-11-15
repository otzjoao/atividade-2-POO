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
        return Problem(
            detail: "Tipo de material inválido. Use 'Livro' ou 'Revista'.",
            statusCode: 400,
            title: "Dados inválidos"
        );
    }

    if (ano.HasValue && ano.Value <= 0)
    {
        return Problem(
            detail: "Ano de publicação inválido.",
            statusCode: 400,
            title: "Dados inválidos"
        );
    }

    // VALIDAR PAGINAÇÃO
    if (pagina < 1)
    {
        return Problem(
            detail: "A página deve ser maior ou igual a 1.",
            statusCode: 400,
            title: "Dados inválidos"
        );
    }

    if (quantidade < 1 || quantidade > 100)
    {
        return Problem(
            detail: "A quantidade deve estar entre 1 e 100.",
            statusCode: 400,
            title: "Dados inválidos"
        );
    }

    return Ok(_repository.ObterTodos(titulo, tipo, ano, pagina, quantidade));
}



 // BUSCAR MATERIAL PELO ID
 [HttpGet("{id:int}")]
 public IActionResult ObterPorId(int id)
 {
     var material = _repository.ObterPorId(id);

     if (material == null)
     {
         return Problem(
             detail: "Material não encontrado.",
             statusCode: 404,
             title: "Material não encontrado"
         );
     }

     return Ok(material);
 }



    // CADASTRAR LIVRO
    [HttpPost("livro")]
    public IActionResult CadastrarLivro([FromBody] Livro livro)
    {
        if (string.IsNullOrWhiteSpace(livro.Titulo))
            return Problem(
                detail: "O título é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (livro.AnoPublicacao < 0)
            return Problem(
                detail: "Ano de publicação inválido.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return Problem(
                detail: "O autor é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (livro.Paginas <= 0)
            return Problem(
                detail: "O número de páginas deve ser maior que zero.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.ISBN))
            return Problem(
                detail: "O ISBN é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Editora))
            return Problem(
                detail: "A editora é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Categoria))
            return Problem(
                detail: "A categoria é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

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
            return Problem(
                detail: "O título é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (revista.AnoPublicacao < 0)
            return Problem(
                detail: "Ano de publicação inválido.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (revista.Edicao <= 0)
            return Problem(
                detail: "A edição deve ser maior que zero.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (revista.MesPublicacao < 1 ||
            revista.MesPublicacao > 12)
            return Problem(
                detail: "O mês deve estar entre 1 e 12.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.ISSN))
            return Problem(
                detail: "O ISSN é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.Editora))
            return Problem(
                detail: "A editora é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.Categoria))
            return Problem(
                detail: "A categoria é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

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
            return Problem(
                detail: "Livro não encontrado.",
                statusCode: 404,
                title: "Livro não encontrado"
            );

        if (existente is not Livro)
            return Problem(
                detail: "O material informado não é um livro.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Titulo))
            return Problem(
                detail: "O título é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (livro.AnoPublicacao < 0)
            return Problem(
                detail: "Ano de publicação inválido.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return Problem(
                detail: "O autor é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (livro.Paginas <= 0)
            return Problem(
                detail: "O número de páginas deve ser maior que zero.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.ISBN))
            return Problem(
                detail: "O ISBN é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Editora))
            return Problem(
                detail: "A editora é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(livro.Categoria))
            return Problem(
                detail: "A categoria é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

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
            return Problem(
                detail: "Revista não encontrada.",
                statusCode: 404,
                title: "Revista não encontrada"
            );

        if (existente is not Revista)
            return Problem(
                detail: "O material informado não é uma revista.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.Titulo))
            return Problem(
                detail: "O título é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (revista.AnoPublicacao < 0)
            return Problem(
                detail: "Ano de publicação inválido.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (revista.Edicao <= 0)
            return Problem(
                detail: "A edição deve ser maior que zero.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (revista.MesPublicacao < 1 ||
            revista.MesPublicacao > 12)
            return Problem(
                detail: "O mês deve estar entre 1 e 12.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.ISSN))
            return Problem(
                detail: "O ISSN é obrigatório.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.Editora))
            return Problem(
                detail: "A editora é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

        if (string.IsNullOrWhiteSpace(revista.Categoria))
            return Problem(
                detail: "A categoria é obrigatória.",
                statusCode: 400,
                title: "Dados inválidos"
            );

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
    {
        return Problem(
            detail: "Material não encontrado.",
            statusCode: 404,
            title: "Material não encontrado"
        );
    }

    return NoContent();
}

// CONSULTAR DESCRIÇÃO E PRAZO
[HttpGet("{id:int}/descricao")]
public IActionResult ObterDescricao(int id)
{
    var material = _repository.ObterPorId(id);

    if (material == null)
    {
        return Problem(
            detail: "Material não encontrado.",
            statusCode: 404,
            title: "Material não encontrado"
        );
    }

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
        return Problem(
            detail: "Material não encontrado.",
            statusCode: 404,
            title: "Material não encontrado"
        );
    }

    if (material is not IEmprestimo emprestimo)
    {
        return Problem(
            detail: "Este material não possui capacidade de empréstimo.",
            statusCode: 400,
            title: "Dados inválidos"
        );
    }

    string mensagem = emprestimo.Emprestar();
    return Ok(mensagem);
}
}