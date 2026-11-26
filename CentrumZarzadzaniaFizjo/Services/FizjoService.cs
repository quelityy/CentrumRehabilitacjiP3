using CentrumZarzadzaniaFizjo.Models;
using System.Text.Json;

namespace CentrumZarzadzaniaFizjo.Services
{
    public class FizjoService
    {
        private readonly string plikPacjentow = "pacjenci.json";
        private readonly string plikFizjo = "fizjoterapeuci.json";
        private readonly string plikWizyt = "wizyty.json";
        private List<Pacjent> pacjenci = new();
        private List<Fizjoterapeuta> fizjoterapeuci = new();
        private List<Wizyta> wizyty = new();

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

            // wizyty - tu nie dodajemy przykladowych, moze byc pusta lista
            if (File.Exists(plikWizyt))
            {
                string json = File.ReadAllText(plikWizyt);
                wizyty = JsonSerializer.Deserialize<List<Wizyta>>(json) ?? new();
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

        private void ZapiszWizyty()
        {
            var opcje = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(wizyty, opcje);
            File.WriteAllText(plikWizyt, json);
        }

        // ----- pacjenci -----

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

                    // usuwamy tez wizyty tego pacjenta
                    wizyty.RemoveAll(w => w.PacjentId == id);
                    ZapiszWizyty();
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

        // ----- fizjoterapeuci -----

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

                    // usuwamy tez jego wizyty
                    wizyty.RemoveAll(w => w.FizjoterapeutaId == id);
                    ZapiszWizyty();
                    return;
                }
            }
        }

        // ----- wizyty -----

        public List<Wizyta> PobierzWizyty()
        {
            // sortujemy po dacie, najblizsze na gorze
            return wizyty.OrderBy(w => w.DataWizyty).ToList();
        }

        // sprawdza czy fizjoterapeuta ma juz wizyte o tej godzinie
        public bool CzyTerminZajety(int fizjoId, DateTime data)
        {
            foreach (Wizyta w in wizyty)
            {
                if (w.FizjoterapeutaId == fizjoId && w.DataWizyty == data)
                {
                    return true;
                }
            }
            return false;
        }

        public void DodajWizyte(Wizyta nowaWizyta)
        {
            if (wizyty.Count > 0)
            {
                nowaWizyta.Id = wizyty.Max(w => w.Id) + 1;
            }
            else
            {
                nowaWizyta.Id = 1;
            }

            wizyty.Add(nowaWizyta);
            ZapiszWizyty();
        }

        public void UsunWizyte(int id)
        {
            foreach (Wizyta w in wizyty)
            {
                if (w.Id == id)
                {
                    wizyty.Remove(w);
                    ZapiszWizyty();
                    return;
                }
            }
        }

        // zeby w tabeli pokazac imie zamiast samego id
        public string PobierzNazwePacjenta(int id)
        {
            foreach (Pacjent p in pacjenci)
            {
                if (p.Id == id)
                {
                    return p.Imie + " " + p.Nazwisko;
                }
            }
            return "brak danych";
        }

        public string PobierzNazweFizjo(int id)
        {
            foreach (Fizjoterapeuta f in fizjoterapeuci)
            {
                if (f.Id == id)
                {
                    return f.Imie + " " + f.Nazwisko;
                }
            }
            return "brak danych";
        }
    }
}