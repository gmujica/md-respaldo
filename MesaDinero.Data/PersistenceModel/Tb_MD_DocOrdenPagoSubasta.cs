namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_DocOrdenPagoSubasta
    {
        [Key]
        public int nIdOrden { get; set; }

        public int nNumeroSubasta { get; set; }

        public int nIdDocumento { get; set; }

        public DateTime dFechaRegistro { get; set; }

        public int vGeneradoPor { get; set; }

        public int iEstadoRegistro { get; set; }

        public virtual Tb_MD_Documentos Tb_MD_Documentos { get; set; }

        public virtual Tb_MD_Subasta Tb_MD_Subasta { get; set; }
    }
}
