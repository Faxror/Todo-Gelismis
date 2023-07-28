using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public enum durums
    {
        Yapılıyor = 3,
        Azkaldı = 4,
        Bitti = 5
    }

    public class Durum : BaseEntity
    {
        public string DurumName { get; set; }

        //...
    }
}
