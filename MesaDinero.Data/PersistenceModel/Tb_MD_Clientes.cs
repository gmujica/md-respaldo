namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Clientes
    {
        public Tb_MD_Clientes()
        {
            Tb_MD_ClienteUsuario = new HashSet<Tb_MD_ClienteUsuario>();
        }

        [Key]
        public int iIdCliente { get; set; }

        [StringLength(200)]
        public string vNombre { get; set; }

        [StringLength(200)]
        public string vApellido { get; set; }

        [StringLength(200)]
        public string vEmail { get; set; }

        [StringLength(10)]
        public string vCelular { get; set; }

        public int? vTipoCliente { get; set; }

        public byte? iEstadoRegistro { get; set; }

        [StringLength(5)]
        public string vTipoDocumento { get; set; }

        [StringLength(20)]
        public string vNroDocumento { get; set; }

        public int? iCodDepartamento { get; set; }

        public int? iCodDistrito { get; set; }

        public Guid? SecretId { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(300)]
        public string NombreCliente { get; set; }

        public int? codigoModificacionDatos { get; set; }

        public int? codigoModificacionCuentasBancarias { get; set; }

        public int? idPreCliente { get; set; }

        public bool? bFlagActivo { get; set; }

        public virtual ICollection<Tb_MD_ClienteUsuario> Tb_MD_ClienteUsuario { get; set; }
    }
}
