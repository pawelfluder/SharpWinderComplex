using GameSynchCoreProj;
using SharpCryptoCalcProg.Register;
using SharpGoogleSheetProg.AAPublic;
using SharpRepoServiceProg.Service;
using SharpSetupProg21Private.AAPublic.Extensions;
using SharpCryptoCalcProg.Info;
using SharpCryptoCalcProg.ASheetObjects;

namespace SharpCryptoCalcProg
{
    internal class CryptoCalcService
    {
        private readonly SychService synchService;
        private readonly SheetInfoGroup sheetGroup;
        private readonly IRepoService repoService;
        private readonly IGoogleSheetService sheetService;
        private readonly BalanceConverter balanceConverter;

        public CryptoCalcService()
        {
            synchService = new SychService();
            sheetGroup = new SheetInfoGroup(
                new SheetInfoCache(),
                synchService.RegisterSheet);
            
            repoService = MyBorder.Container.Resolve<IRepoService>();
            sheetService = MyBorder.Container.Resolve<IGoogleSheetService>();
            balanceConverter = new BalanceConverter();
        }

        public void Sync()
        {
            var transactionsSheet = sheetGroup.Get<Transactions>();
            var balanceSheet = sheetGroup.Get<Transactions>();
            var accountsSheet = sheetGroup.Get<Account>();

            synchService.SyncSheet<Transactions>(transactionsSheet.Names);
            var transactionsList = repoService.GetItemList<Transactions>(transactionsSheet.Names);
            balanceConverter.Convert(
                transactionsList,
                out var balancesList,
                out var accountsDict);

            sheetService.Worker.PasteDataToSheet(
                balanceSheet.SpreadSheetId,
                balanceSheet.SheetTabName,
                ToIListOfIList(balancesList),
                balanceSheet.ColumnNames);

            sheetService.Worker.PasteDataToSheet(
                accountsSheet.SpreadSheetId,
                accountsSheet.SheetTabName,
                ToIListOfIList(accountsDict),
                accountsSheet.ColumnNames);
        }

        private List<string> GetPropertyNames(Type type)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            return propertyNames;
        }

        public IList<IList<object>> ToIListOfIList<T>(IEnumerable<T> inputList) where T : class
        {
            var result = inputList.Select(x => ToIList(x)).ToList();
            return result;
        }

        public IList<object> ToIList(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var result = new List<object>();
            var id = string.Empty;

            foreach (var property in properties)
            {
                var value = property.GetValue(obj, null);

                if (property.Name == "Id")
                {
                    id = value.ToString();
                    continue;
                }

                result.Add(value);
            }

            result.Insert(0, id);

            return result;
        }
    }
}
