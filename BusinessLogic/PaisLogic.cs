using System.Collections.Generic;
using System.Linq;
using Repository;
using Common;
using Model;
using Repository.Interfaces;

namespace BusinessLogic
{
    public interface IPaisLogic
    {
        List<Pais> Listar();
    }
    public class PaisLogic : IPaisLogic
    {
        private ResponseModel rm;
        private readonly IRepository<Pais> repoPais;

        public PaisLogic(IRepository<Pais> _repoPais)
        {
            rm = new ResponseModel();
            repoPais = _repoPais;
        }

        public List<Pais> Listar()
        {
            using (var ctx = new DbContextScope())
            {
                return repoPais.GetAll()
                               .OrderBy(x => x.Nombre)
                               .ToList();
            }
        }
    }
}
