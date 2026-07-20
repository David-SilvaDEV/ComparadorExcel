using System.Collections.Generic;

namespace ComparadorExcel.Models;

public class ResultadoViewModel
{
    public List<ResultadoComparacion> Resultados { get; set; } = new();

    public int TotalArchivos { get; set; }

    public int TotalRegistros { get; set; }

    public int TotalRepetidos { get; set; }

    public int TotalSinDocumento { get; set; }

    // Archivos que no pudieron ser procesados
    public List<RegistroConError> Errores { get; set; } = new();
}