using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaCzlowiekKomputer.Models
{
    public class User : IHasId
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Sol { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty;
}
}
