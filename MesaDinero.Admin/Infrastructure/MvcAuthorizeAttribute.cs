using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using MesaDinero.Domain.Model;

namespace MesaDinero.Admin.Infrastructure
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        private const string URL_HOME = "~/";
        private string _controllerName;
        private string _actionName;
        private string _urlRequest;
        private string _urlName;
        private bool skipLogin = false;
        private bool skipAuthorization = false;
        private IEnumerable<PageAccess> _pages;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (skipAuthorization)
                return true;

            var user = httpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                LoadAccessPages(user);
                bool existe = false;
                PageAccess page_= null;
                if (_urlRequest.ToLower().StartsWith(_urlName.ToLower()))
                {
                    page_ = _pages.FirstOrDefault(x => x.RouteUrl.ToLower().Contains(_urlName.ToLower()));
                    existe = page_ != null;
                }
                else
                {
                    page_ = _pages.FirstOrDefault(x => x.RouteUrl.ToLower().Contains(_urlRequest.ToLower()));
                    existe = page_ != null;
                }
                httpContext.Session["menuid"] = string.Format("m_{0}", page_ == null ? 0 : page_.id);
                
                return existe;
                
                // Registrar ingreso

            }


            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            _controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            _actionName = filterContext.ActionDescriptor.ActionName;
            _urlRequest =  filterContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath;
            _urlName = string.Format("~/{0}/{1}",_controllerName,_actionName);

            //skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                skipAuthorization =
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            }
            PageAccess page_ = null;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                

                if (_urlRequest.ToLower() == URL_HOME.ToLower())
                {
                    LoadAccessPages(filterContext.HttpContext.User);
                     page_ = _pages.FirstOrDefault(x => x.IsDefault);
                    var pageDefault = _pages == null ? null : page_.RouteUrl;
                    if (pageDefault != null && pageDefault.ToLower() != _urlName.ToLower())
                    {
                        _urlRequest =  pageDefault;
                        filterContext.Result = new RedirectResult(pageDefault);
                    }
                }

                var menus = GetAccessMenu(filterContext.HttpContext.User);

                filterContext.Controller.ViewBag.MenuData = menus;
               
            }
            


            //filterContext.Controller.ViewBag.FileVersion = ConfigurationManager.AppSettings["FileVersion"];

             base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext"); //throw new ArgumentNullException(nameof(filterContext));

            filterContext.Result = filterContext.HttpContext.Request.IsAuthenticated ? new HttpStatusCodeResult(HttpStatusCode.Forbidden) : new HttpUnauthorizedResult();
        }

        #region Method's

        private void LoadAccessPages(IPrincipal user)
        {
            //var claimsIdentity = user.Identity as ClaimsIdentity;
            //var windowsAccountName = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.WindowsAccountName);
            //var security = DependencyResolver.Current.GetService<ISecurityRepository>();
            var userName = user.GetClaimValueByType(ClaimTypes.PostalCode);
            int codigo = 0;
            int.TryParse(userName, out codigo);
            MesaDinero.Domain.DataAccess.AccesoDataAccess accesoDataAccess = new Domain.DataAccess.AccesoDataAccess();
            _pages = accesoDataAccess.GetAccessPages(codigo);
            //_pages = security.GetAccessPages(userName);
        }

        private IEnumerable<MesaDinero.Domain.Model.MenuAccess> GetAccessMenu(IPrincipal user)
        {
            //var claimsIdentity = user.Identity as ClaimsIdentity;
            //var windowsAccountName = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.WindowsAccountName);
            //var security = DependencyResolver.Current.GetService<ISecurityRepository>();
            var userName = user.GetClaimValueByType(ClaimTypes.PostalCode);
            int codigo = 0;
            int.TryParse(userName, out codigo);
            MesaDinero.Domain.DataAccess.AccesoDataAccess accesoDataAccess = new Domain.DataAccess.AccesoDataAccess();
            var menus = accesoDataAccess.GetAccessMenu(codigo);


            //var menus = security.GetAccessMenu(userName);
            return menus;
        }

        #endregion


    }
}