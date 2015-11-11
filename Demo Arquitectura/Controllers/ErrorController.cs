using System.Web.Mvc;

namespace Demo_Arquitectura.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Esto es muy vergonzoso, esperemos que no vuelva a pasar ..";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe";
                    break;

                default:
                    ViewBag.Title = "Ocurrio un error en el servidor";
                    ViewBag.Description = "Algo salio muy mal :( ..";
                    break;
            }

            return View("~/views/error/_ErrorPage.cshtml");
        }
    }
}