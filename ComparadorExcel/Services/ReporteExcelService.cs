using ClosedXML.Excel;
using ComparadorExcel.Models;

namespace ComparadorExcel.Services;

public class ReporteExcelService
{
    public byte[] GenerarReporte(
        List<ResultadoComparacion> resultados)
    {
        using var workbook = new XLWorkbook();

        var hoja = workbook.Worksheets.Add(
            "Estudiantes Repetidos");

        // ===== TÍTULO =====

        hoja.Cell("A1").Value =
            "REPORTE DE ESTUDIANTES REPETIDOS";

        hoja.Range("A1:F1").Merge();

        hoja.Cell("A1").Style.Font.Bold = true;

        hoja.Cell("A1").Style.Font.FontSize = 18;

        hoja.Cell("A1").Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        hoja.Cell("A1").Style.Fill.BackgroundColor =
            XLColor.DarkBlue;

        hoja.Cell("A1").Style.Font.FontColor =
            XLColor.White;


        // ===== FECHA =====

        hoja.Cell("A2").Value =
            $"Fecha de generación: " +
            $"{DateTime.Now:dd/MM/yyyy HH:mm}";


        // ===== ENCABEZADOS =====

        int filaEncabezado = 4;

        hoja.Cell(filaEncabezado, 1).Value =
            "Documento";

        hoja.Cell(filaEncabezado, 2).Value =
            "Nombre";

        hoja.Cell(filaEncabezado, 3).Value =
            "Cantidad";

        hoja.Cell(filaEncabezado, 4).Value =
            "Programas / Áreas";

        hoja.Cell(filaEncabezado, 5).Value =
            "Instituciones";

        hoja.Cell(filaEncabezado, 6).Value =
            "Archivos";


        var encabezado = hoja.Range(
            filaEncabezado,
            1,
            filaEncabezado,
            6);


        encabezado.Style.Font.Bold = true;

        encabezado.Style.Font.FontColor =
            XLColor.White;

        encabezado.Style.Fill.BackgroundColor =
            XLColor.SteelBlue;

        encabezado.Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;


        // ===== DATOS =====

        int fila = filaEncabezado + 1;


        // Protección por si la lista llega nula
        resultados ??= new List<ResultadoComparacion>();


        foreach (var resultado in resultados)
        {
            if (resultado == null)
                continue;


            hoja.Cell(fila, 1).Value =
                resultado.Documento ?? string.Empty;


            hoja.Cell(fila, 2).Value =
                resultado.Nombre ?? string.Empty;


            hoja.Cell(fila, 3).Value =
                resultado.Cantidad;


            hoja.Cell(fila, 4).Value =
                string.Join(
                    ", ",
                    resultado.Programas ??
                    new List<string>());


            hoja.Cell(fila, 5).Value =
                string.Join(
                    ", ",
                    resultado.Instituciones ??
                    new List<string>());


            hoja.Cell(fila, 6).Value =
                string.Join(
                    ", ",
                    resultado.Archivos ??
                    new List<string>());


            fila++;
        }


        // ===== BORDES =====

        // Solo aplicamos bordes si hay datos
        if (fila > filaEncabezado + 1)
        {
            var tabla = hoja.Range(
                filaEncabezado,
                1,
                fila - 1,
                6);


            tabla.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;


            tabla.Style.Border.InsideBorder =
                XLBorderStyleValues.Thin;


            // ===== FILTROS =====

            tabla.SetAutoFilter();
        }


        // ===== CONGELAR ENCABEZADO =====

        hoja.SheetView.FreezeRows(
            filaEncabezado);


        // ===== CENTRAR COLUMNAS =====

        hoja.Column(1)
            .Style
            .Alignment
            .Horizontal =
            XLAlignmentHorizontalValues.Center;


        hoja.Column(3)
            .Style
            .Alignment
            .Horizontal =
            XLAlignmentHorizontalValues.Center;


        // ===== AJUSTAR ANCHO =====

        hoja.Columns()
            .AdjustToContents();


        // ===== GENERAR ARCHIVO =====

        using var stream =
            new MemoryStream();


        workbook.SaveAs(stream);


        return stream.ToArray();
    }
}