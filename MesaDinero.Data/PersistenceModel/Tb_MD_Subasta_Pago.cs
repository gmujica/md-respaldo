namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Subasta_Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int nNumeroSubasta { get; set; }

        [StringLength(5)]
        public string vTipoPersona { get; set; }

        [StringLength(20)]
        public string vNroDocumento { get; set; }

        public DateTime? dFechaInformePago { get; set; }

        [StringLength(50)]
        public string vNumOperacionPago { get; set; }

        [StringLength(10)]
        public string vCodBancoCliente { get; set; }

        [StringLength(50)]
        public string vNumeroCuenta { get; set; }

        [StringLength(10)]
        public string vTipoMonedaTransferida { get; set; }

        public decimal? nMontoTransferido { get; set; }

        [StringLength(10)]
        public string vCodBancoFideicomiso { get; set; }

        [StringLength(50)]
        public string vNumeroCuentaFideicomiso { get; set; }

        public DateTime? dFechaValidacionOperaciones { get; set; }

        [StringLength(20)]
        public string vNumDocValidaDepositoOperaciones { get; set; }

        [StringLength(1)]
        public string vEstadoValOperador { get; set; }

        public DateTime? dFechaValidacionFideicomiso { get; set; }

        [StringLength(20)]
        public string vNumDocValidaDepositoFideicomiso { get; set; }

        [StringLength(1)]
        public string vEstadoValFideicomiso { get; set; }

        public DateTime? dFechaEnvioCorfid { get; set; }

        public DateTime? dFechaInformeContravalor { get; set; }

        [StringLength(50)]
        public string vNumOpeBancoACliente { get; set; }

        [StringLength(10)]
        public string vCodBancoDestinoCliente { get; set; }

        public decimal? nMontoTransferidoACliente { get; set; }

        [StringLength(10)]
        public string vTipoMonedaDestinoCliente { get; set; }

        [StringLength(50)]
        public string vNumeroCuentaDestinoCliente { get; set; }

        [StringLength(400)]
        public string vObservacion { get; set; }

        public byte iEstadoRegistro { get; set; }

        [StringLength(13)]
        public string vNumeroLiquidacion { get; set; }

        public DateTime? dFechaRegLiq { get; set; }

        [StringLength(1)]
        public string vEstadoLiq { get; set; }

        public int? iUsuarioLiqLmd { get; set; }

        [StringLength(80)]
        public string vUsuarioLiqLmd { get; set; }

        public bool? vValidaPart { get; set; }

        public int? iUsuarioPart { get; set; }

        [StringLength(80)]
        public string vUsuarioPart { get; set; }

        public DateTime? dFechaValPart { get; set; }

        [StringLength(100)]
        public string vNroVoucherFid { get; set; }

        public int? iUsuarioFid { get; set; }

        [StringLength(80)]
        public string vUsuarioFid { get; set; }

        public DateTime? dFechaPagFid { get; set; }


        public int? iUsuarioPartnerTes { get; set; }

        [StringLength(80)]
        public string vUsuarioPartnerTes { get; set; }

        public DateTime? dFechaPagPartnerTes{ get; set; }


        public virtual Tb_MD_Entidades_Financieras Tb_MD_Entidades_Financieras { get; set; }

        public virtual Tb_MD_Entidades_Financieras Tb_MD_Entidades_Financieras1 { get; set; }

        public virtual Tb_MD_Subasta Tb_MD_Subasta { get; set; }
    }
}
