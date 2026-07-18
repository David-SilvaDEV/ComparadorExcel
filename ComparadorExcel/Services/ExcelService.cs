using ClosedXML.Excel;
using ComparadorExcel.Models;

namespace ComparadorExcel.Services;

public class ExcelService
{
    /// <summary>
    /// Lee los estudiantes de un archivo Excel.
    /// </summary>
    public List<Estudiante> LeerEstudiantes(IFormFile archivo)
    {
        var estudiantes = new List<Estudiante>();

        using var stream = archivo.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var hoja = workbook.Worksheet(1);

        // Datos generales
        string institucion = LimpiarTexto(hoja.Cell("B2").GetString());
        string categoria = LimpiarTexto(hoja.Cell("B4").GetString());

        // Los estudiantes empiezan en la fila 11
        for (int fila = 11; fila <= hoja.LastRowUsed().RowNumber(); fila++)
        {
            string nombre = LimpiarTexto(hoja.Cell(fila, 2).GetString());

            // Cuando termina la lista de estudiantes
            if (string.IsNullOrWhiteSpace(nombre))
                break;

            var estudiante = new Estudiante
            {
                Nombre = nombre,
                TipoDocumento = LimpiarTexto(hoja.Cell(fila, 3).GetString()),
                Documento = LimpiarTexto(hoja.Cell(fila, 4).GetString()),
                Grado = LimpiarTexto(hoja.Cell(fila, 5).GetString()),
                Edad = LimpiarTexto(hoja.Cell(fila, 6).GetString()),
                Sexo = LimpiarTexto(hoja.Cell(fila, 7).GetString()),
                Institucion = institucion,
                Categoria = categoria,
                ArchivoOrigen = archivo.FileName,
                FilaExcel = fila
            };

            estudiantes.Add(estudiante);
        }

        return estudiantes;
    }

    /// <summary>
    /// Lee los docentes de un archivo Excel.
    /// </summary>
    public List<Docente> LeerDocentes(IFormFile archivo)
    {
        var docentes = new List<Docente>();

        using var stream = archivo.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var hoja = workbook.Worksheet(1);

        // Los encabezados están en la fila 1
        // Los datos empiezan en la fila 2

        int ultimaFila = hoja.LastRowUsed().RowNumber();

        for (int fila = 2; fila <= ultimaFila; fila++)
        {
            string nombre = LimpiarTexto(
                hoja.Cell(fila, 5).GetString());

            // Si no hay nombre, se ignora la fila
            if (string.IsNullOrWhiteSpace(nombre))
                continue;

            string institucionDistrital = LimpiarTexto(
                hoja.Cell(fila, 10).GetString());

            string nombreEntidad = LimpiarTexto(
                hoja.Cell(fila, 11).GetString());

            // Usamos la institución distrital si existe.
            // Si no, usamos el nombre de la entidad.
            string institucion =
                !string.IsNullOrWhiteSpace(institucionDistrital)
                    ? institucionDistrital
                    : nombreEntidad;

            var docente = new Docente
            {
                // Columna E
                Nombre = nombre,

                // Columna F
                Documento = LimpiarTexto(
                    hoja.Cell(fila, 6).GetString()),

                // Columna G
                Correo = LimpiarTexto(
                    hoja.Cell(fila, 7).GetString()),

                // Columna H
                Telefono = LimpiarTexto(
                    hoja.Cell(fila, 8).GetString()),

                // Columna D
                Programa = LimpiarTexto(
                    hoja.Cell(fila, 4).GetString()),

                // Columna J o K
                Institucion = institucion,

                // Nombre del archivo
                ArchivoOrigen = archivo.FileName
            };

            docentes.Add(docente);
        }

        return docentes;
    }

    /// <summary>
    /// Limpia un texto eliminando espacios al inicio y final
    /// y convirtiéndolo a minúsculas.
    /// </summary>
    private string LimpiarTexto(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        return texto.Trim().ToLower();
    }
}