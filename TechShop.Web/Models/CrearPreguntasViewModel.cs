namespace TechShop.Web.Models
{
    public class CrearPreguntasViewModel
    {
        public List<Pregunta> Preguntas { get; set; } = new();
    }

    public class Pregunta
    {
        public string TextoPregunta { get; set; }

        public string TipoPregunta { get; set; }

        public List<OpcionesRespuesta> Opciones { get; set; } = new();
    }

    public partial class OpcionesRespuesta
    {

        public string TextoOpcion { get; set; }

        public bool EsCorrecta { get; set; }

    }
}
