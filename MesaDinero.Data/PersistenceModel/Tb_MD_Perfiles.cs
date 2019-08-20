namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Perfiles
    {
        public Tb_MD_Perfiles()
        {
            Tb_MD_PefilPagina = new HashSet<Tb_MD_PefilPagina>();
            Tb_MD_PerfilUsuario = new HashSet<Tb_MD_PerfilUsuario>();
        }

        [Key]
        public int IdPerfil { get; set; }

        [Required]
        [StringLength(150)]
        public string NombrePerfil { get; set; }

        public byte EstadoRegistro { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(20)]
        public string Modulo { get; set; }

        public virtual ICollection<Tb_MD_PefilPagina> Tb_MD_PefilPagina { get; set; }

        public virtual ICollection<Tb_MD_PerfilUsuario> Tb_MD_PerfilUsuario { get; set; }
    }
}
