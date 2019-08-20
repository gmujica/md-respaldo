namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Documentos
    {
        public Tb_MD_Documentos()
        {
            Tb_MD_DocOrdenPagoSubasta = new HashSet<Tb_MD_DocOrdenPagoSubasta>();
        }

        [Key]
        public int nIdDocumento { get; set; }

        [Required]
        [StringLength(120)]
        public string vNombre { get; set; }

        [Required]
        [StringLength(10)]
        public string vExtension { get; set; }

        [Required]
        [StringLength(5)]
        public string vTipo { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] vArchivo { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual ICollection<Tb_MD_DocOrdenPagoSubasta> Tb_MD_DocOrdenPagoSubasta { get; set; }
    }
}
