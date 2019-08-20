namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_ClienteUsuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(80)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Password { get; set; }

        public byte EstadoREgistro { get; set; }

        [Required]
        [StringLength(150)]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(3)]
        public string Iniciales { get; set; }

        public DateTime Creado { get; set; }

        public DateTime? Modificado { get; set; }

        public int IdCliente { get; set; }

        public int TipoCliente { get; set; }

        [StringLength(20)]
        public string vNroDocumento { get; set; }

        public Guid? SecredId { get; set; }

        public virtual Tb_MD_Clientes Tb_MD_Clientes { get; set; }
    }
}
