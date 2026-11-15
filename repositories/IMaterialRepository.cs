using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories;

public interface IMaterialRepository
{
    ResultadoPaginado<MaterialBiblioteca> ObterTodos(
        string? titulo = null,
        string? tipo = null,
        int? ano = null,
        int pagina = 1,
        int quantidade = 10);

    MaterialBiblioteca? ObterPorId(int id);
    MaterialBiblioteca Adicionar(MaterialBiblioteca material);
    bool Atualizar(int id, MaterialBiblioteca material);
    bool Remover(int id);
}

public class ResultadoPaginado<T>
{
    public List<T> Itens { get; set; } = new();
    public int TotalItens { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public int TotalPaginas { get; set; }
}
