namespace BibliotecaApi.Models;

public class Revista : MaterialBiblioteca
{
    public int Edicao { get; set; }

    public int MesPublicacao { get; set; }

    public string ISSN { get; set; } = string.Empty;

    public string Editora { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public override int CalcularPrazo()
    {
        return 7;
    }

    public override string Descricao()
    {
        return $"Revista: {Titulo}, Edição: {Edicao}, " +
               $"ISSN: {ISSN}, Editora: {Editora}, Categoria: {Categoria}";
    }
}