using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain
{
    public partial class ComboCuentasBancarias
    {
        public long id { get; set; }
        public string nro { get; set; }
        public string logo { get; set; }
    }

    public partial class ComboListItemString
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public partial class ComboListItem 
    {
        public int value { get; set; }
        public string text { get; set; }
    }



}
