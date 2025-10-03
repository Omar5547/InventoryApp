using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public  enum OrderStatus
    {
        Open = 0,
        Posted = 1,
        Canceled = 2
    }
    public enum UnitType
    {
        Piece =0,
        Carton=1,
        Liter=2,
        Bottle=3,
        Kg=4,
        Meter= 5

    }
}
