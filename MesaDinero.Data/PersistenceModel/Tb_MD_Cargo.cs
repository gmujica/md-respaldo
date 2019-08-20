namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Cargo
    {
        public Tb_MD_Cargo()
        {
            Tb_MD_Empresa_PersonaAutorizada = new HashSet<Tb_MD_Empresa_PersonaAutorizada>();
            Tb_MD_Pre_Empresa_PersonaAutorizada = new HashSet<Tb_MD_Pre_Empresa_PersonaAutorizada>();
        }

        [Key]
        public int IdCargo { get; set; }

        [Required]
        [StringLength(70)]
        public string Nombre { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_Empresa_PersonaAutorizada> Tb_MD_Empresa_PersonaAutorizada { get; set; }

        public virtual ICollection<Tb_MD_Pre_Empresa_PersonaAutorizada> Tb_MD_Pre_Empresa_PersonaAutorizada { get; set; }
    }
}
