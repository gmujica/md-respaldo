namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Pre_ClientesDatosBancos
    {
        [Key]
        public long iIdDatosBank { get; set; }

        [StringLength(10)]
        public string vBanco { get; set; }

        [StringLength(10)]
        public string vMoneda { get; set; }

        [StringLength(100)]
        public string vNroCuenta { get; set; }

        [StringLength(100)]
        public string vCCI { get; set; }

        public int? iTipoCuenta { get; set; }

        public int? iIdCliente { get; set; }

        public byte? vEstadoRegistro { get; set; }

        public int? vTipoPersona { get; set; }

        [StringLength(20)]
        public string vNroDocumento { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(15)]
        public string vRUCUsuario { get; set; }

        [StringLength(20)]
        public string vRegPAutorizada { get; set; }

        public Guid vSecredId { get; set; }

        public virtual Tb_MD_Entidades_Financieras Tb_MD_Entidades_Financieras { get; set; }

        public virtual Tb_MD_TipoMoneda Tb_MD_TipoMoneda { get; set; }

        public virtual Tb_MD_TipoCuentaBancaria Tb_MD_TipoCuentaBancaria { get; set; }
    }
}
