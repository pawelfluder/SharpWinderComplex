using SharpCryptoCalcProg.ASheetObjects;

namespace SharpCryptoCalcProg
{
    internal class BalanceConverter
    {
        private List<Accounts> accountsList;

        public BalanceConverter()
        {
            accountsList = new List<Accounts>();
        }

        public void Convert(
            IEnumerable<Transactions> inputList,
            IEnumerable<Accounts> accounts,
            out List<Balances> outBalanceList,
            out List<Accounts> outAccountsList)
        {
            var balanceList = new List<Balances>();
            var orderedList = inputList.OrderBy(x => x.Id);

            foreach (var tra in orderedList)
            {
                AddTransaction(tra.From, "-" + tra.FromSend);
                AddTransaction(tra.To, tra.ToReceived);
                var frombalance = accountsList
                    .Single(x => x.Name == tra.From)
                    .Balance.ToString().Replace('.', ',');
                var tobalance = accountsList
                    .Single(x => x.Name == tra.To)
                    .Balance.ToString().Replace('.', ',');
                var balance = new Balances()
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

            outBalanceList = balanceList.OrderByDescending(x => x.Id).ToList();
            outAccountsList = GetAccounts(accounts);
            accountsList.Clear();
        }

        private List<Accounts> GetAccounts(
            IEnumerable<Accounts> accounts)
        {
            var gg = new List<Accounts>(accountsList);
            if (accounts != null)
            {
                foreach (var ac in accountsList)
                {
                    var gg2 = accounts.SingleOrDefault(x => x.Name == ac.Name);
                    if (gg2 != null)
                    {
                        ac.Real = gg2.Real;
                    }
                }
            }

            gg = gg.OrderByDescending(x => x.Name).ToList();

            return gg;
        }

        private void AddTransaction(
            string accountName,
            string valueString)
        {
            var account = accountsList.SingleOrDefault(x => x.Name == accountName);
            if (account == default)
            {
                account = new Accounts(accountName, valueString);
                accountsList.Add(account);
                return;
            }

            var valueToAdd = decimal.Parse(valueString.Replace(',', '.'));
            var valueCurrent = decimal.Parse(account.Balance.Replace(',', '.'));
            var sum = valueCurrent + valueToAdd;
            account.Balance = sum.ToString().Replace('.', ',');
        }
    }
}
