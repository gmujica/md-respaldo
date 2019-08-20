using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess
{
    public class AccesoDataAccess
    {
        public IEnumerable<PageAccess> GetAccessPages(int currentUser)
        {
            using (MesaDineroContext context = new MesaDineroContext())
            {
                var usuario = context.Tb_MD_Mae_Usuarios.First(x => x.iIdUsuario == currentUser);
                IEnumerable<Tb_MD_Perfiles> perfiles ;
                perfiles = usuario.Tb_MD_PerfilUsuario.Select(x => x.Tb_MD_Perfiles).ToList();
                var result = perfiles.SelectMany(x => x.Tb_MD_PefilPagina.Where(y => y.Tb_MD_Pagina.Ruta != null).Select(y => new PageAccess
                {
                    id = y.IdPagina,
                    IsDefault = y.EsPorDefecto,
                    Nombre = y.Tb_MD_Pagina.Nombre,
                    RouteUrl = y.Tb_MD_Pagina.Ruta,
                    Modulo = ""//y.Pagina1.Modulo
                }));
                return result.OrderBy(x => x.Nombre).ToList();
            }

        }

        public IEnumerable<MenuAccess> GetAccessMenu(int currentUser)
        {
             using (MesaDineroContext context = new MesaDineroContext())
             {
                 string raizRuta = ConfigurationManager.AppSettings["RutaMenu"];
                 var usuario = context.Tb_MD_Mae_Usuarios.First(x => x.iIdUsuario == currentUser);
                 IEnumerable<Tb_MD_Perfiles> perfiles;
                 perfiles = usuario.Tb_MD_PerfilUsuario.Select(x => x.Tb_MD_Perfiles).ToList();
                 var result = perfiles.SelectMany(x => x.Tb_MD_PefilPagina.Where(y => y.Tb_MD_Pagina.EsMenu == true).OrderBy(y => y.Tb_MD_Pagina.Orden).Select(y => new MenuAccess
                 {
                     Nombre = y.Tb_MD_Pagina.Nombre,
                     RouteUrl = y.Tb_MD_Pagina.Ruta,
                     Orden = y.Tb_MD_Pagina.Orden,
                     MenuId = y.Tb_MD_Pagina.iIdPagina,
                     ParentMenuId = y.Tb_MD_Pagina.ParentMenu,
                     Modulo = y.Tb_MD_Pagina.Modulo ?? "",
                     Icon = y.Tb_MD_Pagina.Icon ?? ""
                 }));

                 List<MenuAccess> menues = new List<MenuAccess>();
                 result.ToList().ForEach(x =>
                 {
                     if (menues.Where(y => y.Nombre == x.Nombre).ToList().Count() == 0) {
                         MenuAccess men = new MenuAccess();
                         string ruta = "";
                         men.Nombre = x.Nombre;
                         if (x.RouteUrl == null)
                         {
                             ruta = "";
                         }
                         else {
                             if (x.RouteUrl.Length>0)
                             {
                                 ruta = raizRuta + x.RouteUrl.Substring(1, x.RouteUrl.Length - 1).Trim();
                             }
                          
                         }
                         men.RouteUrl = ruta;
                         men.Orden = x.Orden;
                         men.MenuId = x.MenuId;
                         men.ParentMenuId = x.ParentMenuId;
                         men.Modulo = x.Modulo ?? "";
                         men.Icon = x.Icon ?? "";
                         menues.Add(men);
                     }
                    

                 });
                 //var lista2 = menues.ToList();
                 //var listas = result.Select(x => x.Nombre).Distinct();

                 //return result.ToList();
                 return menues.ToList();
             }
            

           
        }


    }
}
