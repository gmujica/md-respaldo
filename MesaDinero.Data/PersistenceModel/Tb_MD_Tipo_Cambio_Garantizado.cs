namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Tipo_Cambio_Garantizado
    {
        [Required]
        [StringLength(20)]
        public string vNumDocPartner { get; set; }

        public DateTime dFechaCreacion { get; set; }

        public decimal? nRango1_n { get; set; }

        public decimal? nValorRangoMinimo { get; set; }

        public decimal? nValorRangoMaximo { get; set; }

        [Required]
        [StringLength(10)]
        public string vTipoMoneda { get; set; }

        public decimal? nValorCompra { get; set; }

        public decimal? nValorVenta { get; set; }

        public decimal? nPorComision { get; set; }

        public decimal? nSpreed { get; set; }

        [StringLength(1)]
        public string vEstadoRegistro { get; set; }

        [StringLength(15)]
        public string vRUCEntidad { get; set; }

        [StringLength(15)]
        public string VRUCUsuario { get; set; }

        [Key]
        public int iIdTipoCambioGarantizado { get; set; }

        public bool UltimoCambioGarantizado { get; set; }
    }
}
