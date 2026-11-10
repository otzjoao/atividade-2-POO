namespace BibliotecaApi.Models;

public abstract class MaterialBiblioteca
{
    public int Id { get; internal set; }

    public string Titulo { get; set; } = string.Empty;

    public int AnoPublicacao { get; set; }

    public string Tipo => GetType().Name;

    public abstract int CalcularPrazo();

    public virtual string Descricao()
    {
        return $"Material: {Titulo}";
    }
}