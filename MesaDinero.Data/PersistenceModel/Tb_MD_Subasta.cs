namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Subasta
    {
        public Tb_MD_Subasta()
        {
            Tb_MD_DocOrdenPagoSubasta = new HashSet<Tb_MD_DocOrdenPagoSubasta>();
            Tb_MD_Subasta_Detalle = new HashSet<Tb_MD_Subasta_Detalle>();
        }

        [StringLength(20)]
        public string vNumDocumento { get; set; }

        [StringLength(1)]
        public string vTipoOperacion { get; set; }

        public DateTime? dFechaInicioSubasta { get; set; }

        [StringLength(1)]
        public string vEstadoSubasta { get; set; }

        public int? nTiempoSubasta { get; set; }

        public int? nTiempoConfirmacion { get; set; }

        public int? nTiempoTransFideicomiso { get; set; }

        public DateTime? dFechaFinSubasta { get; set; }

        [StringLength(50)]
        public string vNumInsPago { get; set; }

        [StringLength(20)]
        public string vNumDocPartner { get; set; }

        public DateTime? dFechaSeleccion { get; set; }

        [StringLength(1)]
        public string vEstadoRegistro { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(5)]
        public string vTipoPersona { get; set; }

        [StringLength(15)]
        public string vRUCUsuario { get; set; }

        [StringLength(15)]
        public string vRUCEntidad { get; set; }

        [Key]
        public int nNumeroSubasta { get; set; }

        public int? nTiempoConfitmacionOperacion { get; set; }

        public int? nTiempoConfitmacionPago { get; set; }

        public DateTime? dFechaConfirmacionOperacion { get; set; }

        public DateTime? dFechaConfirmacionPago { get; set; }

        [StringLength(10)]
        public string vMonedaEnviaCliente { get; set; }

        [StringLength(10)]
        public string vMonedaRecibeCliente { get; set; }

        public decimal? nMontoEnviaCliente { get; set; }

        public decimal? nMontiRecibeCliente { get; set; }

        public Guid SecredId { get; set; }

        public int IdCliente { get; set; }

        public int? TipoCambioGanador { get; set; }

        public long? cuentaBancoOrigen { get; set; }

        public long? cuentaBancoDestino { get; set; }

        [StringLength(30)]
        public string NroOperacionPago { get; set; }

        [StringLength(200)]
        public string NombreCliente { get; set; }

        public bool bInitSubasta { get; set; }


        public virtual ICollection<Tb_MD_DocOrdenPagoSubasta> Tb_MD_DocOrdenPagoSubasta { get; set; }

        public virtual ICollection<Tb_MD_Subasta_Detalle> Tb_MD_Subasta_Detalle { get; set; }

        public virtual Tb_MD_Subasta_Pago Tb_MD_Subasta_Pago { get; set; }
    }
}
