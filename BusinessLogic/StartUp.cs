using LightInject;
using Model;
using Repository;
using Repository.Interfaces;

namespace BusinessLogic
{
    public class StartUp
    {
        public static void Initialize(ServiceContainer container)
        {
            container.Register<IRepository<Alumno>, Repository<Alumno>>();
            container.Register<IRepository<Usuario>, Repository<Usuario>>();
            container.Register<IRepository<Pais>, Repository<Pais>>();
        }
    }
}