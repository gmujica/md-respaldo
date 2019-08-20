namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Proceso_Subasta
    {
        [Key]
        [StringLength(1)]
        public string vIdProceso { get; set; }

        [StringLength(100)]
        public string vNombre { get; set; }

        [StringLength(1)]
        public string vSiguiente { get; set; }

        public bool? bFinalSubasta { get; set; }
    }
}
