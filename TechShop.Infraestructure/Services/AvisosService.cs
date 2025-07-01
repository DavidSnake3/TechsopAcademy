using Microsoft.Extensions.Caching.Memory;

public class AvisosService
{
    private const string CacheKey = "AvisosActivos";
    private readonly IMemoryCache _cache;

    public AvisosService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public List<Aviso> ObtenerAvisos()
    {
        if (!_cache.TryGetValue(CacheKey, out List<Aviso> avisos))
            avisos = new List<Aviso>();

        avisos = avisos.Where(a => a.FechaCreacion > DateTime.UtcNow.AddDays(-7)).ToList();
        _cache.Set(CacheKey, avisos, TimeSpan.FromDays(7));
        return avisos;
    }

    public void AgregarAviso(string texto)
    {
        var avisos = ObtenerAvisos();
        avisos.Add(new Aviso { Texto = texto, FechaCreacion = DateTime.UtcNow });
        _cache.Set(CacheKey, avisos, TimeSpan.FromDays(7));
    }

    public void EliminarAviso(DateTime fecha)
    {
        var avisos = ObtenerAvisos();
        avisos.RemoveAll(a => a.FechaCreacion == fecha);
        _cache.Set(CacheKey, avisos, TimeSpan.FromDays(7));
    }

    public void EditarAviso(DateTime fecha, string nuevoTexto)
    {
        var avisos = ObtenerAvisos();
        var aviso = avisos.FirstOrDefault(a => a.FechaCreacion == fecha);
        if (aviso != null)
        {
            aviso.Texto = nuevoTexto;
            _cache.Set(CacheKey, avisos, TimeSpan.FromDays(7));
        }
    }
}

public class Aviso
{
    public string Texto { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
}
