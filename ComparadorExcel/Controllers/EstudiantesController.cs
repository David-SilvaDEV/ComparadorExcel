using ComparadorExcel.Models;
using ComparadorExcel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComparadorExcel.Controllers;

public class EstudiantesController : Controller
{
    private readonly ExcelService _excelService;
    private readonly ComparadorService _comparadorService;
    private readonly ReporteExcelService _reporteExcelService;

    public EstudiantesController(
        ExcelService excelService,
        ComparadorService comparadorService,
        ReporteExcelService reporteExcelService)
    {
        _excelService = excelService;
        _comparadorService = comparadorService;
        _reporteExcelService = reporteExcelService;
    }

    // Muestra la página para cargar los archivos
    public IActionResult Index()
    {
        return View();
    }

    // Compara los archivos cargados
    [HttpPost]
    public IActionResult Comparar(List<IFormFile> archivos)
    {
        if (archivos == null || archivos.Count == 0)
        {
            return Content("Debe seleccionar al menos un archivo.");
        }

        var todosLosEstudiantes = new List<Estudiante>();

        foreach (var archivo in archivos)
        {
            var estudiantes = _excelService.LeerEstudiantes(archivo);
            todosLosEstudiantes.AddRange(estudiantes);
        }

        var repetidos = _comparadorService.CompararEstudiantes(todosLosEstudiantes);

        var modelo = new ResultadoViewModel
        {
            Resultados = repetidos,
            TotalArchivos = archivos.Count,
            TotalRegistros = todosLosEstudiantes.Count,
            TotalRepetidos = repetidos.Count,
            TotalSinDocumento = todosLosEstudiantes.Count(e => string.IsNullOrWhiteSpace(e.Documento))
        };

        return View("Resultado", modelo);
    }

    // Descarga el reporte en Excel
    [HttpPost]
    public IActionResult DescargarExcel(List<ResultadoComparacion> resultados)
    {
        var archivoExcel = _reporteExcelService.GenerarReporte(resultados);

        return File(
            archivoExcel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Reporte_Estudiantes.xlsx");
    }
}