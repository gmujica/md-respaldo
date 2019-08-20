namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_UsuariosAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUsuario { get; set; }

        [StringLength(150)]
        public string NombreUsuario { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Rol { get; set; }

        [StringLength(150)]
        public string NombrePersonal { get; set; }

        [StringLength(20)]
        public string NroDocumento { get; set; }

    }
}
