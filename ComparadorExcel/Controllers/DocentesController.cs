using ComparadorExcel.Models;
using ComparadorExcel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComparadorExcel.Controllers;

public class DocentesController : Controller
{
    private readonly ExcelService _excelService;
    private readonly ComparadorService _comparadorService;
    private readonly ReporteExcelService _reporteExcelService;

    public DocentesController(
        ExcelService excelService,
        ComparadorService comparadorService,
        ReporteExcelService reporteExcelService)
    {
        _excelService = excelService;
        _comparadorService = comparadorService;
        _reporteExcelService = reporteExcelService;
    }

    // Pantalla principal del módulo de docentes
    public IActionResult Index()
    {
        return View();
    }

    // Comparar docentes
    [HttpPost]
    public IActionResult Comparar(List<IFormFile> archivos)
    {
        if (archivos == null || archivos.Count == 0)
        {
            return Content("Debe seleccionar al menos un archivo.");
        }

        var todosLosDocentes = new List<Docente>();

        foreach (var archivo in archivos)
        {
            var docentes = _excelService.LeerDocentes(archivo);
            todosLosDocentes.AddRange(docentes);
        }

        var repetidos = _comparadorService.CompararDocentes(todosLosDocentes);

        var modelo = new ResultadoViewModel
        {
            Resultados = repetidos,
            TotalArchivos = archivos.Count,
            TotalRegistros = todosLosDocentes.Count,
            TotalRepetidos = repetidos.Count,
            TotalSinDocumento = todosLosDocentes.Count(d => string.IsNullOrWhiteSpace(d.Documento))
        };

        return View("Resultado", modelo);
    }

    // Descargar reporte de docentes
    [HttpPost]
    public IActionResult DescargarExcel(List<ResultadoComparacion> resultados)
    {
        var archivoExcel = _reporteExcelService.GenerarReporte(resultados);

        return File(
            archivoExcel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Reporte_Docentes.xlsx");
    }
}