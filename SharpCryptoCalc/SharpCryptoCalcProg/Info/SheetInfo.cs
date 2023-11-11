namespace SharpCryptoCalcProg.Info
{
    public class SheetInfo
    {
        public Type Type { get; private set; }
        public string FileName { get; private set; }
        public string[] Names { get; private set; }
        public string SheetTabName { get; private set; }
        public List<string> ColumnNames { get; private set; }
        public string DataRange { get; private set; }
        public string SpreadSheetId { get; private set; }
        public string SheetId { get; private set; }
        public Dictionary<char, string> Formulas { get; private set; }

        public SheetInfo(
            Type type,
            string fileName,
            string spreadSheetId,
            string sheetId,
            string[] namesArray,
            Dictionary<char, string> formulas)
        {
            Type = type;
            FileName = fileName;
            SpreadSheetId = spreadSheetId;
            SheetId = sheetId;
            SheetTabName = type.Name;
            Names = namesArray;
            ColumnNames = GetPropertyNames(type);
            DataRange = GetDataRange(ColumnNames.Count());
            Formulas = formulas;
        }

        private List<string> GetPropertyNames(Type type)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            return propertyNames;
        }

        private string GetDataRange(int columnNumber)
        {
            var dataRange = "A4:";
            char c1 = 'A';
            Times(columnNumber - 1, () => { c1++; });
            dataRange += c1;

            return dataRange;
        }

        public void Times(int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
    }
}
