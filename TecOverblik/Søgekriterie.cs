using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // enum: sabit, adlandırılmış seçenekler. "1", "2", "3" gibi sihirli sayılar yerine okunur isimler
    // Type-safety: derleyici bu üç değerin dışında bir şey kabul etmez (Reflektion: "type-safety med enum")
    internal enum Søgekriterie
    {
        Elev,
        Underviser,
        Fag
    }
}
