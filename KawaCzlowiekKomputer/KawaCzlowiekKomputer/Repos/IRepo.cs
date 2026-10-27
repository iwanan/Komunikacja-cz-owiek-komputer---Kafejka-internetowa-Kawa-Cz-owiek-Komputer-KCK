using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Repos
{
    // Kontrakt repozytorium danych typu T (usun dodaj itp.) okreslajacy jakie operacje na zapisanych danych musi obslugiwac kazde repo z danym typem T
    public interface IRepo<T> where T : IHasId
    {
        List<T> GetAll();      // wszystkie rekordy
        T? GetById(int id);    // jeden rekord albo null
        void Add(T item);      // dodaje i nadaje Id
        void Update(T item);   // podmienia rekord o tym samym Id
        void Delete(int id);   // usuwa rekord
    }
}