namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_TipoCuentaBancaria
    {
        public Tb_MD_TipoCuentaBancaria()
        {
            Tb_MD_ClientesDatosBancos = new HashSet<Tb_MD_ClientesDatosBancos>();
            Tb_MD_Cuentas_Bancarias = new HashSet<Tb_MD_Cuentas_Bancarias>();
            Tb_MD_Pre_ClientesDatosBancos = new HashSet<Tb_MD_Pre_ClientesDatosBancos>();
        }

        [Key]
        public int IdTipoCuenta { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public byte EstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_ClientesDatosBancos> Tb_MD_ClientesDatosBancos { get; set; }

        public virtual ICollection<Tb_MD_Cuentas_Bancarias> Tb_MD_Cuentas_Bancarias { get; set; }

        public virtual ICollection<Tb_MD_Pre_ClientesDatosBancos> Tb_MD_Pre_ClientesDatosBancos { get; set; }
    }
}
