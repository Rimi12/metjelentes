using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace metjelentes
{
    class Jelentés
    {
        public string TelepülésKód { get; private set; }
        public string Idő { get; private set; }
        public string Szél { get; private set; }
        public int Hőmérséklet { get; private set; }
        public string IdőÚj => Idő.Insert(2, ":");
        public string Óra => Idő.Substring(0, 2);
        public bool Szélcsend => Szél == "00000";
        private int Erősség => int.Parse(Szél.Substring(3, 2));
        public string ErősségHashtag => "".PadRight(Erősség, '#');

        public Jelentés(string sor)
        {
            string[] m = sor.Split();
            TelepülésKód = m[0];
            Idő = m[1];
            Szél = m[2];
            Hőmérséklet = int.Parse(m[3]);
        }
        public override string ToString() => $"{TelepülésKód} {IdőÚj} {Hőmérséklet} fok.";
    }
    class MetJelentes
    {
        static void Main()
        {
            List<Jelentés> jelentések = new List<Jelentés>();
            foreach (var sor in File.ReadAllLines("tavirathu13.txt")) jelentések.Add(new Jelentés(sor));

            Console.WriteLine("2. feladat\nAdja meg egy település kódját! Település: ");
            string inputKód = Console.ReadLine();
            Console.WriteLine($"Az utolsó mérési adat a megadott településről {jelentések.Where(x=> x.TelepülésKód == inputKód).Last().IdőÚj}-kor érkezett.");

            Console.WriteLine("3. feladat");
            Console.WriteLine($"A legalacsonyabb hőmérséklet: {jelentések.OrderBy(x=>x.Hőmérséklet).First()}");
            Console.WriteLine($"A legmagasabb hőmérséklet: {jelentések.OrderBy(x => x.Hőmérséklet).Last()}");

            Console.WriteLine("4. feladat");
            var szélcsend = jelentések.Where(x => x.Szélcsend);
            if (szélcsend.Count() != 0) Console.Write(szélcsend.Aggregate("", (c, n) => c += $"{n.TelepülésKód} {n.IdőÚj}\n"));
            else Console.WriteLine("Nem volt szélcsend a mérések idején.");

            Console.WriteLine("5. feladat");
            HashSet<string> telpülésKódok = new HashSet<string>(jelentések.Select(x=>x.TelepülésKód));
            foreach (var i in telpülésKódok)
            {
                var aktTelepülésMérései = jelentések.Where(x => x.TelepülésKód == i);
                int min = aktTelepülésMérései.Min(x => x.Hőmérséklet);
                int max = aktTelepülésMérései.Max(x => x.Hőmérséklet);
                int ingadozás = max - min;
                var aktTelepülésMéréseiÁtlaghoz = aktTelepülésMérései.Where(x => "01 07 13 19".Contains(x.Óra));
                HashSet<string> átlagÓrái = new HashSet<string>(aktTelepülésMéréseiÁtlaghoz.Select(x => x.Óra));
                string középhőmérséklet = átlagÓrái.Count != 4 ? "NA" : $"Középhőmérséklet: {aktTelepülésMéréseiÁtlaghoz.Average(x => x.Hőmérséklet):F0}";
                Console.WriteLine($"{i} {középhőmérséklet}; Hőmérséklet-ingadozás: {ingadozás}");
            }

            Console.WriteLine("6. feladat\nA fájlok elkészültek.");
            foreach (var i in telpülésKódok)
            {
                List<string> ki = new List<string>() {i};
                foreach (var j in jelentések.Where(x => x.TelepülésKód == i))
                {
                    ki.Add($"{j.IdőÚj} {j.ErősségHashtag}");
                }
                File.WriteAllLines($"{i}.txt", ki);
            }
            Console.ReadKey();
        }
    }
}
