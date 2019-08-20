namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_RegistroPersonaAutorizada
    {
        [Key]
        [StringLength(20)]
        public string vRegPAutorizada { get; set; }
    }
}
