namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Cuentas_Email
    {
        [Required]
        [StringLength(20)]
        public string vNumDocumento { get; set; }

       
        [StringLength(50)]
        public string vMailContacto { get; set; }

        [Required]
        [StringLength(20)]
        public string iIdPerjuridica { get; set; }

        [Required]
        [StringLength(20)]
        public string vRol { get; set; }

        public byte? vEstadoRegsitro { get; set; }

        [Key]
        public long iIdCuentasEmail { get; set; }
    }
}
