namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_Accionistas
    {
        public DateTime dFechaCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [Required]
        [StringLength(20)]
        public string IdPersonaNatural { get; set; }

        [Required]
        [StringLength(20)]
        public string IdEmpresa { get; set; }

        [Key]
        public int IdAccionista { get; set; }

        public byte? EstadoRegistro { get; set; }

        public virtual Tb_MD_Per_Juridica Tb_MD_Per_Juridica { get; set; }

        public virtual Tb_MD_Per_Natural Tb_MD_Per_Natural { get; set; }
    }
}
