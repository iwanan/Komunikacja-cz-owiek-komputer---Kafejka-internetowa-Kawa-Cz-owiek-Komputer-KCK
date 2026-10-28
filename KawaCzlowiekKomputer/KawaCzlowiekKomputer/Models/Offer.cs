namespace KawaCzlowiekKomputer.Models
{
    public class Offer<T>
    {
        public T Item { get; set; } = default!;
        public bool IsAllowed { get; set; }
    }
}