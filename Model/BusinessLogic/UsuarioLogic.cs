using Model.Entities;
using Repository;
using Common;

namespace Model.BusinessLogic
{
    public class UsuarioLogic
    {
        private ResponseModel rm;
        private Repository<Usuario> repo;

        public UsuarioLogic()
        {
            rm = new ResponseModel();
            repo = new Repository<Usuario>();
        }

        public ResponseModel Autenticar(string correo, string contrasena)
        {
            using (repo.ContextScope(new DemoContext()))
            {
                contrasena = HashHelper.SHA1(contrasena);

                var usuario = repo.Get(x => x.Correo == correo && x.Contrasena == contrasena);

                if (usuario != null)
                {
                    SessionHelper.AddUserToSession(usuario.id.ToString());
                    rm.SetResponse(true);
                }
                else
                {
                    rm.SetResponse(false, "Acceso denegado al sistema");
                }

                return rm;
            }
        }
    }
}
