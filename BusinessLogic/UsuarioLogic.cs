using Common;
using Model;
using Repository;
using Repository.Interfaces;

namespace BusinessLogic
{
    public interface IUsuarioLogic {
        ResponseModel Autenticar(string correo, string contrasena);
    }
    public class UsuarioLogic : IUsuarioLogic
    {
        private ResponseModel rm;
        private readonly IRepository<Usuario> repoUsuario;

        public UsuarioLogic(IRepository<Usuario> _repoUsuario)
        {
            rm = new ResponseModel();
            repoUsuario = _repoUsuario;
        }

        public ResponseModel Autenticar(string correo, string contrasena)
        {
            using (var ctx = new DbContextScope())
            {
                contrasena = HashHelper.SHA1(contrasena);

                var usuario = repoUsuario.Get(x => x.Correo == correo && x.Contrasena == contrasena);

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
