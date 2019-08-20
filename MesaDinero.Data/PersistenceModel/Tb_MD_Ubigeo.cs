namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tb_MD_Ubigeo
    {
        [Key]
        public int IdUbigeo { get; set; }

        [Required]
        [StringLength(4)]
        public string CodPais { get; set; }

        public int CodDepartamento { get; set; }

        public int CodProvincia { get; set; }

        public int CodDistrito { get; set; }

        [StringLength(10)]
        public string CodUbigeo { get; set; }

        public byte iEstadoRegistro { get; set; }

        public virtual Tb_MD_Departamento Tb_MD_Departamento { get; set; }

        public virtual Tb_MD_Distrito Tb_MD_Distrito { get; set; }

        public virtual Tb_MD_Pais Tb_MD_Pais { get; set; }

        public virtual Tb_MD_Provincia Tb_MD_Provincia { get; set; }
    }
}
