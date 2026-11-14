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

    public IEnumerable<MaterialBiblioteca> ObterTodos()
    {
        return _materiais.Values;
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