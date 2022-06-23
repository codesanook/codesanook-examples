namespace Codesanook.Examples.Playwright
{
    public class Product
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ImageUrl { get; set; }
        public string DetailsUrl { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return string.Join("\n", new[] {
                $"{nameof(Name)}: {Name}",
                $"{nameof(ShortDescription)}: {ShortDescription}",
                $"{nameof(DetailsUrl)}: {DetailsUrl}",
                $"{nameof(ImageUrl)}: {ImageUrl}",
                $"{nameof(Price)}: {Price}",
            });
        }
    }
}
