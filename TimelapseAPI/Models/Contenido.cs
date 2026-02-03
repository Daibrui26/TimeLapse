using System;

namespace TimelapseAPI.Models
{
    public class Contenido
    {
        public int IdContenido { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string ContenidoTexto { get; set; } = string.Empty; // Para no usar "Contenido" como nombre
        public DateTime FechaSubida { get; set; }
        public int IdCapsula { get; set; }
    }
}