using BusinessLogic;
using LightInject;

namespace Demo_Arquitectura.App_Start
{
    public class IoC
    {
        public static ServiceContainer container;

        public static void Initialize()
        {
            container = new ServiceContainer();

            container.Register<IAlumnoLogic, AlumnoLogic>();
            container.Register<IUsuarioLogic, UsuarioLogic>();
            container.Register<IPaisLogic, PaisLogic>();

            StartUp.Initialize(container);
        }
    }
}