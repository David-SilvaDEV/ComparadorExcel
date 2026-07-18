namespace ComparadorExcel.Models;

public class Estudiante
{
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public string Grado { get; set; } = string.Empty;
    public string Edad { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;

    public string Programa { get; set; } = string.Empty;
    public string Institucion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int FilaExcel { get; set; }
    public string ArchivoOrigen { get; set; } = string.Empty;
}