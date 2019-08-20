namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Pre_Empresa_PersonaAutorizada
    {
        [Key]
        public int IdEmpresaPersonaAutorizada { get; set; }

        [Required]
        [StringLength(20)]
        public string IdEmpresa { get; set; }

        [Required]
        [StringLength(20)]
        public string IdPersonaAutorizada { get; set; }

        [StringLength(10)]
        public string vTipoDocumento { get; set; }

        [StringLength(200)]
        public string vNombre { get; set; }

        [StringLength(200)]
        public string vApellido { get; set; }

        [StringLength(200)]
        public string vApellidoMat { get; set; }

        [StringLength(15)]
        public string vTelefonoMovil { get; set; }

        [StringLength(5)]
        public string vPreCelular { get; set; }

        [StringLength(150)]
        public string vMailContacto { get; set; }

        public int IdPreCliente { get; set; }

        public int IdCargo { get; set; }

        public byte EstadoRegistro { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Principal { get; set; }

        public virtual Tb_MD_Cargo Tb_MD_Cargo { get; set; }
    }
}
