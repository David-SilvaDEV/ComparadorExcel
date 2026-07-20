using ComparadorExcel.Models;

namespace ComparadorExcel.Services;

public class ReporteTemporalService
{
    private readonly Dictionary<string, List<ResultadoComparacion>> _reportes = new();

    public string Guardar(List<ResultadoComparacion> resultados)
    {
        var id = Guid.NewGuid().ToString();

        _reportes[id] = resultados;

        return id;
    }

    public List<ResultadoComparacion>? Obtener(string id)
    {
        if (_reportes.TryGetValue(id, out var resultados))
        {
            return resultados;
        }

        return null;
    }

    public void Eliminar(string id)
    {
        _reportes.Remove(id);
    }
}