namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_TipoMoneda
    {
        public Tb_MD_TipoMoneda()
        {
            Tb_MD_ClientesDatosBancos = new HashSet<Tb_MD_ClientesDatosBancos>();
            Tb_MD_Cuentas_Bancarias = new HashSet<Tb_MD_Cuentas_Bancarias>();
            Tb_MD_Pre_ClientesDatosBancos = new HashSet<Tb_MD_Pre_ClientesDatosBancos>();
        }

        [Key]
        [StringLength(10)]
        public string vCodMoneda { get; set; }

        [StringLength(150)]
        public string vDesMoneda { get; set; }

        [StringLength(5)]
        public string vSimboloMoneda { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(15)]
        public string vRUCUsuario { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_ClientesDatosBancos> Tb_MD_ClientesDatosBancos { get; set; }

        public virtual ICollection<Tb_MD_Cuentas_Bancarias> Tb_MD_Cuentas_Bancarias { get; set; }

        public virtual ICollection<Tb_MD_Pre_ClientesDatosBancos> Tb_MD_Pre_ClientesDatosBancos { get; set; }
    }
}
