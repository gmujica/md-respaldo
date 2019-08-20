namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_OrigenFondo
    {
        public Tb_MD_OrigenFondo()
        {
            Tb_MD_Per_Juridica = new HashSet<Tb_MD_Per_Juridica>();
            Tb_MD_Per_Natural = new HashSet<Tb_MD_Per_Natural>();
            Tb_MD_Pre_Per_Juridica = new HashSet<Tb_MD_Pre_Per_Juridica>();
            Tb_MD_Pre_Per_Natural = new HashSet<Tb_MD_Pre_Per_Natural>();
        }

        [Key]
        public int IdOrigenFondos { get; set; }

        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_Per_Juridica> Tb_MD_Per_Juridica { get; set; }

        public virtual ICollection<Tb_MD_Per_Natural> Tb_MD_Per_Natural { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Juridica> Tb_MD_Pre_Per_Juridica { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Natural> Tb_MD_Pre_Per_Natural { get; set; }
    }
}
