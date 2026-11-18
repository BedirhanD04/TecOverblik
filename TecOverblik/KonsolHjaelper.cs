using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // static: durumu olmayan yardımcı sınıf, nesne oluşturmadan KonsolHjaelper.VisFejl(...) diye çağrılı
    internal static class KonsolHjaelper
    {
        public static void VisFejl(string fejlbesked)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(fejlbesked);
            Console.ResetColor();
        }

        // min ile max arasında bir sayı okur, geçersizse hata gösterip tekrar sorar
        public static int LæsTalIMellem(string prompt, int min, int max)
        {
            while (true)  // geçerli giriş gelene kadar döner
            {
                
                Console.Write(prompt);  // soruyu yaz
                    string? input = Console.ReadLine(); // girişi oku
                    // TryParse: metni sayıya çevirir (tip dönüşümü), olmazsa false döner, program çökmez
                    if (int.TryParse(input, out int valg) && valg >= min && valg <= max)
                    return valg; // geçerliyse sayıyı döndür, döngü biter

                    // geçersizse ortak hata metodunu çağır
                    VisFejl($"Ugyldigt input. Indtast et heltal mellem {min} og {max}.");
            }
        }

        //Convert.ToInt32 ya da int.Parse kullansaydık, kullanıcı abc yazınca program hata fırlatıp kapanırdı.
        //TryParse bunu önlüyor. Ayrıca tip dönüşümü (type conversion) Reflektion listesinde açıkça geçiyor, bu satır tam bir örnek.

        public static void SkrivElev(Elev elev, string indrykning)
        {
            if (elev.ErUnder18)                           
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(indrykning + "Elev: " + elev.Fornavn + ", " + elev.Efternavn);
            Console.ResetColor();
        }

        // Boş olmayan bir metin okur, boşsa kırmızı hata gösterip tekrar sorar

        public static string LaesTekst(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input)) return input.Trim();// NOT NULL

                VisFejl("Må ikke være tomt.");

            }
        }

        public static DateOnly LæseDato(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                // TryParseExact: accepterer kun formatet dd-MM-yyyy (typekonvertering), programmet går ikke ned ved fejl
                // Anden betingelse: En fremtidig dato kan ikke være en fødselsdato
                if (DateOnly.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", out DateOnly dato)
                    && dato <= DateOnly.FromDateTime(DateTime.Today))
                    return dato;

                VisFejl("Ugyldig dato. Brug formatet dd-MM-yyyy.");
            }
        }
    }
}
