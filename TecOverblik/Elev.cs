using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    internal sealed record Elev : Person
    {
        // ": Person" = Person'dan miras alır (ad, soyad, doğum tarihi, yaş, sıralama hazır gelir)
        // sealed: bu sınıftan kimse türeyemez
        public Elev(string fornavn, string efternavn, DateOnly fødselsdato) : base(fornavn, efternavn, fødselsdato)
        {
        }

        // reglen er inde i Alder-structen
        public bool ErUnder18 => Alder.ErUnder18;
    }
}
