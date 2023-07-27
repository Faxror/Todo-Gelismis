using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public enum Category
    {
        Günlük = 1,
        Haftalık = 2,
        Aylık = 3
    }

    public class Categorys : BaseEntity
    {
        public string CategoryName { get; set; }

        //...
    }
}
