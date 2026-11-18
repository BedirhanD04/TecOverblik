using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // Nesne üretmekten sorumlu sınıf. static: nesne oluşturmadan HovedforloebFactory.Opret(...) diye çağrılır
    internal static class HovedforloebFactory
    {
        // Dönüş tipi INTERFACE: çağıran hangi sınıfın geldiğini bilmez
        // switch expression: seçime göre doğru somut sınıfı üretir
        public static IHovedforloeb Opret(int valg) => valg switch
        {
            1 => new H1Forløb(),
            2 => new H2Forløb(),
            _ => throw new ArgumentException("Ugyldigt hovedforløb", nameof(valg))
        };
    }
}
