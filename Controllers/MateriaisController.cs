using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories;

namespace BibliotecaApi.Controllers;

/// <summary>
/// Gerencia as operações de consulta, cadastro,
/// atualização, exclusão e empréstimo de materiais.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MateriaisController : ControllerBase
{
    private readonly IMaterialRepository _repository;

/// <summary>
/// Inicializa o controller com o repositório de materiais.
/// </summary>
/// <param name="repository">Repositório responsável pelo acesso aos materiais.</param>
    public MateriaisController(IMaterialRepository repository)
    {
        _repository = repository;
    }


// LISTAR MATERIAIS COM FILTROS E PAGINAÇÃO
/// <summary>
/// Lista os materiais cadastrados, permitindo filtrar por título,
/// tipo e ano de publicação, além de utilizar paginação.
/// </summary>
/// <param name="titulo">Título ou parte do título do material.</param>
/// <param name="tipo">Tipo do material: Livro ou Revista.</param>
/// <param name="ano">Ano de publicação do material.</param>
/// <param name="pagina">Número da página desejada. Padrão: 1.</param>
/// <param name="quantidade">Quantidade de registros por página. Padrão: 10. Máximo: 100.</param>
/// <response code="200">Lista de materiais retornada com sucesso.</response>
/// <response code="400">Parâmetros de filtro ou paginação inválidos.</response>
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
/// <summary>
/// Busca um material específico pelo seu identificador.
/// </summary>
/// <param name="id">Identificador do material.</param>
/// <response code="200">Material encontrado com sucesso.</response>
/// <response code="404">Material não encontrado.</response>
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



/// <summary>
/// Cadastra um novo livro na biblioteca.
/// </summary>
/// <remarks>
/// Exemplo de requisição:
///
///     POST /api/Materiais/livro
///     {
///         "titulo": "Clean Code",
///         "anoPublicacao": 2008,
///         "autor": "Robert C. Martin",
///         "paginas": 464,
///         "isbn": "9780132350884",
///         "editora": "Prentice Hall",
///         "categoria": "Tecnologia"
///     }
/// </remarks>
/// <param name="livro">Dados do livro que será cadastrado.</param>
/// <response code="201">Livro cadastrado com sucesso.</response>
/// <response code="400">Dados inválidos ou campos obrigatórios não preenchidos.</response>
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



/// <summary>
/// Cadastra uma nova revista na biblioteca.
/// </summary>
/// <remarks>
/// Exemplo de requisição:
///
///     POST /api/Materiais/revista
///     {
///         "titulo": "Revista de Tecnologia",
///         "anoPublicacao": 2025,
///         "edicao": 10,
///         "mesPublicacao": 6,
///         "issn": "1234-5678",
///         "editora": "Editora Tech",
///         "categoria": "Tecnologia"
///     }
/// </remarks>
/// <param name="revista">Dados da revista que será cadastrada.</param>
/// <response code="201">Revista cadastrada com sucesso.</response>
/// <response code="400">Dados inválidos ou campos obrigatórios não preenchidos.</response>
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



/// <summary>
/// Atualiza os dados de um livro cadastrado.
/// </summary>
/// <param name="id">Identificador do livro que será atualizado.</param>
/// <param name="livro">Novos dados do livro.</param>
/// <response code="200">Livro atualizado com sucesso.</response>
/// <response code="400">Dados inválidos ou material informado não é um livro.</response>
/// <response code="404">Livro não encontrado.</response>
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


/// <summary>
/// Atualiza os dados de uma revista cadastrada.
/// </summary>
/// <param name="id">Identificador da revista que será atualizada.</param>
/// <param name="revista">Novos dados da revista.</param>
/// <response code="200">Revista atualizada com sucesso.</response>
/// <response code="400">Dados inválidos ou material informado não é uma revista.</response>
/// <response code="404">Revista não encontrada.</response>
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


/// <summary>
/// Remove um material da biblioteca pelo seu identificador.
/// </summary>
/// <param name="id">Identificador do material que será removido.</param>
/// <response code="204">Material removido com sucesso.</response>
/// <response code="404">Material não encontrado.</response>
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

/// <summary>
/// Consulta a descrição e o prazo de empréstimo de um material.
/// </summary>
/// <param name="id">Identificador do material.</param>
/// <response code="200">Descrição e prazo retornados com sucesso.</response>
/// <response code="404">Material não encontrado.</response>
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

/// <summary>
/// Realiza o empréstimo de um material da biblioteca.
/// </summary>
/// <param name="id">Identificador do material que será emprestado.</param>
/// <response code="200">Empréstimo realizado com sucesso.</response>
/// <response code="400">Material não possui capacidade de empréstimo.</response>
/// <response code="404">Material não encontrado.</response>
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