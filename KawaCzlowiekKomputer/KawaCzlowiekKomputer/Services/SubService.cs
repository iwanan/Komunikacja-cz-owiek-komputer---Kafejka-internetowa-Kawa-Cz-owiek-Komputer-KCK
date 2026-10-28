using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class SubService
    {
        private readonly IRepo<Subscription> _subs;

        public SubService(IRepo<Subscription> subs) => _subs = subs;

        // karnety w kolejności Id
        public List<Subscription> GetSubscriptions() => _subs.GetAll().OrderBy(s => s.Id).ToList();
    }
}