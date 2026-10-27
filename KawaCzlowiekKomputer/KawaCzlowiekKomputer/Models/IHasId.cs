using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaCzlowiekKomputer.Models
{
    //pokazuje, że obiekt ma Id, które jest intem i można je ustawiać i pobierać
    public interface IHasId
    {
        int Id { get; set; }
    }
}

