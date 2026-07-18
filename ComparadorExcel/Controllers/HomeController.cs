using System.Diagnostics;
using ComparadorExcel.Models;
using ComparadorExcel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComparadorExcel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ExcelService _excelService;
    private readonly ComparadorService _comparadorService;
    private readonly ReporteExcelService _reporteExcelService;

    public HomeController(
        ILogger<HomeController> logger,
        ExcelService excelService,
        ComparadorService comparadorService,
        ReporteExcelService reporteExcelService)
    {
        _logger = logger;
        _excelService = excelService;
        _comparadorService = comparadorService;
        _reporteExcelService = reporteExcelService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Comparar(List<IFormFile> archivos)
    {
        if (archivos == null || archivos.Count == 0)
        {
            return Content("No se recibió ningún archivo.");
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

    [HttpPost]
    public IActionResult DescargarExcel(List<ResultadoComparacion> resultados)
    {
        var archivoExcel = _reporteExcelService.GenerarReporte(resultados);

        return File(
            archivoExcel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Reporte_Comparacion.xlsx");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}