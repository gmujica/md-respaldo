namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Pre_Clientes
    {
        [StringLength(20)]
        public string vNroDocumento { get; set; }

        [StringLength(200)]
        public string vEmail { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        public DateTime? dFechaValidacionPaso1 { get; set; }

        public DateTime? dFechaValidacionPaso2 { get; set; }

        public DateTime? dFechaValidacionPaso3 { get; set; }

        public DateTime? dFechaValidacionPaso4 { get; set; }

        public DateTime? dFechaValidacionPaso5 { get; set; }

        public Guid SecretId { get; set; }

        public int vEstadoRegistro { get; set; }

        public int vTipoCliente { get; set; }

        [StringLength(15)]
        public string vClaveSMS { get; set; }

        [StringLength(200)]
        public string vNombre { get; set; }

        [StringLength(200)]
        public string vApellido { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [Key]
        public int idPreCliente { get; set; }

        [StringLength(20)]
        public string vCelular { get; set; }

        public bool envioMSM { get; set; }

        [StringLength(5)]
        public string Seguimiento { get; set; }

        public bool Finalizado { get; set; }

        [StringLength(5)]
        public string vTipoDocumento { get; set; }

        public int? iCodDepartamento { get; set; }

        public int? iCodDistrito { get; set; }

        [StringLength(300)]
        public string NombreCliente { get; set; }

        public int? UsuarioValidacion_Fideicomiso { get; set; }

        public int? UsuarioValidacion_Operador { get; set; }

        public DateTime? FechaValidacion_Operador { get; set; }

        public DateTime? FechaValidacion_Fideicomiso { get; set; }

        [StringLength(1)]
        public string EstadoValidacion { get; set; }

        [StringLength(1)]
        public string EstadoValidacion_Fideicomiso { get; set; }

        [StringLength(300)]
        public string ComentarioOperador { get; set; }

        [StringLength(300)]
        public string ComentarioFideicomiso { get; set; }

        public int iEstadoNavegacion { get; set; }

        [StringLength(1)]
        public string vTipoRegistro { get; set; }

        public bool envioValidacion { get; set; }

        public DateTime? dFechaEnvioValidacion { get; set; }

        [StringLength(20)]
        public string nroDocumentoContacto { get; set; }

        [StringLength(250)]
        public string nombreEmpresa { get; set; }

        [StringLength(10)]
        public string vTipoValidacion { get; set; }
    }
}
