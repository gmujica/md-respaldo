namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Tiempos
    {
        [Key]
        [StringLength(10)]
        public string vCodTransaccion { get; set; }

        public int? nTiempoStandar { get; set; }

        public int? nTiempoPremiun { get; set; }

        public int? nTiempoVip { get; set; }

        public DateTime? dFechaCreacion { get; set; }

        [StringLength(20)]
        public string vUsuarioCreacion { get; set; }

        public DateTime? dFechaModificacion { get; set; }

        [StringLength(20)]
        public string vUsuarioModificacion { get; set; }

        [StringLength(1)]
        public string nTiempoTransFideicomiso { get; set; }

        [StringLength(20)]
        public string vNroDocumento { get; set; }

        [StringLength(15)]
        public string vRUCEntidad { get; set; }

        public byte iEstadoRegistro { get; set; }
    }
}
