using SharpCryptoCalcProg.ASheetObjects;
using System.Collections.Generic;

namespace SharpCryptoCalcProg.Info
{
    internal class SheetInfoGroup
    {
        private readonly SheetInfoCache cache;
        public Dictionary<Type, SheetInfo> dictionary;

        public SheetInfo Get(Type type)
        {
            dictionary.TryGetValue(type, out SheetInfo sheetData);
            return sheetData;
        }

        public SheetInfo Get<T>() where T : class
        {
            var type = typeof(T);
            dictionary.TryGetValue(type, out SheetInfo sheetData);
            return sheetData;
        }

        public List<SheetInfo> GetAllSheetData()
        {
            var values = dictionary.Values.ToList();
            return values;
        }

        public SheetInfoGroup(
            SheetInfoCache cache,
            Action<Type, string, string, string, string> registerMethod)
        {
            dictionary = new Dictionary<Type, SheetInfo>();
            this.cache = cache;
            Add<Transactions>();
            Add<Balances>();
            Add<Accounts>();
            Add<BinanceConvert>();
            Add<BinanceTransaction>();
            Add<BinanceWithdraw>();
            Register(registerMethod);
        }

        //public SheetInfoGroup(
        //    SheetInfoCache cache)
        //{
        //    dictionary = new Dictionary<Type, SheetInfo>();
        //    this.cache = cache;
        //    Add<Transactions>();
        //}

        public void Register(
            Action<Type, string, string, string, string> registerMethod)
        {
            foreach (var entry in dictionary)
            {
                var title = CreateTitle(entry.Value.Names);
                registerMethod.Invoke(
                    entry.Key,
                    entry.Value.FileName,
                    entry.Value.SpreadSheetId,
                    entry.Value.SheetId,
                    title);
            }
        }

        private string CreateTitle(string[] names)
        {
            var title = string.Join("; ", names);
            return title;
        }

        public void Add<T>() where T : class
        {
            var sheetInfo = cache.Get<T>();
            var type = typeof(T);
            dictionary.Add(type, sheetInfo);
        }
    }
}
