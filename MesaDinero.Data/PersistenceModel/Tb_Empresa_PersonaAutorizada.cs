namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_Empresa_PersonaAutorizada
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string IdEmpresa { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string IdPersonaAutorizada { get; set; }

        public int IdCargo { get; set; }

        public byte EstadoRegistro { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public virtual Tb_MD_Cargo Tb_MD_Cargo { get; set; }

        public virtual Tb_MD_Per_Juridica Tb_MD_Per_Juridica { get; set; }

        public virtual Tb_MD_Per_Natural Tb_MD_Per_Natural { get; set; }
    }
}
