using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model
{
    public partial class PageAccess
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string RouteUrl { get; set; }
        public bool IsDefault { get; set; }
        public string Modulo { get; set; }
    }

    public partial class MenuAccess
    {
        public string Modulo { get; set; }
        public int MenuId { get; set; }
        public string Nombre { get; set; }
        public string Icon { get; set; }
        public string _routeUrl
        {
            get
            {

                string result = string.Empty;

                if (!string.IsNullOrEmpty(RouteUrl))
                {
                    result = RouteUrl.Substring(1, RouteUrl.Length - 1);
                }

                return result;
            }
        }
        public string RouteUrl { get; set; }
        public byte? Orden { get; set; }
        public int? ParentMenuId { get; set; }
    }
}
