using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RepositoryManager : IRepositoryService
    {
        private readonly IRepository _repository;
        public void createtodo(Test test)
        {
             _repository.createtodo(test);
        }

        public void deletetodo(int id)
        {
             _repository.deletetodo(id);
        }

        public Test getotodoid(int id)
        {
           return  _repository.getotodoid(id);
        }

        public List<Test> gettodoall()
        {
            return _repository.gettodoall();
        }

        public void updatetodo(Test test)
        {
            _repository.updatetodo(test);
        }
    }
}
