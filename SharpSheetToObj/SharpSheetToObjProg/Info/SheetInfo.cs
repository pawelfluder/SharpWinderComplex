namespace SharpSheetToObjProg.Info
{
    public class SheetInfo
    {
        public Type Type { get; private set; }
        public string FileName { get; private set; }
        public string SpreadSheetId { get; private set; }
        public string SheetId { get; private set; }
        public string SheetTabName { get; private set; }
        public List<string> ColumnNames { get; private set; }
        public string DataRange { get; private set; }
        public string Title { get; private set; }

        public SheetInfo(
            Type type,
            string fileName,
            string spreadSheetId,
            string sheetId,
            string title,
            Dictionary<char, string> formulas)
        {
            Type = type;
            FileName = fileName;
            SpreadSheetId = spreadSheetId;
            SheetId = sheetId;
            SheetTabName = type.Name;
            ColumnNames = GetPropertyNames(type);
            DataRange = GetDataRange(ColumnNames.Count());
        }

        private List<string> GetPropertyNames(Type type)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            return propertyNames;
        }

        public void SetIds(string spreadSheetId, string sheetId)
        {
            SpreadSheetId = spreadSheetId;
            SheetId = sheetId;
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
