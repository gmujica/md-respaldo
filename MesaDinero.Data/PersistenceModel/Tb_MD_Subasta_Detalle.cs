namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Subasta_Detalle
    {
        public int nNumeroSubasta { get; set; }

        [Required]
        [StringLength(1)]
        public string vTipoDetalle { get; set; }

        [Required]
        [StringLength(20)]
        public string vNumDocPartner { get; set; }

        public decimal? nValorRangoMinimo { get; set; }

        public decimal? nValorRangoMaximo { get; set; }

        [StringLength(10)]
        public string vTipoMoneda { get; set; }

        public decimal? nValorCompra { get; set; }

        public decimal? nValorVenta { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(1)]
        public string vEstadoRegistro { get; set; }

        [StringLength(15)]
        public string vRUCEntidad { get; set; }

        [Key]
        public int iIdSubastaDEtalle { get; set; }

        public bool UltimoTipoCambioGarantizado { get; set; }

        [StringLength(150)]
        public string RazonSocial { get; set; }

        public decimal? TipoCambio { get; set; }

        public Guid? SecredId_Subasta { get; set; }

        public virtual Tb_MD_Subasta Tb_MD_Subasta { get; set; }
    }
}
