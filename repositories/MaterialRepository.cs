using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly Dictionary<int, MaterialBiblioteca> _materiais;

    private int _ultimoId;

    public MaterialRepository()
    {
        _materiais = new Dictionary<int, MaterialBiblioteca>
        {
            [1] = new Livro
            {
                Id = 1,
                Titulo = "Clean Code",
                AnoPublicacao = 2008,
                Autor = "Robert C. Martin",
                Paginas = 464
            },

            [2] = new Revista
            {
                Id = 2,
                Titulo = "Tecnologia e Inovação",
                AnoPublicacao = 2026,
                Edicao = 15,
                MesPublicacao = 11
            }
        };

        _ultimoId = 2;
    }

    private int GerarNovoId()
    {
        _ultimoId++;
        return _ultimoId;
    }

    public IEnumerable<MaterialBiblioteca> ObterTodos()
    {
        return _materiais.Values;
    }

    public MaterialBiblioteca? ObterPorId(int id)
    {
        _materiais.TryGetValue(id, out var material);

        return material;
    }

    public MaterialBiblioteca Adicionar(MaterialBiblioteca material)
    {
        material.Id = GerarNovoId();

        _materiais.Add(material.Id, material);

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