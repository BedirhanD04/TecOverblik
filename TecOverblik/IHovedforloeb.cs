using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // Menü sadece bu sözleşmeyi bilir (gevşek bağlılık)
    internal interface IHovedforloeb
    {
        string Navn { get; }              // "H1" ya da "H2"

        // IReadOnlyList: menü listeleri okuyabilir ama dışarıdan Add/Remove yapamaz
        IReadOnlyList<Fag> FagListe { get; }
        IReadOnlyList<Underviser> UnderviserListe { get; }
        IReadOnlyList<Elev> ElevListe { get; }

        void AddStudent(Elev elev, Fag fag);                  // öğrenciyi belirli bir fag altında oluştur
        bool DeleteStudent(Elev elev);                        // tüm fag'lardan sil, silindiyse true
        SøgeResultat GetData(Søgekriterie kritere, string søgeord); // HER ZAMAN tek DTO döner

    }
}
