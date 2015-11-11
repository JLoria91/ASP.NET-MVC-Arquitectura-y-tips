using Common;
using Demo_Arquitectura.Filters;
using Demo_Arquitectura.ViewsModel;
using Model.BusinessLogic;
using System;
using System.Web.Mvc;

namespace Demo_Arquitectura.Controllers
{
    public class AccountController : Controller
    {
        private UsuarioLogic usuarioLogic = new UsuarioLogic();

        [EstaAutenticadoAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Autenticar(LoginViewModel model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                rm = usuarioLogic.Autenticar(model.Correo, model.Contrasena);

                if (rm.response)
                {
                    rm.href = Url.Content("~/home");
                }
            }
            else
            {
                rm.SetResponse(false, "Debe llenar los campos para poder autenticarse.");
            }

            return Json(rm);
        }

        public ActionResult Desconectarse()
        {
            SessionHelper.DestroyUserSession();
            return Redirect("~/account");
        }
    }
}