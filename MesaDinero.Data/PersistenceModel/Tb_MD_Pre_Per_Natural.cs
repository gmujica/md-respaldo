namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Pre_Per_Natural
    {
        public Tb_MD_Pre_Per_Natural()
        {
            Tb_MD_Pre_Per_Juridica = new HashSet<Tb_MD_Pre_Per_Juridica>();
        }

        [Key]
        public int Id_Pre_Per_Natural { get; set; }

        [Required]
        [StringLength(20)]
        public string vNumDocumento { get; set; }

        [StringLength(50)]
        public string vUbigeoDireccion { get; set; }

        [StringLength(150)]
        public string vDireccion { get; set; }

        [StringLength(15)]
        public string vTelefonoMovil { get; set; }

        [StringLength(50)]
        public string vOcupacion { get; set; }

        [Required]
        [StringLength(50)]
        public string vMailContacto { get; set; }

        [StringLength(4)]
        public string vIdPaisOrigen { get; set; }

        [StringLength(1)]
        public string vFlgExpuestoPoliticamente { get; set; }

        public DateTime dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(5)]
        public string vTipoPersona { get; set; }

        public int? iCodDistrito { get; set; }

        public int? iCodDepartamento { get; set; }

        [StringLength(200)]
        public string vNombre { get; set; }

        [StringLength(200)]
        public string vApellido { get; set; }

        [StringLength(200)]
        public string vApellidoMat { get; set; }

        [Required]
        [StringLength(10)]
        public string vTipoDocumento { get; set; }

        public DateTime? vFechaNacimiento { get; set; }

        public int? iOrigenFondos { get; set; }

        public byte? vEstadoRegistro { get; set; }

        [StringLength(5)]
        public string vPreCelular { get; set; }

        public int? iCodProvincia { get; set; }

        [StringLength(1)]
        public string vFlgSituacionLaboral { get; set; }

        public int Id_Pre_Cliente { get; set; }

        [StringLength(150)]
        public string NombreEntidadExpuesto { get; set; }

        [StringLength(50)]
        public string CargoExpuesto { get; set; }

        public virtual Tb_MD_Departamento Tb_MD_Departamento { get; set; }

        public virtual Tb_MD_Distrito Tb_MD_Distrito { get; set; }

        public virtual Tb_MD_OrigenFondo Tb_MD_OrigenFondo { get; set; }

        public virtual Tb_MD_Pais Tb_MD_Pais { get; set; }

        public virtual ICollection<Tb_MD_Pre_Per_Juridica> Tb_MD_Pre_Per_Juridica { get; set; }

        public virtual Tb_MD_Provincia Tb_MD_Provincia { get; set; }
    }
}
