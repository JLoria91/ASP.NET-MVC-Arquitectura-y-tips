using Common;
using Demo_Arquitectura.Filters;
using Model.BusinessLogic;
using Model.Entities;
using System.Web.Mvc;

namespace Demo_Arquitectura.Controllers
{
    [AutenticadoAttribute]
    public class HomeController : Controller
    {
        private AlumnoLogic alumnoLogic = new AlumnoLogic();
        private PaisLogic paisLogic = new PaisLogic();

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