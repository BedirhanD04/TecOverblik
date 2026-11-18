using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    internal sealed record Underviser : Person
    {
        public Underviser(string fornavn, string efternavn, DateOnly fødselsdato) : base(fornavn, efternavn, fødselsdato)
        {
        }
    }
}
