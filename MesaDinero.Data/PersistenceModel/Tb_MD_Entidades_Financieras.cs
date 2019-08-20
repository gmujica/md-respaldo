namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Entidades_Financieras
    {
        public Tb_MD_Entidades_Financieras()
        {
            Tb_MD_ClientesDatosBancos = new HashSet<Tb_MD_ClientesDatosBancos>();
            Tb_MD_Cuentas_Bancarias = new HashSet<Tb_MD_Cuentas_Bancarias>();
            Tb_MD_Pre_ClientesDatosBancos = new HashSet<Tb_MD_Pre_ClientesDatosBancos>();
            Tb_MD_Subasta_Pago = new HashSet<Tb_MD_Subasta_Pago>();
            Tb_MD_Subasta_Pago1 = new HashSet<Tb_MD_Subasta_Pago>();
        }

        [Key]
        [StringLength(10)]
        public string vCodEntidad { get; set; }

        [StringLength(150)]
        public string vDesEntidad { get; set; }

        [StringLength(50)]
        public string vLogoEntidad { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(3)]
        public string VTipo { get; set; }

        [StringLength(25)]
        public string vFormatoCCI { get; set; }

        [StringLength(25)]
        public string vFormatoCuentaBancaria { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_ClientesDatosBancos> Tb_MD_ClientesDatosBancos { get; set; }

        public virtual ICollection<Tb_MD_Cuentas_Bancarias> Tb_MD_Cuentas_Bancarias { get; set; }

        public virtual ICollection<Tb_MD_Pre_ClientesDatosBancos> Tb_MD_Pre_ClientesDatosBancos { get; set; }

        public virtual ICollection<Tb_MD_Subasta_Pago> Tb_MD_Subasta_Pago { get; set; }

        public virtual ICollection<Tb_MD_Subasta_Pago> Tb_MD_Subasta_Pago1 { get; set; }
    }
}
