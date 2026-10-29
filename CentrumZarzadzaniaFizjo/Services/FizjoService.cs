using CentrumZarzadzaniaFizjo.Models;
using System.Text.Json;

namespace CentrumZarzadzaniaFizjo.Services
{
    public class FizjoService
    {
        private readonly string plikPacjentow = "pacjenci.json";
        private List<Pacjent> pacjenci = new();

        public FizjoService()
        {
            WczytajZPliku();
        }

        private void WczytajZPliku()
        {
            if (File.Exists(plikPacjentow))
            {
                var json = File.ReadAllText(plikPacjentow);
                pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(json) ?? new();
            }
            if (pacjenci == null || pacjenci.Count == 0)
            {
                pacjenci = new()
        {
            new Pacjent
            {
                Id = 1,
                Imie = "Konrad",
                Nazwisko = "Plonka",
                Telefon = "515151551",
                Email = "eeeee@wp.pl"
            }
        };

                ZapiszDoPliku();
            }
        }

        private void ZapiszDoPliku()
        {
            var opcje = new JsonSerializerOptions {WriteIndented = true};
            var json = JsonSerializer.Serialize(pacjenci, opcje);
            File.WriteAllText(plikPacjentow, json);
        }
        public List<Pacjent> PobierzPacjentow() => pacjenci;
        public void DodajPacjenta(Pacjent nowyPacjent)
        {
            nowyPacjent.Id = pacjenci.Count > 0
            ? pacjenci.Max(p => p.Id) + 1
                : 1;
            pacjenci.Add(nowyPacjent);
            ZapiszDoPliku();
        }

        public List<Pacjent> WyszukajPacjentow(string szukanaFraza)
        {
            if (string.IsNullOrWhiteSpace(szukanaFraza))
            {
                return pacjenci;
            }
            return pacjenci
                .Where(p => p.Imie.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase) || p.Nazwisko.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}