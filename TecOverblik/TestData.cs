using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TecOverblik
{
    internal class TestData
    {
        // H1 listeleri: static (ödev şartı), sınıf static değil
        public static List<Fag> H1FagList { get; } = new List<Fag>();
        public static List<Elev> H1ElevList { get; } = new List<Elev>();
        public static List<Underviser> H1UnderviserList { get; } = new List<Underviser>();

        // H2 listeleri: tamamen ayrı
        public static List<Fag> H2FagList { get; } = new List<Fag>();
        public static List<Elev> H2ElevList { get; } = new List<Elev>();
        public static List<Underviser> H2UnderviserList { get; } = new List<Underviser>();

        // Veri constructor'da üretilir (ödev şartı)
        public TestData()
        {
            // Listeler static olduğu için önce temizle, yoksa ikinci new TestData() veriyi ikiye katlar
            H1FagList.Clear(); H1ElevList.Clear(); H1UnderviserList.Clear();
            H2FagList.Clear(); H2ElevList.Clear(); H2UnderviserList.Clear();

            OpretH1();
            OpretH2();

            // Alfabetik sıralama: Sort(), Person'daki CompareTo'yu kullanır
            H1ElevList.Sort(); H1UnderviserList.Sort();
            H2ElevList.Sort(); H2UnderviserList.Sort();
        }

        // Yardımcı metotlar: new Elev(..., new DateOnly(...)) yazmayı kısaltır, veri tablosu okunaklı kalır
        private static Elev E(string fornavn, string efternavn, int aar, int maaned, int dag)
            => new Elev(fornavn, efternavn, new DateOnly(aar, maaned, dag));

        private static Underviser U(string fornavn, string efternavn, int aar, int maaned, int dag)
            => new Underviser(fornavn, efternavn, new DateOnly(aar, maaned, dag));

        private static void OpretH1()
        {
            var anders = U("Anders", "Kjeldsen", 1978, 3, 14);
            var maria = U("Maria", "Holm", 1983, 8, 2);
            var peter = U("Peter", "Lund", 1975, 11, 21);
            H1UnderviserList.AddRange(new[] { anders, maria, peter });

            // 15 öğrenci. Son üçü 18 yaş altı (kırmızı yazılacak). İki "Emil": soyad sıralamasını sınar
            var elever = new List<Elev>
            {
                E("Emil", "Jensen", 2005, 2, 11),
                E("Emil", "Andersen", 2004, 9, 30),
                E("Frederik", "Nielsen", 2003, 5, 7),
                E("Oliver", "Hansen", 2006, 1, 19),
                E("Noah", "Pedersen", 2005, 12, 4),
                E("William", "Christensen", 2004, 4, 23),
                E("Lucas", "Larsen", 2006, 7, 16),
                E("Mikkel", "Sørensen", 2003, 10, 9),
                E("Jonas", "Rasmussen", 2005, 6, 28),
                E("Rasmus", "Jørgensen", 2004, 3, 5),
                E("Kasper", "Petersen", 2006, 11, 2),
                E("Victor", "Madsen", 2005, 8, 14),
                E("Sofie", "Kristensen", 2009, 6, 15),    // 18 yaş altı
                E("Freja", "Olsen", 2010, 2, 3),          // 18 yaş altı
                E("Ida", "Thomsen", 2009, 12, 2),         // 18 yaş altı
            };
            H1ElevList.AddRange(elever);

            // Her fag'da farklı sayıda öğrenci: Take/Skip ile aynı Elev nesneleri birden fazla fag'da olur
            // (Fag constructor listeyi kopyalar, orijinal 'elever' listesi bozulmaz)
            H1FagList.Add(new Fag("Grundlæggende programmering", anders, elever.Take(15).ToList()));   // 15
            H1FagList.Add(new Fag("OOP", maria, elever.Take(14).ToList()));                            // 14
            H1FagList.Add(new Fag("Computerteknologi", anders, elever.Skip(1).Take(15).ToList()));     // 14
            H1FagList.Add(new Fag("Serversikkerhed", peter, elever.Skip(2).Take(12).ToList()));        // 12
            H1FagList.Add(new Fag("Database programmering", peter, elever.Take(13).ToList()));         // 13
            H1FagList.Add(new Fag("Database Server", maria, elever.Skip(3).Take(12).ToList()));        // 12
            H1FagList.Add(new Fag("Clientside programmering", maria, elever.Skip(1).Take(14).ToList())); // 14
        }

        private static void OpretH2()
        {
            var louise = U("Louise", "Bech", 1981, 5, 25);
            var thomas = U("Thomas", "Dahl", 1976, 9, 8);
            H2UnderviserList.AddRange(new[] { louise, thomas });

            var elever = new List<Elev>
            {
                E("Alma", "Berg", 2004, 3, 12),
                E("August", "Holst", 2005, 7, 1),
                E("Cecilie", "Winther", 2003, 11, 18),
                E("Magnus", "Lindholm", 2006, 2, 27),
                E("Nanna", "Skov", 2004, 8, 6),
                E("Oscar", "Friis", 2005, 10, 22),
                E("Signe", "Mølgaard", 2003, 1, 31),
                E("Tobias", "Vestergaard", 2006, 5, 13),
                E("Astrid", "Brandt", 2004, 12, 9),
                E("Viggo", "Hald", 2009, 9, 17),         // 18 yaş altı
                E("Karoline", "Bak", 2010, 4, 20),       // 18 yaş altı
                E("Bjørn", "Eskildsen", 2005, 6, 3),
            };
            H2ElevList.AddRange(elever);

            H2FagList.Add(new Fag("Webudvikling", louise, elever.Take(12).ToList()));
            H2FagList.Add(new Fag("Netværk og sikkerhed", thomas, elever.Take(10).ToList()));
            H2FagList.Add(new Fag("Databaser", thomas, elever.Skip(2).Take(10).ToList()));
            H2FagList.Add(new Fag("Mobilapplikationer", louise, elever.Skip(4).Take(8).ToList()));
            H2FagList.Add(new Fag("Projektstyring", louise));   // öğrenci listesi verilmedi: null (ödev: NULLABLE)
        }
    }
}