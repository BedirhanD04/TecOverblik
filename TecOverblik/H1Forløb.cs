using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // sealed: bundan kimse türemesin. ": Hovedforloeb" = ortak kodu miras al
    internal sealed class H1Forløb : Hovedforloeb
    {
        //H1'in kendi listelerini base'e gönder
        public H1Forløb()
    : base("H1", TestData.H1FagList, TestData.H1UnderviserList, TestData.H1ElevList)
        {
        }

        // override: base'deki abstract metodun H1'e özel yazılışı (polimorfizm)
        public override void AddStudent(Elev elev, Fag fag)
        {
            if (!_fag.Contains(fag))  // başka hovedforløb'un fag'ına öğrenci eklenmesin
                throw new ArgumentException("Faget tilhører ikke H1.", nameof(fag));

            fag.AddElev(elev); // fag'a ekle
            if(!_elev.Contains(elev)) // genel listede yoksa ekle
            {
                _elev.Add(elev);
                _elev.Sort();
            }
        }

        public override bool DeleteStudent(Elev elev) => FjernElevFraAlt(elev);

        public override SøgeResultat GetData(Søgekriterie kritere, string søgeord)
        {
            // switch expression:
            IEnumerable<Fag> match = kritere switch
            {
                 Søgekriterie.Fag => _fag.Where(f => Er(f.Navn, søgeord)),
                 Søgekriterie.Underviser => _fag.Where(f => Er(Fuldt(f.Underviser), søgeord)),
                Søgekriterie.Elev => _fag.Where(f => f.Elever is not null && f.Elever.Any(e => Er(Fuldt(e), søgeord))),
                _ => throw new ArgumentOutOfRangeException(nameof(kritere))
            };

            var list = match.Select(f => new FagResultat(f)).ToList();
            return new SøgeResultat(list.AsReadOnly());

        }








    }
}
