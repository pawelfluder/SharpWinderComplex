using SharpCryptoCalcProg.ASheetObjects;

namespace SharpCryptoCalcProg
{
    internal class BalanceConverter
    {
        private Dictionary<string, decimal> accountsDict;

        public BalanceConverter()
        {
            accountsDict = new Dictionary<string, decimal>();
        }

        public void Convert(
            IEnumerable<Transactions> inputList,
            out List<Balance> orderedBalanceList,
            out Dictionary<string, decimal> accountsDict)
        {
            accountsDict = new Dictionary<string, Account>();
            var balanceList = new List<Balance>();
            var orderedList = inputList.OrderBy(x => x.Id);

            foreach (var tra in orderedList)
            {
                AddTransaction(tra.From, "-" + tra.FromSend);
                AddTransaction(tra.To, tra.ToReceived);
                var frombalance = accountsDict[tra.From].ToString().Replace('.', ',');
                var tobalance = accountsDict[tra.To].ToString().Replace('.', ',');
                var balance = new Balance()
                {
                    Id = tra.Id,
                    Date = tra.Date,
                    Hour = tra.Hour,
                    From = tra.From,
                    FromBalance = frombalance,
                    FromDiff = "-" + tra.FromSend,
                    To = tra.To,
                    ToBalance = tobalance,
                    ToDiff = tra.ToReceived
                };

                
                balanceList.Add(balance);
            }

            orderedBalanceList = balanceList.OrderByDescending(x => x.Id).ToList();
        }

        private void AddTransaction(
            string accountName,
            string valueString)
        {
            var value = decimal.Parse(valueString.Replace(',','.'));
            
            if (!accountsDict.ContainsKey(accountName))
            {
                accountsDict.Add(accountName, value);
                return;
            }

            var current = accountsDict[accountName];
            var sum = current + value;
            accountsDict[accountName] = sum;
        }
    }
}
