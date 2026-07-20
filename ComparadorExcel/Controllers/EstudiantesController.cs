using ComparadorExcel.Models;
using ComparadorExcel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComparadorExcel.Controllers;

public class EstudiantesController : Controller
{
    private readonly ExcelService _excelService;
    private readonly ComparadorService _comparadorService;
    private readonly ReporteExcelService _reporteExcelService;
    private readonly ReporteTemporalService _reporteTemporalService;

    public EstudiantesController(
        ExcelService excelService,
        ComparadorService comparadorService,
        ReporteExcelService reporteExcelService,
        ReporteTemporalService reporteTemporalService)
    {
        _excelService = excelService;
        _comparadorService = comparadorService;
        _reporteExcelService = reporteExcelService;
        _reporteTemporalService = reporteTemporalService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Comparar(List<IFormFile> archivos)
    {
        if (archivos == null || archivos.Count == 0)
        {
            return Content("Debe seleccionar al menos un archivo.");
        }

        var todosLosEstudiantes = new List<Estudiante>();

        var errores = new List<RegistroConError>();

        foreach (var archivo in archivos)
        {
            try
            {
                var estudiantes =
                    _excelService.LeerEstudiantes(archivo);

                todosLosEstudiantes.AddRange(estudiantes);
            }
            catch (Exception ex)
            {
                errores.Add(new RegistroConError
                {
                    Archivo = archivo.FileName,
                    Motivo = ex.Message
                });
            }
        }

        var repetidos =
            _comparadorService.CompararEstudiantes(
                todosLosEstudiantes);

        // Guardamos los resultados temporalmente
        var reporteId =
            _reporteTemporalService.Guardar(repetidos);

        var modelo = new ResultadoViewModel
        {
            Resultados = repetidos,

            TotalArchivos = archivos.Count,

            TotalRegistros =
                todosLosEstudiantes.Count,

            TotalRepetidos =
                repetidos.Count,

            TotalSinDocumento =
                todosLosEstudiantes.Count(e =>
                    string.IsNullOrWhiteSpace(
                        e.Documento)),

            Errores = errores
        };

        // Enviamos el identificador a la vista
        ViewBag.ReporteId = reporteId;

        return View("Resultado", modelo);
    }

    [HttpGet]
    public IActionResult DescargarExcel(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(
                "No se encontró el identificador del reporte.");
        }

        var resultados =
            _reporteTemporalService.Obtener(id);

        if (resultados == null)
        {
            return NotFound(
                "El reporte ya no está disponible.");
        }

        var archivoExcel =
            _reporteExcelService.GenerarReporte(
                resultados);

        // Eliminamos el reporte después de descargarlo
        _reporteTemporalService.Eliminar(id);

        return File(
            archivoExcel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Reporte_Estudiantes.xlsx");
    }
}