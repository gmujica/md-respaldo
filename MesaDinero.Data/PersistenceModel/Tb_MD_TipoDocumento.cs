namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_TipoDocumento
    {
        [Key]
        [StringLength(5)]
        public string IdTipoDocumento { get; set; }

        [StringLength(150)]
        public string Nombre { get; set; }

        [StringLength(1)]
        public string Tipo { get; set; }

        public byte EstadoRegistro { get; set; }
    }
}
