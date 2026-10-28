using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class CafeService
    {
        private readonly IRepo<Rules> _rulesRepo;
        private readonly IRepo<PriceListing> _pricesRepo;
        private readonly IRepo<Game> _gamesRepo;

        public CafeService(IRepo<Rules> rulesRepo, IRepo<PriceListing> pricesRepo, IRepo<Game> gamesRepo)
        {
            _rulesRepo = rulesRepo;
            _pricesRepo = pricesRepo;
            _gamesRepo = gamesRepo;
        }

        // punkty regulaminu w kolejności Id
        public List<Rules> GetRules() => _rulesRepo.GetAll().OrderBy(r => r.Id).ToList();

        // cennik w kolejności Id
        public List<PriceListing> GetPrices() => _pricesRepo.GetAll().OrderBy(p => p.Id).ToList();

        // gry posortowane alfabetycznie
        public List<Game> GetGames() => _gamesRepo.GetAll().OrderBy(g => g.Title).ToList();
    }
}