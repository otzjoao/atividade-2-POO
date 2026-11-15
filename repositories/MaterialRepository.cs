using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly Dictionary<int, MaterialBiblioteca> _materiais = new();

    public MaterialRepository()
{
    Adicionar(new Livro
    {
        Titulo = "O Senhor dos Anéis",
        Autor = "J.R.R. Tolkien",
        AnoPublicacao = 1954,
        Paginas = 1216,
        ISBN = "9780544003415",
        Editora = "HarperCollins",
        Categoria = "Fantasia"
    });

    Adicionar(new Livro
    {
        Titulo = "Clean Code",
        Autor = "Robert C. Martin",
        AnoPublicacao = 2008,
        Paginas = 464,
        ISBN = "9780132350884",
        Editora = "Prentice Hall",
        Categoria = "Programação"
    });

    Adicionar(new Revista
    {
        Titulo = "Revista de Tecnologia",
        AnoPublicacao = 2026,
        Edicao = 20,
        MesPublicacao = 11,
        ISSN = "1516-3270",
        Editora = "Editora Exemplo",
        Categoria = "Tecnologia"
    });
}

    private int GerarNovoId()
    {
        if (_materiais.Count == 0)
            return 1;
        
        return _materiais.Keys.Max() + 1;
    }

    public ResultadoPaginado<MaterialBiblioteca> ObterTodos(
        string? titulo = null,
        string? tipo = null,
        int? ano = null,
        int pagina = 1,
        int quantidade = 10)
    {
        var filtrados = _materiais.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(titulo))
            filtrados = filtrados.Where(m =>
                m.Titulo.Contains(titulo.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(tipo))
            filtrados = filtrados.Where(m =>
                m.GetType().Name.Equals(tipo.Trim(), StringComparison.OrdinalIgnoreCase));

        if (ano.HasValue)
            filtrados = filtrados.Where(m => m.AnoPublicacao == ano.Value);

        var lista = filtrados.OrderBy(m => m.Id).ToList();
        int totalItens = lista.Count;

        int totalPaginas = (int)Math.Ceiling(totalItens / (double)quantidade);

        var itens = lista
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToList();

        return new ResultadoPaginado<MaterialBiblioteca>
        {
            Itens = itens,
            TotalItens = totalItens,
            Pagina = pagina,
            Quantidade = quantidade,
            TotalPaginas = totalPaginas
        };
    }

    public MaterialBiblioteca? ObterPorId(int id)
    {
        return _materiais.TryGetValue(id, out var material) ? material : null;

    }

    public MaterialBiblioteca Adicionar(MaterialBiblioteca material)
    {
        int novoId = GerarNovoId();
        material.Id = novoId;
        _materiais[novoId] = material;
        return material;
    }

    public bool Atualizar(int id, MaterialBiblioteca material)
    {
        if (!_materiais.ContainsKey(id))
            return false;

        material.Id = id;
        _materiais[id] = material;
        return true;
    }

    public bool Remover(int id)
    {
        return _materiais.Remove(id);
    }
}