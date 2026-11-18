using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    internal sealed class H2Forløb : Hovedforloeb
    {
        // H2'nin kendi listelerini base'e gönderir, H1'e hiç dokunmaz
        public H2Forløb()
            : base("H2", TestData.H2FagList, TestData.H2UnderviserList, TestData.H2ElevList)
        {
        }

        public override void AddStudent(Elev elev, Fag fag)
        {
            if (!_fag.Contains(fag))                       // başka hovedforløb'un fag'ına öğrenci eklenmesin
                throw new ArgumentException("Faget tilhører ikke H2.", nameof(fag));

            fag.AddElev(elev);                         // fag'a ekle
            if (!_elev.Contains(elev))                   // genel listede yoksa ekle
            {
                _elev.Add(elev);
                _elev.Sort();                            // alfabetik düzen bozulmasın
            }
        }

        public override bool DeleteStudent(Elev elev) => FjernElevFraAlt(elev);

        public override SøgeResultat GetData(Søgekriterie kriterie, string soegeord)
        {
            IEnumerable<Fag> match = kriterie switch
            {
                Søgekriterie.Fag => _fag.Where(f => Er(f.Navn, soegeord)),
                Søgekriterie.Underviser => _fag.Where(f => Er(Fuldt(f.Underviser), soegeord)),
                Søgekriterie.Elev => _fag.Where(f => f.Elever is not null && f.Elever.Any(e => Er(Fuldt(e), soegeord))),
                _ => throw new ArgumentOutOfRangeException(nameof(kriterie))
            };

            var liste = match.Select(f => new FagResultat(f)).ToList();
            return new SøgeResultat(liste.AsReadOnly());
        }
    }
}