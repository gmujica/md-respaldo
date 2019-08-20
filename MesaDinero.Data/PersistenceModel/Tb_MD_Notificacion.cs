namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }

        [StringLength(50)]
        public string IdUsuario { get; set; }

        public int? IdCliente { get; set; }

        public int? Tipo { get; set; }
        
        [StringLength(200)]
        public string Url {get;set;}
        
        [StringLength(500)]
        public string Titulo { get; set; }

        [StringLength(10)]
        public string vNumeroSubasta { get; set; }

        [StringLength(10)]
        public string vEstadoSubasta { get; set; }
        

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public byte iEstadoRegistro { get; set; }
    }
}
