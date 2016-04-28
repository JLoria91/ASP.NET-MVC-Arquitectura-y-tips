using BusinessLogic;
using Common;
using Demo_Arquitectura.Filters;
using System.Web.Mvc;
using Model;
using Demo_Arquitectura.App_Start;

namespace Demo_Arquitectura.Controllers
{
    [AutenticadoAttribute]
    public class HomeController : Controller
    {
        private readonly IAlumnoLogic alumnoLogic;
        private readonly IPaisLogic paisLogic;

        public HomeController()
        {
            alumnoLogic = IoC.container.GetInstance<IAlumnoLogic>();
            paisLogic = IoC.container.GetInstance<IPaisLogic>();
        }

        public ActionResult Index()
        {
            var alumnos = alumnoLogic.Listar();
            return View(alumnos);
        }

        public ActionResult Ver(int id)
        {
            var model = alumnoLogic.Obtener(id);
            return View(model);
        }

        public ActionResult Crud(int id = 0)
        {
            ViewBag.Paises = paisLogic.Listar();
            return View(
                id == 0 ? new Alumno()
                        : alumnoLogic.Obtener(id)
            );
        }

        public JsonResult Guardar(Alumno model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                rm = alumnoLogic.Guardar(model);

                if (rm.response)
                {
                    rm.href = Url.Content("~/home");
                }
            }

            return Json(rm);
        }

        public ActionResult Eliminar(int id)
        {
            alumnoLogic.Eliminar(id);
            return Redirect("~/home");
        }
    }
}