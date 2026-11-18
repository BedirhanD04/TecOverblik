using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // GetData bunu döndürür: fag, öğretmen ya da öğrenci aransa da HER ZAMAN bu tek tip
    // Positional record: parantezdeki parametreler init-only property olur, yani immutable
    internal sealed record SøgeResultat(IReadOnlyList<FagResultat> FagListe);

}
