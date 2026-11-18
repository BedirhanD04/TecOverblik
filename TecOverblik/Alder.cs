using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // struct = DEĞER tipi (stack'te durur, atamada kopyalanır). readonly = oluşturulduktan sonra değişmez
    // struct = VÆRDI-type (ligger på stack'en, kopieres ved tildeling). readonly = kan ikke ændres efter oprettelse
    public readonly record struct Alder
    {
        public int Aar { get; }

        public Alder(int aar)
        {
            // Ugyldig alder afvises af selve typen (typesikkerhed)
            if (aar < 0 || aar > 150)
            throw new ArgumentOutOfRangeException(nameof(aar), "Alder skal være mellem 0 og 150.");
            Aar = aar;
        }
        public bool ErUnder18 => Aar < 18;

        public static Alder FraFøldselsdato(DateOnly fødselsdato, DateOnly idag)
        {
            int aar = idag.Year - fødselsdato.Year;
            if(fødselsdato > idag.AddYears(-aar)) aar--;
            return new Alder(aar);

        }
        public override string ToString() => Aar + " år";
    }
}
