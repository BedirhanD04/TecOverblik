using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    internal sealed class Fag
    {
        public string Navn { get; } // NOT NULL, sadece get
        public Underviser Underviser { get; }

        // private: dışarıdan doğrudan Add/Remove yapılamaz (kapsülleme)
        // List<Elev>? = soru işareti: liste null olabilir (ödev: NULLABLE)
        private List<Elev>? _elever;

        public Fag(string navn, Underviser underviser, List<Elev>? elever = null)
        {
            if(string.IsNullOrWhiteSpace(navn))
                throw new ArgumentException("Navn må ikke være tomt.", nameof(navn));

            Navn = navn.Trim();
            Underviser = underviser ?? throw new ArgumentNullException(nameof(underviser)); // null ise hata fırlat
            _elever = elever is null ? null : new List<Elev>(elever); // KOPYA al, dışarıdaki liste sonradan bozmasın
        }

        // IReadOnlyList: dışarı sadece okuma izni verir. Sonuç null olabilir
        public IReadOnlyList<Elev>? Elever => _elever;

        public void AddElev(Elev elev)
        {
            _elever ??= new List<Elev>();                            // liste null ise oluştur

            if (_elever.Contains(elev))                              // aynı öğrenci zaten bu fag'da mı? (record eşitliği)
                throw new InvalidOperationException("Eleven er allerede tilmeldt dette fag.");

            _elever.Add(elev);
        }

        // ?. : liste null ise Remove çağrılmaz. ?? false : null geldiyse false döndür
        public bool FjernElev(Elev elev) => _elever?.Remove(elev) ?? false;

        // Overload: aynı isim (FjernElev), aynı sınıf, FARKLI parametre (string). Derleyici çağrıdaki tipe bakıp seçer
        // Overload : samme navn (FjernElev), samme klasse, FORSKELLIG parameter (string). Kompilatoren vælger baseret på typen i kaldet
        public bool FjernElev(string fuldtNavn)
        {
            if (_elever is null) return false;                      // if its null, therese nobosy to delete

            // RemoveAll: koşula uyan HEPSİNİ siler ve silinen sayısını döndürür
            // RemoveAll: Fjerner ALLE, der opfylder betingelsen, og returnerer antallet af slettede elementer
            int antal = _elever.RemoveAll(e =>
                string.Equals(e.Fornavn + " " + e.Efternavn, fuldtNavn.Trim(), StringComparison.OrdinalIgnoreCase));

            return antal > 0;                                       // en az biri silindiyse true // true, hvis mindst én blev slettet

        }
    }
}
