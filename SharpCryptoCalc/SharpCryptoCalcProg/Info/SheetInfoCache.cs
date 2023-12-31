﻿using SharpCryptoCalcProg.ASheetObjects;
using System.Reflection;

namespace SharpCryptoCalcProg.Info
{
    internal class SheetInfoCache
    {
        public SheetInfoCacheHelper helper;

        public SheetInfoCache()
        {
            helper = new SheetInfoCacheHelper();
        }

        public SheetInfo Get<T>()
        {
            var type = typeof(T);
            var formulas = GetFormulas<T>();
            var fileName = "crypto";
            var names = new string[] { fileName };
            if (type == typeof(Transactions))
            {
                var spreadSheetId = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
                var sheetId = "304249276";
                var sheet = CreateSheet(type, fileName, spreadSheetId, sheetId, names, formulas);
                return sheet;
            }
            if (type == typeof(Balances))
            {
                var spreadSheetId = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
                var sheetId = "1154582261";
                var sheet = CreateSheet(type, fileName, spreadSheetId, sheetId, names, formulas);
                return sheet;
            }
            if (type == typeof(Accounts))
            {
                var spreadSheetId = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
                var sheetId = "1077237614";
                var sheet = CreateSheet(type, fileName, spreadSheetId, sheetId, names, formulas);
                return sheet;
            }
            if (type == typeof(BinanceConvert))
            {
                var spreadSheetId = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
                var sheetId = "207106681";
                var sheet = CreateSheet(type, fileName, spreadSheetId, sheetId, names, formulas);
                return sheet;
            }
            if (type == typeof(BinanceTransaction))
            {
                var spreadSheetId = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
                var sheetId = "253108670";
                var sheet = CreateSheet(type, fileName, spreadSheetId, sheetId, names, formulas);
                return sheet;
            }
            if (type == typeof(BinanceWithdraw))
            {
                var spreadSheetId = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
                var sheetId = "529917431";
                var sheet = CreateSheet(type, fileName, spreadSheetId, sheetId, names, formulas);
                return sheet;
            }

            //var typeName = typeof(T).Name;
            //var (spreadSheetId, name2) = helper.GetFileByName(fileName);
            //var sheetId = helper.GetSheetId(spreadSheetId, typeName);

            return default;
        }

        private Dictionary<char, string> GetFormulas<T>()
        {
            var type = typeof(T);
            var method = type.GetMethod("GetFormulas");
            if (method == null)
            {
                return null;
            }
            var result = method.Invoke(null, null);
            return (Dictionary<char, string>)result;
        }

        private SheetInfo CreateSheet(
            Type type,
            string fileName,
            string spreadSheetId,
            string sheetId,
            string[] namesArray,
            Dictionary<char, string> formulas)
        {
            var sheet = new SheetInfo(
                type,
                fileName,
                spreadSheetId,
                sheetId,
                namesArray,
                formulas);
            return sheet;
        }
    }
}
