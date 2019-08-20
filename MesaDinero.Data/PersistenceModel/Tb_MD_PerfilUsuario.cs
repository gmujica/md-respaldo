namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_PerfilUsuario
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUsuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPerfil { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual Tb_MD_Mae_Usuarios Tb_MD_Mae_Usuarios { get; set; }

        public virtual Tb_MD_Perfiles Tb_MD_Perfiles { get; set; }
    }
}
