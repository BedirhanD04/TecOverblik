using System;
using System.Collections.Generic;
using System.Text;

namespace TecOverblik
{
    // IComparable<Person>: Sort() metodunun iki Person'ı nasıl sıralayacağını bilmesi için
    public abstract record Person : IComparable<Person>
    {
        // sadece get: değerler constructor'da bir kez atanır, sonra değişmez (immutable)
        public string Fornavn { get; }
        public string Efternavn { get; }
        public DateOnly Fødselsdato { get; }

        protected Person(string forNavn, string efternavn, DateOnly fødselsdato)
        {
            // NOT NULL kuralı: boş ya da sadece boşluk olan isimleri reddet
            if (string.IsNullOrWhiteSpace(forNavn))
                throw new ArgumentException("ForNavn må ikke være tomt eller kun bestå af mellemrum.", nameof(forNavn));
            if(string.IsNullOrWhiteSpace(efternavn))
                throw new ArgumentException("Efternavn må ikke være tomt eller kun bestå af mellemrum.", nameof(efternavn));

            // Gelecekte doğum olamaz. Bunu yakalamazsak Alder negatif yaş üretmeye çalışıp hata fırlatırdı
            if (fødselsdato > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Fødselsdato kan ikke ligge i fremtiden.", nameof(fødselsdato));

            Fornavn = forNavn.Trim();
            Efternavn = efternavn.Trim();
            Fødselsdato = fødselsdato;
        }

        // Yaş doğum tarihinden HESAPLANIR (kullanıcıdan alınmaz). Tipi int değil, kendi struct'ımız
        public Alder Alder => Alder.FraFøldselsdato(Fødselsdato, DateOnly.FromDateTime(DateTime.Today));

        // Varsayılan sıralama: önce ada (fornavn) göre, eşitse soyada göre
        public int CompareTo(Person? other)
        {
            if (other is null) return 1; // null her zaman küçük sayılır
            int fornavnComparison = string.Compare(Fornavn, other.Fornavn, StringComparison.OrdinalIgnoreCase);
            if (fornavnComparison != 0)
                return fornavnComparison;  // adlar farklıysa sonuç bu
            return string.Compare(Efternavn, other.Efternavn, StringComparison.OrdinalIgnoreCase);
        }


    }
}
