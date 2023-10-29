using CSharpGameSynchProg.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpGameSynchProg.Info
{
    public class SheetInfoBase
    {
        private string underscore = "_";
        public SheetInfoBase[] PreviousSheetInfo { get; protected set; }
        public string LocalStartPath { get; }

        public string PersistencyFolder { get; }

        public List<string> Names;

        public List<string> Ids;

        public Dictionary<Type, SheetData> dictionary;
        public string DataRange { get; protected set; }
        public string IdsDataRange { get; protected set; }
        public (string Name, string Id) BaseFolder { get; }
        public string Year { get; }
        public string LocalHost { get; }

        public string ServerHost { get; }


        public SheetData GetSheetData(Type type)
        {
            dictionary.TryGetValue(type, out SheetData sheetData);
            return sheetData;
        }

        public List<SheetData> GetAllSheetData()
        {
            var values = dictionary.Values.ToList();
            return values;
        }

        public SheetInfoBase((string, string) baseFolder, string year)
        {
            dictionary = new Dictionary<Type, SheetData>();
            //var backSlash = @"\";
            //var slash = "/";
            Year = year;
            BaseFolder = baseFolder;
            LocalHost = "http://localhost:8080";
            ServerHost = "notki.com.pl";

            //var assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            //var assemblyPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            //PathOfCredentialJson = assemblyPath + backSlash + "Credentials" + backSlash + "21-09-30_Notki-info_GameStatistics.json";

            //PathOfCredentialJson = assemblyPath + backSlash + "credentials3.json";

            //PrintPathOfCredectialJson();

            DataRange = "A4:F";

            AddToDictionary(typeof(Approaches));
            AddToDictionary(typeof(Rejections), typeof(Approaches));
            //AddToDictionary(typeof(DataDate));
            AddToDictionary(typeof(Dates));
            //AddToDictionary(typeof(Meetings));

            AddToDictionary(typeof(Records));
            AddToDictionary(typeof(Contacts));
            AddToDictionary(typeof(Messages));
            AddToDictionary(typeof(Active));
            AddToDictionary(typeof(WinderActive), typeof(Active));

            //AddToDictionary(typeof(DataMigrations));
            //AddToDictionary(typeof(DataActive));
            //AddToDictionary(typeof(DataClosed));
            //AddToDictionary(typeof(DataHistory));

            IdsDataRange = "A4:A";
            LocalStartPath = "D:/01_Synchronized/01_Programming_Files";
            PersistencyFolder = "9558c642-7766-46ae-bf6a-af380e4c68b9/Persistency";
        }

        public void AddToDictionary(Type type, Type main)
        {
            dictionary.Add(type, CreateSheet(type, main));
        }

        public void AddToDictionary(Type type)
        {
            dictionary.Add(type, CreateSheet(type));
        }

        private SheetData CreateSheet(Type type)
        {
            var properties = type.GetProperties();
            var columnNames = properties.Select(x => x.Name).ToList();
            var name = GetMainName(type.Name);
            var dataRange = GetDataRange(properties.Count());
            var copySheetName = GetCopyName(type.Name);
            var sheet = new SheetData(BaseFolder, Year, name, copySheetName, columnNames, dataRange);
            return sheet;
        }

        private SheetData CreateSheet(Type type, Type parentType)
        {
            var properties = type.GetProperties();
            var sheetTabName = GetMainName(type.Name);
            var columnNames = properties.Select(x => x.Name).ToList();
            var dataRange = GetDataRange(properties.Count());
            var fileName = Year + "_" + parentType.Name;
            var persistencyName = type.Name;
            var copySheetName = GetCopyName(type.Name);
            var sheet = new SheetData(BaseFolder, Year, sheetTabName, copySheetName, columnNames, dataRange, fileName, persistencyName);
            return sheet;
        }

        //private SheetData CreateTemp(string name)
        //{
        //    var id = "1DiI9lxIzwAlXyQ9_Bs3EvA10Qzk_M9RDnsYyHWZ7jLg";
        //    var columns = new List<string>() { "Date", "Type", "Count" };
        //    var dateRange = "2021_Rejections!A4:C";
        //    var sheet = new SheetData(
        //        BaseFolder,
        //        Year,
        //        GetMainName(name),
        //        GetCopyName(name),
        //        columns,
        //        dateRange);

        //    sheet.SetIds(id, null);

        //    return sheet;
        //}

        //private SheetData CreateTemp2(string name)
        //{
        //    var id = "1BtUalickQcTjjQ6zP4xAmeuf2cPWq7Ijm4Zp_M75N68";
        //    var columns = new List<string>() { "TookPlace", "ApproachId", "Type", "Number", "Sex", "Date" };
        //    var dateRange = "2022_Dates!A4:F";
        //    var sheet = new SheetData(
        //        BaseFolder,
        //        Year,
        //        GetMainName(name),
        //        GetCopyName(name),
        //        columns,
        //        dateRange);

        //    sheet.SetIds(id, null);

        //    return sheet;
        //}

        private SheetData CreateCopySheet(string name)
        {
            var sheet = new SheetData(BaseFolder, Year, GetMainName(name), GetCopyName(name), new List<string>(), DataRange);
            return sheet;
        }

        private SheetData CreateMainSheet(string name)
        {
            var columnNames = new List<string> { "Id", "Date", "Type", "Name", "Surname", "Attributes" };
            var sheet = new SheetData(BaseFolder, Year, GetMainName(name), null, columnNames, DataRange);
            return sheet;
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

        private string GetMainName(string name)
        {
            var fileName = Year + underscore + name;
            return fileName;
        }

        private string GetCopyName(string name)
        {
            var copy = "Copy";
            var fileName = copy + underscore + "Approaches";
            return fileName;
        }
    }
}
