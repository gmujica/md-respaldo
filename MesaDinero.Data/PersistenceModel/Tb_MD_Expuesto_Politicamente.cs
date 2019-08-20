namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Expuesto_Politicamente
    {
        [Required]
        [StringLength(20)]
        public string vNumDocumento { get; set; }

        [StringLength(15)]
        public string vRUCEntidad { get; set; }

        [StringLength(150)]
        public string vNombreEntidad { get; set; }

        [StringLength(50)]
        public string vCargo { get; set; }

        public DateTime? dFechaInicioActivadad { get; set; }

        public DateTime? dFechaFinActividad { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(1)]
        public string iEstadoRegistro { get; set; }

        [StringLength(5)]
        public string vTipoPersona { get; set; }

        [Key]
        public int IdExpuestoPoliticamente { get; set; }
    }
}
