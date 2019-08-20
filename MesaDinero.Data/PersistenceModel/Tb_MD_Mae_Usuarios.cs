namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Mae_Usuarios
    {
        public Tb_MD_Mae_Usuarios()
        {
            Tb_MD_PerfilUsuario = new HashSet<Tb_MD_PerfilUsuario>();
        }

        [StringLength(1)]
        public string vTipoPersona { get; set; }

        [Required]
        [StringLength(50)]
        public string vEmailUsuario { get; set; }

        public byte vEstadoRegistro { get; set; }

        [StringLength(5)]
        public string vTipoUsuario { get; set; }

        [StringLength(200)]
        public string vPassword { get; set; }

        [StringLength(1)]
        public string vFlgValidado { get; set; }

        [StringLength(20)]
        public string vUsuarioValidacion { get; set; }

        public DateTime dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        public DateTime? dFechaValidacionPaso1 { get; set; }

        public DateTime? dFechaValidacionPaso2 { get; set; }

        public DateTime? dFechaValidacionPaso3 { get; set; }

        public DateTime? dFechaValidacionPaso4 { get; set; }

        public DateTime? dFechaValidacionPaso5 { get; set; }

        [StringLength(10)]
        public string vCelular { get; set; }

        [StringLength(10)]
        public string vTelefonoFijo { get; set; }

        [StringLength(20)]
        public string vRucEmpresa { get; set; }

        [StringLength(20)]
        public string vNroDocumento { get; set; }

        [Key]
        public int iIdUsuario { get; set; }

        public Guid? SecretId { get; set; }

        public virtual ICollection<Tb_MD_PerfilUsuario> Tb_MD_PerfilUsuario { get; set; }
    }
}
