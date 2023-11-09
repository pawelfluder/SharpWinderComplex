namespace SharpCryptoCalcProg.ASheetObjects
{
    internal class Accounts
    {
        public Accounts()
        {
        }

        public Accounts(string name, string balance)
        {
            Name = name;
            Balance = balance;
        }

        public string Name { get; set; }

        public string Balance { get; set; }
    }
}
