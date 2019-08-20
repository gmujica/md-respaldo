namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Pagina
    {
        public Tb_MD_Pagina()
        {
            Tb_MD_PefilPagina = new HashSet<Tb_MD_PefilPagina>();
        }

        [Key]
        public int iIdPagina { get; set; }

        [StringLength(150)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Ruta { get; set; }

        public bool? EsMenu { get; set; }

        public int? ParentMenu { get; set; }

        public byte? Orden { get; set; }

        [StringLength(70)]
        public string Modulo { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        public virtual ICollection<Tb_MD_PefilPagina> Tb_MD_PefilPagina { get; set; }
    }
}
