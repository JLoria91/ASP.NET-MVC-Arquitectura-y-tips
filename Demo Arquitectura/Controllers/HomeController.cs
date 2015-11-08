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
            var alumnos = alumnoLogic.Listar();
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

        public void InsercionMasiva()
        {
            var alumnos = new List<Alumno>();

            for (var i = 0; i < 10; i++)
            {
                alumnos.Add(new Alumno() {
                    Nombre = "Nombre " + i,
                    Apellido = "Apellido " + i,
                    Sexo = 1,
                    FechaNacimiento = "1989-01-01"
                });
            }

            alumnoLogic.InsertarVarios(alumnos);
        }
    }
}