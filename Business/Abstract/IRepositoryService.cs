using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Entity;

namespace Business.Abstract
{
    public interface IRepositoryService
    {
        List<Test> gettodoall();

        Test getotodoid(int id);
        void createtodo(Test test);
        void updatetodo(Test test);

        void deletetodo(int id);
    }
}
