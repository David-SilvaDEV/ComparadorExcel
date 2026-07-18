namespace ComparadorExcel.Models;

public class Docente
{
    // Número de documento (será nuestro identificador para comparar)
    public string Documento { get; set; } = string.Empty;

    // Nombre completo del docente
    public string Nombre { get; set; } = string.Empty;

    // Número de teléfono
    public string Telefono { get; set; } = string.Empty;

    // Correo electrónico
    public string Correo { get; set; } = string.Empty;

    // Institución educativa
    public string Institucion { get; set; } = string.Empty;

    // Programa o Skill
    public string Programa { get; set; } = string.Empty;

    // Nombre del archivo del que proviene el registro
    public string ArchivoOrigen { get; set; } = string.Empty;
}