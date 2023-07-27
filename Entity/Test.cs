using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Test : BaseEntity
    {
        public string Title { get; set; }

        public string Contents { get; set; }

        public string AssignedPerson { get; set; }
        public string Size { get; set; }

        public string Montly { get; set; }

        public bool RowStatus { get; set; }

       
    }
}
