namespace metjelentes
{
    class Jelentés
    {
        public string Településkód { get; private set; }
        public string Idő { get; private set; }
        public string Szél { get; private set; }
        public int Hőmérséklet { get; private set; }
        public string IdőÚj => Idő.Insert(2, ":");
        public string Óra => Idő.Substring(0, 2);
        public bool Szélcsend => Szél == "00000";
        private int Szélerősség => int.Parse(Szél.Substring(3, 2));
        public string SzélerősségHashtag => "".PadRight(Szélerősség, '#');

        public Jelentés(string sor)
        {
            string[] m = sor.Split();
            Településkód = m[0];
            Idő = m[1];
            Szél = m[2];
            Hőmérséklet = int.Parse(m[3]);
        }
        public override string ToString() => $"{Településkód} {IdőÚj} {Hőmérséklet} fok.";
    }
}
