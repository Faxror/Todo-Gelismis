using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataAccess.Abstract
{
    public interface IRepository
    {
       List<Test>  gettodoall();

       Test getotodoid(int id);
       void createtodo(Test test);
       void updatetodo(Test test);

        void listtodotrue(int id);
        void listtodofalse(int id);

        void deletetodo(int id);
    }
}
