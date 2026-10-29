namespace CentrumZarzadzaniaFizjo.Models
{
    public class Fizjoterapeuta
    {
        public int Id { get; set; }
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string Specjalizacja { get; set; } = string.Empty; 
        public string Telefon { get; set; } = string.Empty;
    }
}