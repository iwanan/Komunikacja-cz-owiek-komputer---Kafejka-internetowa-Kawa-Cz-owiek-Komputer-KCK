using System.Text.Encodings.Web;
using System.Text.Json;
using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Repos
{
    // Repozytorium zapisujące dane w pliku JSON (folder Data)
    public class JsonRepo<T> : IRepo<T> where T : IHasId
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,                                  // czytelny plik
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping  // polskie znaki bez \u
        };

        private readonly string _path;
        private readonly List<T> _items;

        // Nazwa pliku, np. "Stations.json"
        public JsonRepo(string fileName)
        {
            _path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            _items = Load();
        }

        public List<T> GetAll() => new(_items); // kopia, żeby nikt nie zmienił listy z zewnątrz

        public T? GetById(int id) => _items.FirstOrDefault(i => i.Id == id);

        public void Add(T item)
        {
            item.Id = _items.Count == 0 ? 1 : _items.Max(i => i.Id) + 1;
            _items.Add(item);
            Save();
        }

        public void Update(T item)
        {
            int index = _items.FindIndex(i => i.Id == item.Id);
            if (index < 0) return;

            _items[index] = item;
            Save();
        }

        public void Delete(int id)
        {
            if (_items.RemoveAll(i => i.Id == id) > 0)
                Save();
        }

        private List<T> Load()
        {
            if (!File.Exists(_path)) return new List<T>();

            try
            {
                string json = File.ReadAllText(_path);
                return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
            }
            catch (JsonException) { return new List<T>(); } // uszkodzony plik
            catch (IOException) { return new List<T>(); }
        }

        private void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path, JsonSerializer.Serialize(_items, Options));
        }
    }
}