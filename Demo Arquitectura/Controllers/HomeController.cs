using Model.BusinessLogic;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Demo_Arquitectura.Controllers
{
    public class HomeController : Controller
    {
        private AlumnoLogic alumnoLogic = new AlumnoLogic();

        public ActionResult Index()
        {
            var alumnos = new List<Alumno>();

            foreach (var a in alumnoLogic.Listar()) alumnos.Add(a);

            alumnos.Add(alumnoLogic.Buscar(3001));

            alumnos.AddRange(alumnoLogic.ListarConQueryPersonalizado());

            return View(alumnos);
        }

        public ActionResult Ver(int id)
        {
            return View(alumnoLogic.Get(id));
        }

        public ActionResult Crud()
        {
            return View();
        }

        public void Guardar()
        {
            alumnoLogic.Guardar(new Alumno {
                Nombre = "Carlos",
                Apellido = "Lozano Paredes",
                FechaNacimiento = "2001-02-03",
                Sexo = 1
            });
        }

        public void ActualizarNombre()
        {
            alumnoLogic.ActualizarNombre(new Alumno
            {
                id = 2004,
                Nombre = "Carlos",
                Apellido = "Rodríguez"
            });
        }
    }
}