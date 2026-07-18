using ComparadorExcel.Models;

namespace ComparadorExcel.Services;

public class ComparadorService
{
    public List<ResultadoComparacion> CompararEstudiantes(
        List<Estudiante> estudiantes)
    {
        var resultados = estudiantes
            .Where(e => !string.IsNullOrWhiteSpace(e.Documento))
            .GroupBy(e => e.Documento)
            .Where(g => g.Count() > 1)
            .Select(g => new ResultadoComparacion
            {
                Documento = g.Key,
                Nombre = g.First().Nombre,
                Cantidad = g.Count(),

                Programas = g.Select(e => e.Programa)
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Distinct()
                    .ToList(),

                Instituciones = g.Select(e => e.Institucion)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Distinct()
                    .ToList(),

                Archivos = g.Select(e => e.ArchivoOrigen)
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Distinct()
                    .ToList()
            })
            .ToList();

        return resultados;
    }


    public List<ResultadoComparacion> CompararDocentes(
        List<Docente> docentes)
    {
        var resultados = docentes
            .Where(d => !string.IsNullOrWhiteSpace(d.Documento))
            .GroupBy(d => d.Documento)
            .Where(g => g.Count() > 1)
            .Select(g => new ResultadoComparacion
            {
                Documento = g.Key,

                Nombre = g.First().Nombre,

                Cantidad = g.Count(),

                Programas = g.Select(d => d.Programa)
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Distinct()
                    .ToList(),

                Instituciones = g.Select(d => d.Institucion)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Distinct()
                    .ToList(),

                Archivos = g.Select(d => d.ArchivoOrigen)
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Distinct()
                    .ToList(),

                Telefonos = g.Select(d => d.Telefono)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .ToList(),

                Correos = g.Select(d => d.Correo)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .ToList()
            })
            .ToList();

        return resultados;
    }
}