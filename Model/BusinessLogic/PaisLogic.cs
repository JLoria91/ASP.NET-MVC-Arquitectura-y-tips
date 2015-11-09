using Model.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Repository;

namespace Model.BusinessLogic
{
    public class PaisLogic : Repository<Pais>
    {
        private ResponseModel rm;

        public PaisLogic()
        {
            rm = new ResponseModel();
        }

        public List<Pais> Listar()
        {
            using (var db = this.ContextScope(new DemoContext()))
            {
                return this.GetAll().ToList();
            }
        }
    }
}
