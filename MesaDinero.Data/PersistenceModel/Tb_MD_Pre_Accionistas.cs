namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Pre_Accionistas
    {
        public DateTime dFechaCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [Required]
        [StringLength(20)]
        public string IdPersonaNatural { get; set; }

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

        [Required]
        [StringLength(20)]
        public string IdEmpresa { get; set; }

        public int IdPreCliente { get; set; }

        [Key]
        public int IdAccionista { get; set; }

        public byte? EstadoRegistro { get; set; }
    }
}
