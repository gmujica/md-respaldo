namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_ActividadEconomica
    {
        public Tb_MD_ActividadEconomica()
        {
            Tb_MD_Per_Juridica = new HashSet<Tb_MD_Per_Juridica>();
            Tb_MD_Pre_Per_Juridica = new HashSet<Tb_MD_Pre_Per_Juridica>();
            Tb_MD_Pre_Per_Juridica1 = new HashSet<Tb_MD_Pre_Per_Juridica>();
        }

        [Key]
        public int IdActividadEconomica { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_Per_Juridica> Tb_MD_Per_Juridica { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Juridica> Tb_MD_Pre_Per_Juridica { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Juridica> Tb_MD_Pre_Per_Juridica1 { get; set; }
    }
}
