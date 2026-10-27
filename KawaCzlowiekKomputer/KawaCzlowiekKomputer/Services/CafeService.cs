using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class CafeService
    {
        private readonly IRepo<Rules> _rulesRepo;

        public CafeService(IRepo<Rules> rulesRepo) => _rulesRepo = rulesRepo;

        // punkty regulaminu w kolejności Id
        public List<Rules> GetRules() => _rulesRepo.GetAll().OrderBy(r => r.Id).ToList();
    }
}