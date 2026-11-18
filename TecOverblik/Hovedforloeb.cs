using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    internal abstract class Hovedforloeb : IHovedforloeb
    {
        // protected: sadece bu sınıf ve türeyenler (H1, H2) erişir, menü erişemez
       protected readonly List<Fag> _fag;
       protected readonly List<Underviser> _underviser;
       protected readonly List<Elev> _elev;

       public string Navn { get; }

       // protected constructor: sadece H1Forloeb ve H2Forloeb çağırabilir, ortak veriyi buraya gönderirler
       protected Hovedforloeb(string navn, List<Fag> fagListe, List<Underviser> underviserListe, List<Elev> elevListe)
       {
           Navn = navn;
           _fag = fagListe;
           _underviser = underviserListe;
           _elev = elevListe;
       }

        // Dışarıya salt okunur görünüm
        public IReadOnlyList<Fag> FagListe => _fag.AsReadOnly();
        public IReadOnlyList<Underviser> UnderviserListe => _underviser.AsReadOnly();
        public IReadOnlyList<Elev> ElevListe => _elev.AsReadOnly();

        // abstract: gövde YOK, her hovedforløb kendi versiyonunu yazmak ZORUNDA (ödev şartı)
        public abstract void AddStudent(Elev elev, Fag fag);
        public abstract bool DeleteStudent(Elev elev);
        public abstract SøgeResultat GetData(Søgekriterie kritere, string søgeord);

        // Ortak yardımcılar: iki türeyen sınıf da kullanır, kod tekrarlanmaz
        protected static string Fuldt(Person p) =>  p.Fornavn + " " + p.Efternavn;

        protected static bool Er(string tekst, string søgeord) => string.Equals(tekst, søgeord.Trim(), StringComparison.OrdinalIgnoreCase); // büyük/küçük harf fark etmez

        // Öğrenciyi TÜM fag'lardan ve öğrenci listesinden çıkarır (ödev: "slettet fra alle fag")
        protected bool FjernElevFraAlt(Elev elev)
        {
            foreach (var f in _fag)
            {
                f.FjernElev(elev);  // elev o fag'da değilse zararsız, false döner
            }
        
            return _elev.Remove(elev); // listeden de çıkar, vardıysa true
        }



    }
}
