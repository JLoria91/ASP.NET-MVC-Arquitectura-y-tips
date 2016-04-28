using BusinessLogic;
using Common;
using Demo_Arquitectura.App_Start;
using Demo_Arquitectura.Filters;
using Demo_Arquitectura.ViewsModel;
using System.Web.Mvc;

namespace Demo_Arquitectura.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioLogic usuarioLogic;

        //Esta forma no me funciona, debería ser la más optima
        //public AccountController(IUsuarioLogic _usuarioLogic)
        //{
        //    usuarioLogic = _usuarioLogic;
        //}

        public AccountController()
        {
            usuarioLogic = IoC.container.GetInstance<IUsuarioLogic>();
        }

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