namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Transaccion
    {
        [Key]
        [StringLength(20)]
        public string vCodTransaccion { get; set; }

        [Required]
        [StringLength(50)]
        public string vNomTransaccion { get; set; }

        [Required]
        [StringLength(200)]
        public string vDesTransaccion { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(1)]
        public string vEstadoRegistro { get; set; }

        [StringLength(15)]
        public string vRUCUsuario { get; set; }

        [Required]
        [StringLength(20)]
        public string vIdUsuario { get; set; }
    }
}
