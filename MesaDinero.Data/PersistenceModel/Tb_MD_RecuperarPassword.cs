namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_RecuperarPassword
    {
        [Key]
        public int IdCambiarPassWord { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public int? IdUsuario { get; set; }

        public Guid SecredId { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(5)]
        public string TipoUsuario { get; set; }
    }
}
