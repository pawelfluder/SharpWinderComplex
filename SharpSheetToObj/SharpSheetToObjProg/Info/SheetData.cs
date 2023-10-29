using System;
using System.Collections.Generic;

namespace CSharpGameSynchProg.Info
{
    public class SheetData
    {
        public string SpreadSheetId { get; private set; }
        public string SheetId { get; private set; }
        public string FileName { get; private set; }
        public string TypeName { get; private set; }
        public string Year { get; private set; }
        public string DataRange { get; private set; }
        public bool HasIdObjects { get; private set; }
        public List<string> PersistedIndexes { get; private set; }
        public string PersistencyName { get; private set; }
        public string SheetTabName { get; private set; }
        public string CopySheetTabName { get; private set; }
        public List<string> ColumnNames { get; private set; }
        public (string Name, string Id) MainFolder { get; private set; }

        public SheetData(
           (string Name, string Id) mainFolder,
           string year,
           string sheetTabName,
           string copySheetTabName,
           List<string> columnNames,
           string dataRange,
           string fileName,
           string persistencyName)
        {
            AssignParameters(
               mainFolder,
               year,
               sheetTabName,
               copySheetTabName,
               columnNames,
               dataRange,
               fileName,
               persistencyName);
        }

        public SheetData(
           (string Name, string Id) mainFolder,
           string year,
           string sheetTabName,
           string nameOfCopySheet,
           List<string> columnNames,
           string dataRange)
        {
            AssignParameters(
               mainFolder,
               year,
               sheetTabName,
               nameOfCopySheet,
               columnNames,
               dataRange,
               sheetTabName,
               sheetTabName);
        }

        public void AssignParameters(
           (string Name, string Id) mainFolder,
           string year,
           string sheetTabName,
           string nameOfCopySheet,
           List<string> columnNames,
           string dataRange,
           string fileName,
           string persistencyName)
        {
            MainFolder = mainFolder;
            Year = year;
            SheetTabName = sheetTabName;
            CopySheetTabName = nameOfCopySheet;
            ColumnNames = columnNames;
            DataRange = dataRange;
            var typeName = GetTypeFromYearAndName(sheetTabName);
            TypeName = typeName;
            FileName = fileName;
            PersistencyName = persistencyName;
            PersistedIndexes = new List<string>
         {
            year,
            typeName
         };
        }

        private string GetTypeFromYearAndName(string input)
        {
            var typeName = input.Replace(Year, "").Replace("_", "");
            return typeName;
        }

        public void SetIds(string spreadSheetId, string sheetId)
        {
            SpreadSheetId = spreadSheetId;
            SheetId = sheetId;
        }

        public object Where(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
