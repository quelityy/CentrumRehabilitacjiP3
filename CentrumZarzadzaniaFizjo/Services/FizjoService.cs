using CentrumZarzadzaniaFizjo.Models;
using System.Text.Json;

namespace CentrumZarzadzaniaFizjo.Services
{
    public class FizjoService
    {
        private readonly string plikPacjentow = "pacjenci.json";
        private readonly string plikFizjo = "fizjoterapeuci.json";

        private List<Pacjent> pacjenci = new();
        private List<Fizjoterapeuta> fizjoterapeuci = new();

        public FizjoService()
        {
            WczytajZPliku();
        }

        private void WczytajZPliku()
        {
            // wczytujemy pacjentow z pliku jesli istnieje
            if (File.Exists(plikPacjentow))
            {
                string json = File.ReadAllText(plikPacjentow);
                pacjenci = JsonSerializer.Deserialize<List<Pacjent>>(json) ?? new();
            }

            // jak nikogo nie ma to dodajemy przykladowego
            if (pacjenci.Count == 0)
            {
                Pacjent p = new Pacjent();
                p.Id = 1;
                p.Imie = "Jan";
                p.Nazwisko = "Kowalski";
                p.Telefon = "500100200";
                p.Email = "jan.kowalski@example.com";
                pacjenci.Add(p);
                ZapiszPacjentow();
            }

            // to samo dla fizjoterapeutow
            if (File.Exists(plikFizjo))
            {
                string json = File.ReadAllText(plikFizjo);
                fizjoterapeuci = JsonSerializer.Deserialize<List<Fizjoterapeuta>>(json) ?? new();
            }

            if (fizjoterapeuci.Count == 0)
            {
                Fizjoterapeuta f = new Fizjoterapeuta();
                f.Id = 1;
                f.Imie = "Anna";
                f.Nazwisko = "Nowak";
                f.Specjalizacja = "Rehabilitacja ortopedyczna";
                f.Telefon = "600300400";
                fizjoterapeuci.Add(f);
                ZapiszFizjo();
            }
        }

        private void ZapiszPacjentow()
        {
            var opcje = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(pacjenci, opcje);
            File.WriteAllText(plikPacjentow, json);
        }

        private void ZapiszFizjo()
        {
            var opcje = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(fizjoterapeuci, opcje);
            File.WriteAllText(plikFizjo, json);
        }

        public List<Pacjent> PobierzPacjentow()
        {
            return pacjenci;
        }

        public void DodajPacjenta(Pacjent nowyPacjent)
        {
            // nowe id to najwieksze id + 1
            if (pacjenci.Count > 0)
            {
                nowyPacjent.Id = pacjenci.Max(p => p.Id) + 1;
            }
            else
            {
                nowyPacjent.Id = 1;
            }

            pacjenci.Add(nowyPacjent);
            ZapiszPacjentow();
        }

        public void UsunPacjenta(int id)
        {
            foreach (Pacjent p in pacjenci)
            {
                if (p.Id == id)
                {
                    pacjenci.Remove(p);
                    ZapiszPacjentow();
                    return; // konczymy bo juz usunelismy
                }
            }
        }

        public List<Pacjent> WyszukajPacjentow(string szukanaFraza)
        {
            // pusta fraza = wszyscy pacjenci
            if (string.IsNullOrWhiteSpace(szukanaFraza))
            {
                return pacjenci;
            }

            return pacjenci
                .Where(p => p.Imie.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase)
                         || p.Nazwisko.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Fizjoterapeuta> PobierzFizjoterapeutow()
        {
            return fizjoterapeuci;
        }

        public void DodajFizjoterapeute(Fizjoterapeuta nowy)
        {
            if (fizjoterapeuci.Count > 0)
            {
                nowy.Id = fizjoterapeuci.Max(f => f.Id) + 1;
            }
            else
            {
                nowy.Id = 1;
            }

            fizjoterapeuci.Add(nowy);
            ZapiszFizjo();
        }

        public void UsunFizjoterapeute(int id)
        {
            foreach (Fizjoterapeuta f in fizjoterapeuci)
            {
                if (f.Id == id)
                {
                    fizjoterapeuci.Remove(f);
                    ZapiszFizjo();
                    return;
                }
            }
        }
    }
}