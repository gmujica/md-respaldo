namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_PefilPagina
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPerfil { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPagina { get; set; }

        public bool EsPorDefecto { get; set; }

        public virtual Tb_MD_Pagina Tb_MD_Pagina { get; set; }

        public virtual Tb_MD_Perfiles Tb_MD_Perfiles { get; set; }
    }
}
