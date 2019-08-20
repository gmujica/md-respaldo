namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Cuentas_Bancarias
    {
        [Key]
        public int nIdCuentaEmpresa { get; set; }

        [Required]
        [StringLength(20)]
        public string vNumDocumento { get; set; }

        [StringLength(10)]
        public string vCodEntidad { get; set; }

        [StringLength(50)]
        public string vNumCuenta { get; set; }

        [StringLength(50)]
        public string vNumCCI { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        public int iTipoCuenta { get; set; }

        [StringLength(10)]
        public string vMonedaCuenta { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual Tb_MD_TipoCuentaBancaria Tb_MD_TipoCuentaBancaria { get; set; }

        public virtual Tb_MD_Entidades_Financieras Tb_MD_Entidades_Financieras { get; set; }

        public virtual Tb_MD_TipoMoneda Tb_MD_TipoMoneda { get; set; }

        public virtual Tb_MD_Per_Juridica Tb_MD_Per_Juridica { get; set; }
    }
}
