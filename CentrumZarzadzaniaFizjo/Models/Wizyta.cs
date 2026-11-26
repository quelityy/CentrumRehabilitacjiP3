namespace CentrumZarzadzaniaFizjo.Models
{
    public class Wizyta
    {
        public int Id { get; set; }
        public DateTime DataWizyty { get; set; }
        public string OpisZabiegu { get; set; } = string.Empty;

        // zamiast calego pacjenta trzymamy tylko jego id
        public int PacjentId { get; set; }
        public int FizjoterapeutaId { get; set; }
    }
}