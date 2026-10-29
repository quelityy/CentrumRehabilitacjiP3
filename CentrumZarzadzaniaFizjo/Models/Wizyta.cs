using System.Data;

namespace CentrumZarzadzaniaFizjo.Models
{
    public class Wizyta
    {
        public int Id { get; set; }
        public DateTime DataWizyty { get; set; }
        public string OpisZabiegu { get; set; } = string.Empty;
        public int PacjentId {  get; set; }
        public Pacjent? Pacjent { get; set; }
        public int FizjoterapeutaId { get; set; }
        public Fizjoterapeuta? Fizjoterapeuta { get; set; }
    }
}
