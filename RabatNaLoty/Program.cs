using System;

namespace WarsztatRabatNaLoty
{
    internal class Program
    {
        static void Main()
        {
            Console.Write("Podaj swoją datę urodzenia w formacie RRRR-MM-DD: ");
            DateTime dataUrodzenia;
            while (!DateTime.TryParse(Console.ReadLine(), out dataUrodzenia))
            {
                Console.Write("Błędny format daty. Podaj datę w formacie RRRR-MM-DD: ");
            }

            DateTime dataLotu = WczytajDateLotu();

            bool czyKrajowy = WczytajOdpowiedz("Czy lot jest krajowy (T/N)? ");

            bool czyStalyKlient = WczytajOdpowiedz("Czy jesteś stałym klientem (T/N)? ");

            int rabat = ObliczRabat(dataUrodzenia, dataLotu, czyKrajowy, czyStalyKlient);


            Console.WriteLine("\n=== Do obliczeń przyjęto:");
            Console.WriteLine($" * Data urodzenia: {dataUrodzenia:dd.MM.yyyy}");
            Console.WriteLine($" * Data lotu: {dataLotu:dddd, d MMMM yyyy}. {(CzyPozaSezonem(dataLotu) ? "Lot poza sezonem" : "Lot w sezonie")}");
            Console.WriteLine($" * Lot {(czyKrajowy ? "krajowy" : "międzynarodowy")}");
            Console.WriteLine($" * Stały klient: {(czyStalyKlient ? "Tak" : "Nie")}");
            Console.WriteLine($"\nPrzysługuje Ci rabat w wysokości: {rabat}%");
            Console.WriteLine($"Data wygenerowania raportu: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }

        public static DateTime WczytajDateLotu()
        {
            DateTime dataLotu;

            do
            {
                Console.Write("Podaj datę lotu w formacie RRRR-MM-DD: ");
                while (!DateTime.TryParse(Console.ReadLine(), out dataLotu))
                {
                    Console.Write("Błędny format daty. Podaj datę w formacie RRRR-MM-DD: ");
                }

                if (dataLotu < DateTime.Today)
                {
                    Console.WriteLine("Data lotu nie może być w przeszłości. Podaj poprawną datę.");
                }
            } while (dataLotu < DateTime.Today);

            return dataLotu;
        }

        static bool WczytajOdpowiedz(string pytanie)
        {
            while (true)
            {
                Console.Write(pytanie);
                string odpowiedz = Console.ReadLine().Trim().ToLower();

                if (odpowiedz == "t")
                {
                    return true;
                }
                else if (odpowiedz == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Proszę podać poprawną odpowiedź (T/N). ");
                }
            }
        }
        static int ObliczRabat(DateTime dataUrodzenia, DateTime dataLotu, bool czyKrajowy, bool czyStalyKlient)
        {
            int wiek = dataLotu.Year - dataUrodzenia.Year;

            if (dataUrodzenia > dataLotu.AddYears(-wiek)) wiek--;

            bool pozaSezonem = CzyPozaSezonem(dataLotu);
            int rabat = 0;

            if (wiek < 2) rabat = czyKrajowy ? 80 : 70;
            else if (wiek >= 2 && wiek <= 16) rabat = 10;

            if ((DateTime.Now < dataLotu.AddMonths(-5))) rabat += 10;

            if (!czyKrajowy && pozaSezonem) rabat += 15;

            if (wiek >= 18 && czyStalyKlient) rabat += 15;

            if (wiek < 2 && rabat > 80) rabat = 80;
            else if (wiek > 2 && rabat > 30) rabat = 30;

            return rabat;
        }

        static bool CzyPozaSezonem(DateTime dataLotu)
        {
            int rok = dataLotu.Year;
            DateTime poczatekSezonu1 = new DateTime(rok, 12, 20);
            DateTime koniecSezonu1 = new DateTime(rok + 1, 1, 10);
            DateTime poczatekSezonu2 = new DateTime(rok, 3, 20);
            DateTime koniecSezonu2 = new DateTime(rok, 4, 10);
            DateTime lipiec = new DateTime(rok, 7, 1);
            DateTime sierpien = new DateTime(rok, 8, 1);

            bool pozaSezonem = !((dataLotu >= poczatekSezonu1 && dataLotu <= koniecSezonu1) ||
                                  (dataLotu >= poczatekSezonu2 && dataLotu <= koniecSezonu2) ||
                                  (dataLotu.Month == lipiec.Month || dataLotu.Month == sierpien.Month));

            return pozaSezonem;
        }
    }
}