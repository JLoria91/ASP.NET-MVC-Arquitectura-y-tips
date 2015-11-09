using Model.BusinessLogic;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        private static AlumnoLogic alumnoLogic = new AlumnoLogic();

        static void Main(string[] args)
        {
            try
            {
                ActualizarNombre();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void ActualizarNombre()
        {
            alumnoLogic.ActualizarNombre(new Alumno
            {
                id = 3033,
                Nombre = "Pamela",
                Apellido = "Rodríguez"
            });
        }

        // Esta prueba incluye un bloque de transacción
        static void InsertarVarios()
        {
            var alumnos = new List<Alumno>();

            for (var i = 0; i < 10; i++)
            {
                alumnos.Add(new Alumno()
                {
                    Nombre = null,
                    Apellido = "Apellido " + i,
                    Sexo = 1,
                    FechaNacimiento = "1989-01-01"
                });
            }

            alumnoLogic.InsertarVarios(alumnos);
        }
    }
}
