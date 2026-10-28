using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventos.Web.Models
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventoId { get; set; }

        [ForeignKey("EventoId")]
        public Evento? Evento { get; set; }

        [Required]
        public int AsistenteId { get; set; }

        [ForeignKey("AsistenteId")]
        public Asistente? Asistente { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Confirmada";
    }
}