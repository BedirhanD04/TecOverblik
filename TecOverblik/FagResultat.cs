using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // Bir fag'ın o anki halinin DEĞİŞTİRİLEMEZ kopyası
    internal sealed record FagResultat
    {
        public string Fagnavn { get; } // sadece get: immutable
        public Underviser Underviser { get; }
        public IReadOnlyList<Elev> Elever { get; }  // salt okunur liste

        public FagResultat(Fag fag)
        {
            Fagnavn = fag.Navn;
            Underviser = fag.Underviser;

            // Fag'ın öğrenci listesi null olabilir (ödev: NULLABLE), o zaman boş koleksiyon kullan
            IEnumerable<Elev> kilde = fag.Elever ?? Enumerable.Empty<Elev>();

            var kopi = new List<Elev>(kilde);  // KOPYA: sonradan Fag değişse bile DTO değişmez
            kopi.Sort();  // ödev: yerleşik Sort() ile alfabetik (fornavn'a göre)
            Elever = kopi.AsReadOnly();        // dışarıya sadece okuma izni
        }

    }
}
