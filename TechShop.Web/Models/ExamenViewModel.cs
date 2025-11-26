namespace TechShop.Web.Models.ViewModels
{
    public class ExamenVM
    {
        public int CapacitacionId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string DescripcionCorta { get; set; }
        public List<PreguntaVM> Preguntas { get; set; } = new();
    }

    public class PreguntaVM
    {
        public int PreguntaId { get; set; }
        public string TextoPregunta { get; set; }
        public string TipoPregunta { get; set; }     // "MultipleChoice" o "Abierta"
        public List<OpcionVM> Opciones { get; set; } = new();
        public int? OpcionSeleccionada { get; set; } // Para radio buttons
        public List<int> OpcionesSeleccionadas { get; set; } = new(); // para checkbox
    }

    public class OpcionVM
    {
        public int OpcionId { get; set; }
        public string TextoOpcion { get; set; }
    }

    public class ResultadoDetalleVM
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string TextoPregunta { get; set; }
        public string RespuestaUsuario { get; set; }
        public string RespuestaCorrecta { get; set; }
        public bool EsCorrecta { get; set; }
    }

    public class ResultadosVM
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int CapacitacionId { get; set; }
        public int TotalPreguntas { get; set; }
        public int Correctas { get; set; }
        public decimal Nota { get; set; }
        public bool Aprobado { get; set; }
        public List<ResultadoDetalleVM> Detalles { get; set; } = new();
    }

    public class FinalizadosVM
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string DescripcionCorta { get; set; }
        public string DescripcionLarga { get; set; }
        public int? DuracionHoras { get; set; }    
        public string Dificultad { get; set; }
        public byte[] Foto { get; set; }

        public int TotalPreguntas { get; set; }
        public int Correctas { get; set; }
        public decimal Nota { get; set; }
        public bool Aprobado { get; set; }
        public List<ResultadoDetalleVM> Detalles { get; set; }
    }

}
