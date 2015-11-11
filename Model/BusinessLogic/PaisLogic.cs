using Model.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Repository;
using Common;

namespace Model.BusinessLogic
{
    public class PaisLogic
    {
        private ResponseModel rm;
        private Repository<Pais> _repoPais;

        public PaisLogic()
        {
            rm = new ResponseModel();
            _repoPais = new Repository<Pais>();
        }

        public List<Pais> Listar()
        {
            using (_repoPais.ContextScope(new DemoContext()))
            {
                return _repoPais.GetAll().OrderBy(x => x.Nombre).ToList();
            }
        }
    }
}
