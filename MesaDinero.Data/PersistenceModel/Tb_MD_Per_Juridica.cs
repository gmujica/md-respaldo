namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Per_Juridica
    {
        public Tb_MD_Per_Juridica()
        {
            Tb_MD_Accionistas = new HashSet<Tb_MD_Accionistas>();
            Tb_MD_Cuentas_Bancarias = new HashSet<Tb_MD_Cuentas_Bancarias>();
            Tb_MD_Empresa_PersonaAutorizada = new HashSet<Tb_MD_Empresa_PersonaAutorizada>();
        }

        [Key]
        [StringLength(20)]
        public string vNumDocumento { get; set; }

        [StringLength(150)]
        public string vRazonSocial { get; set; }

        [StringLength(150)]
        public string vDireccion { get; set; }

        [StringLength(10)]
        public string vUbigeoDireccion { get; set; }

        [StringLength(10)]
        public string vCodigoPostal { get; set; }

        [StringLength(10)]
        public string vTelefonoFijo { get; set; }

        [StringLength(10)]
        public string vTelefonoMovil { get; set; }

        [StringLength(50)]
        public string vRubro { get; set; }

        [StringLength(50)]
        public string vMailEmpresa { get; set; }

        [StringLength(50)]
        public string vNumDocumentoCreaEmpresa { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        public byte vEstadoRegsitro { get; set; }

        [StringLength(120)]
        public string vNombreEntidad { get; set; }

        public int? iCodDistrito { get; set; }

        public int? iCodDepartamento { get; set; }

        [StringLength(20)]
        public string vRepresentanteLegal { get; set; }

        public int? ActividadEconomica { get; set; }

        public int? OrigenFondos { get; set; }

        [StringLength(4)]
        public string vIdPaisOrigen { get; set; }

        public int? iCodProvincia { get; set; }

        public bool IsPartner { get; set; }

        public bool IsFideicomiso { get; set; }

        public bool IsLmd { get; set; }

        public virtual ICollection<Tb_MD_Accionistas> Tb_MD_Accionistas { get; set; }

        public virtual Tb_MD_ActividadEconomica Tb_MD_ActividadEconomica { get; set; }

        public virtual ICollection<Tb_MD_Cuentas_Bancarias> Tb_MD_Cuentas_Bancarias { get; set; }

        public virtual Tb_MD_Departamento Tb_MD_Departamento { get; set; }

        public virtual Tb_MD_Distrito Tb_MD_Distrito { get; set; }

        public virtual ICollection<Tb_MD_Empresa_PersonaAutorizada> Tb_MD_Empresa_PersonaAutorizada { get; set; }

        public virtual Tb_MD_OrigenFondo Tb_MD_OrigenFondo { get; set; }

        public virtual Tb_MD_Pais Tb_MD_Pais { get; set; }

        public virtual Tb_MD_Per_Natural Tb_MD_Per_Natural { get; set; }

        public virtual Tb_MD_Provincia Tb_MD_Provincia { get; set; }
    }
}
