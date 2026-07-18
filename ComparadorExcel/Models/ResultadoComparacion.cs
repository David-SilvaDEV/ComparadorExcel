namespace ComparadorExcel.Models;

public class ResultadoComparacion
{
    public string Documento { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public List<string> Programas { get; set; } = new();

    public List<string> Instituciones { get; set; } = new();

    public List<string> Archivos { get; set; } = new();

    // Datos adicionales de docentes
    public List<string> Telefonos { get; set; } = new();

    public List<string> Correos { get; set; } = new();
}