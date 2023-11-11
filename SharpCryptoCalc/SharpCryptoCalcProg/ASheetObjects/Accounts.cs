namespace SharpCryptoCalcProg.ASheetObjects
{
    internal class Accounts
    {
        public Accounts()
        {
        }

        public Accounts(
            string name,
            string balance)
        {
            Name = name;
            Balance = balance;
        }

        public string Name { get; set; }

        public string Balance { get; set; }

        public string Real { get; set; }

        public string Diff { get; set; }

        public static Dictionary<char, string> GetFormulas()
        {
            var f = new Dictionary<char, string>();
            f.Add('D', "=C4-B4");
            return f;
        }
    }
}
