namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Observacion_Cliente
    {
        [Key]
        public int iIdObservacion { get; set; }

        [StringLength(400)]
        public string Mensaje { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public int? UsuarioCreacion { get; set; }

        public int? IdCliente { get; set; }

        [StringLength(1)]
        public string Estado { get; set; }

        [StringLength(1)]
        public string RolObservador { get; set; }

        [StringLength(3)]
        public string TipoRegistroObservacion { get; set; }
    }
}
