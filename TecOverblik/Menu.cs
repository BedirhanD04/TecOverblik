using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    internal sealed class Menu
    {
        public void Køre()
        {
            while (true)
            {
                int valg = VælgFraMenu(new[] { "H1", "H2" }, "Vælg hovedforløb: ", "Ugyldigt valg.");

                // Somut sınıf SADECE burada, kullanıcı seçince üretilir. Menü onu IHovedforloeb olarak tutar
                IHovedforloeb hovedforloeb = HovedforloebFactory.Opret(valg);
                VisHovedforløbMenu(hovedforloeb);
            }
        }

        private void VisHovedforløbMenu(IHovedforloeb hf)
        {
            while (true)
            {
                int valg = VælgFraMenu(new[]
                {
                    "Se " + hf.Navn + " fag info",
                    "Se " + hf.Navn + " lærer info",
                    "Se " + hf.Navn + " elev info",
                    "Tilbage"
                }, "Vælg 1, 2, 3 eller 4: ");

                switch(valg)
                {
                    case 1: VisFagInfo(hf); break;
                    case 2: VisLæerInfo(hf); break;
                    case 3: VisElevMenu(hf); break;
                    case 4: return;
                }
            }
        }

        // TÜM menüler bunu kullanır: numaralı menüyü gösterir, geçersiz girişte rengi kırmızı hata ile yeniden çizer
        // fejlTekst parametresi eklendi, varsayılanı "Ingen match fundet!"
        private static int VælgFraMenu(IReadOnlyList<string> punkter, string prompt, string fejlTekst = "Ingen match fundet!")
        {
            string? fejl = null; //ilk gösterimde hata yok

            while (true)
            {
                if (fejl is not null)
                {
                    Console.Clear();
                    KonsolHjaelper.VisFejl(fejl);             // kırmızı hata (tek yerde yazıldı, yeniden kullanılıyor)
                }
                for(int i = 0; i < punkter.Count; i++)
                {
                    Console.WriteLine((i + 1) + ") " + punkter[i]);   // numarayı otomatik üret
                }
                Console.WriteLine(prompt);

                // TryParse: tip dönüşümü, metin sayı değilse program çökmez
                if (int.TryParse(Console.ReadLine(), out int valg) && valg >= 1 && valg <= punkter.Count)
                {
                    return valg;
                }

                fejl = fejlTekst;    // sonraki turda kırmızı gösterilecek
            }
        }

        private void VisFagInfo(IHovedforloeb hf)
        {
            List<string> fagNavne = hf.FagListe.Select(f => f.Navn).ToList();
            int valg = VælgFraMenu(fagNavne, "Vælg en fag id fra listen foroven: ");

            // valg starter fra 1, listeindekset fra 0: træk én fra
            // valg 1'den başlar, liste indeksi 0'dan: bir eksilt
            SøgeResultat resultat = hf.GetData(Søgekriterie.Fag, fagNavne[valg - 1]);
            SkrivResultat(resultat); ;
        }

        private void VisLæerInfo(IHovedforloeb hf)
        {
            List<string> navne = hf.UnderviserListe.Select(u => u.Fornavn + " " + u.Efternavn).ToList();
            int valg = VælgFraMenu(navne, "Vælg en lærer id fra listen foroven: ");

            SøgeResultat resultat = hf.GetData(Søgekriterie.Underviser, navne[valg - 1]);
            SkrivResultat(resultat);
        }


        private static void SkrivResultat(SøgeResultat resultat)
        {
            foreach(FagResultat fag in resultat.FagListe)
            {
                Console.WriteLine("    Fag: " + fag.Fagnavn);
                Console.WriteLine("    Lærer: " + fag.Underviser.Fornavn + " " + fag.Underviser.Efternavn);
                Console.WriteLine("    Antal elever ialt: " + fag.Elever.Count);
                foreach (Elev elev in fag.Elever)                   // DTO içinde zaten alfabetik sıralı
                    KonsolHjaelper.SkrivElev(elev, "        ");

                Console.WriteLine();   // fag'lar arasında boş satır
            }
        }

        private void VisElevMenu(IHovedforloeb hf)
        {
            while (true)
            {
                int valg = VælgFraMenu(new[] { "Opret ny elev", "Slet en elev", "Se elev fag", "Tilbage" },
                                        "Vælg 1, 2, 3 eller 4: ");
                switch (valg)
                {
                    case 1: OpretElev(hf); break;
                    case 2: SletElev(hf); break;
                    case 3: VisElevFag(hf); break;
                    case 4: return;
                }
            }
        }

        private void OpretElev(IHovedforloeb hf)
        {
            // Kullanıcıdan veri al (boş ad ve geçersiz tarih KonsolHjaelper içinde reddedilir)
            string fornavn = KonsolHjaelper.LaesTekst("Fornavn: ");
            string efternavn = KonsolHjaelper.LaesTekst("Efternavn: ");
            DateOnly foedselsdato = KonsolHjaelper.LæseDato("Fødselsdato (dd-MM-yyyy): ");

            // Ödev: öğrenci belirli bir fag altında oluşturulur, o yüzden fag seçtiriyoruz
            List<string> fagNavne = hf.FagListe.Select(f => f.Navn).ToList();
            int valg = VælgFraMenu(fagNavne, "Vælg en fag id til eleven: ");
            Fag fag = hf.FagListe[valg - 1];                       // menü 1'den başlar, liste 0'dan: bir eksilt

            try
            {
                // new Elevyeni nesneyi üretir, AddStudent hovedforløb'un KENDİ versiyonuyla ekler (polimorfizm)
                hf.AddStudent(new Elev(fornavn, efternavn, foedselsdato), fag);
                Console.WriteLine("Eleven er oprettet i " + fag.Navn + ".");
            }
            catch (InvalidOperationException ex)                   // aynı öğrenci bu fag'da zaten varsa Fag.TilfoejElev fırlatır
            {
                KonsolHjaelper.VisFejl(ex.Message);                // ortak kırmızı hata metodu
            }
        }

        private void SletElev(IHovedforloeb hf)
        {
            List<string> navne = hf.ElevListe.Select(e => e.Fornavn + " " + e.Efternavn).ToList();
            if (navne.Count == 0)
            {
                KonsolHjaelper.VisFejl("Ingen elever at slette.");
                return;
            }

            int valg = VælgFraMenu(navne, "Vælg en elev id fra listen foroven: ");
            Elev elev = hf.ElevListe[valg - 1];

            hf.DeleteStudent(elev);
            Console.WriteLine(navne[valg - 1] + " er slettet fra alle fag.");
        }

        private void VisElevFag(IHovedforloeb hf)
        {
            string navn = KonsolHjaelper.LaesTekst("Angive elevens navn:");
            SøgeResultat resultat = hf.GetData(Søgekriterie.Elev, navn);

            if (resultat.FagListe.Count == 0)     // if there is no match
            {
                KonsolHjaelper.VisFejl("Ingen match fundet!");
                return;
            }


            foreach (FagResultat fag in resultat.FagListe)
            {
                Console.WriteLine("    Fag: " + fag.Fagnavn);
                Console.WriteLine("    Lærer: " + fag.Underviser.Fornavn + " " + fag.Underviser.Efternavn);
                Console.WriteLine();
            }

        }

    }
}
