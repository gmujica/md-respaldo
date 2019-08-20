namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Departamento
    {
        public Tb_MD_Departamento()
        {
            Tb_MD_Per_Juridica = new HashSet<Tb_MD_Per_Juridica>();
            Tb_MD_Per_Natural = new HashSet<Tb_MD_Per_Natural>();
            Tb_MD_Pre_Per_Juridica = new HashSet<Tb_MD_Pre_Per_Juridica>();
            Tb_MD_Pre_Per_Natural = new HashSet<Tb_MD_Pre_Per_Natural>();
            Tb_MD_Ubigeo = new HashSet<Tb_MD_Ubigeo>();
        }

        [Key]
        public int IdDepartamento { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(4)]
        public string idPais { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_Per_Juridica> Tb_MD_Per_Juridica { get; set; }

        public virtual ICollection<Tb_MD_Per_Natural> Tb_MD_Per_Natural { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Juridica> Tb_MD_Pre_Per_Juridica { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Natural> Tb_MD_Pre_Per_Natural { get; set; }

        public virtual ICollection<Tb_MD_Ubigeo> Tb_MD_Ubigeo { get; set; }

        public virtual Tb_MD_Pais Tb_MD_Pais { get; set; }
    }
}
