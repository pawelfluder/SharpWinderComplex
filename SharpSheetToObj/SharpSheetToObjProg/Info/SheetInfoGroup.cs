namespace SharpSheetToObjProg.Info
{
    internal class SheetInfoGroup
    {
        public Dictionary<string, SheetInfo> dictionary;

        public SheetInfo Get(Type type)
        {
            dictionary.TryGetValue(type.Name, out SheetInfo sheetData);
            return sheetData;
        }

        public SheetInfo Get<T>() where T : class
        {
            var type = typeof(T);
            dictionary.TryGetValue(type.Name, out SheetInfo sheetData);
            return sheetData;
        }

        public List<SheetInfo> GetAllSheetData()
        {
            var values = dictionary.Values.ToList();
            return values;
        }

        public SheetInfoGroup()
        {
            dictionary = new Dictionary<string, SheetInfo>();
        }

        public void Add<T>(SheetInfo sheetInfo)
        {
            var type = typeof(T);
            dictionary.Add(type.Name, sheetInfo);
        }

        public void Add(Type type, SheetInfo sheetInfo)
        {
            dictionary.Add(type.Name, sheetInfo);
        }

        //private SheetInfo CreateSheet(
        //    Type type,
        //    string fileName,
        //    string[] names)
        //{
        //    var names2 = names.Append(fileName).ToArray();
        //    var sheet = new SheetInfo(
        //        type,
        //        names2);
        //    return sheet;
        //}
    }
}
