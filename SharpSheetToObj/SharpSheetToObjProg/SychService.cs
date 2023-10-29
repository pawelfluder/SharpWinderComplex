using CommonTypesCoreProj.Contracts;
using System.Reflection;
using SharpYaml.Serialization;
using CSharpGameSynchProg.Contracts;
using CSharpGameSynchProg.Extensions;
using CSharpGameSynchProg.Interfaces;
using CSharpGameSynchProg.Objects;
using CSharpGameSynchProg.Info;
using SharpGoogleDocsProg.AAPublic;
using CSharpGameSynchProg.Register;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpSetupProg21Private.AAPublic.Extensions;

namespace GameSynchCoreProj
{
    public class SychService
    {
        private IGoogleDocsService googleDocsService;
        private IGoogleDriveService googleDriveService;
        private IGoogleSheetService googleSheetService;
        private SheetInfoBase info;
        //private IPersistencyService persistency;
        private MessagesWorker messagesWorker;
        private static Serializer yamlSerializerSharp = new Serializer();
        private readonly IFileService fileService;
        private readonly IRepoService repoService;

        public SychService(SheetInfoBase info)
        {
            this.info = info;
            googleDocsService = MyBorder.Container.Resolve<IGoogleDocsService>();
            googleDriveService = MyBorder.Container.Resolve<IGoogleDriveService>();
            googleSheetService = MyBorder.Container.Resolve<IGoogleSheetService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
            repoService = MyBorder.Container.Resolve<IRepoService>();

            var startPath = info.LocalStartPath;
            var searchPlace = info.PersistencyFolder;
            //persistency = new PersistencyService(startPath, searchPlace);
            SetSheetsIdsForInfo();
            messagesWorker = new MessagesWorker();
        }

        private void SetSheetsIdsForInfo()
        {
            var yearFolder = googleDriveService.Worker
                .GetFolderByNameAndId(info.BaseFolder.Name, info.BaseFolder.Id);
            var sheetFiles = googleDriveService.Worker
                .GetFilesRequest($"'{yearFolder.Id}' in parents and mimeType='application/vnd.google-apps.spreadsheet'");
            var names = sheetFiles.Select(x => x.Name);

            var sheetInfos = info.GetAllSheetData();

            foreach (var sheetInfo in sheetInfos)
            {
                try
                {
                    var spreadSheetId = sheetFiles.Single(x => x.Name == sheetInfo.FileName).Id;
                    var spreadSheet = googleSheetService.Worker.GetSpreadsheet(spreadSheetId);
                    var sheet = spreadSheet.Sheets.SingleOrDefault(x => x.Properties.Title == sheetInfo.SheetTabName);
                    var sheetId = sheet.Properties.SheetId.ToString();
                    sheetInfo.SetIds(spreadSheetId, sheetId);
                }
                catch { }
            }
        }

        public void SyncSheets(string option)
        {
            if (option != Options.CopyOnly)
            {
                SynchForCommonObjects<Dates>();
                SynchForCommonObjects<Rejections>();
                SynchForCommonObjects<Contacts>();
                SynchForCommonObjects<Meetings>();
                SynchForCommonObjects<Active>();
                SynchForCommonObjects<WinderActive>();
            }

            SynchForCommonIdObjects<Approaches>();
            NamedApproachesToOtherSheets<Approaches>(info);

            //var messages = GetAllMessages();
            //UpdateMessages(messages);
        }

        private void PrintInfo(string from, string to)
        {
            Console.WriteLine("From " + from + " To " + to);
        }

        private void UpdateMessages(IEnumerable<Messages> messages)
        {
            //string yamlResult = persistency.Serialize(messages);
            var sheetData = info.GetSheetData(typeof(Messages));
            //.SaveData(sheetData.PersistedIndexes, messages);

            var dataToSaveToSheet = CommonObject.ToIList(messages.ToList());

            List<string> headerNames = GetPropertyNames(typeof(Messages));
            //googleSheetService.PasteDataAndFunctionsToSheet(sheetData.SpreadSheetId, sheetData.SheetId, dataToSaveToSheet, sheetData.ColumnNames, new List<(string, string)>());
        }

        private void SynchForCommonObjects<T>() where T : CommonObject
        {
            var sheetData = info.GetSheetData(typeof(T));
            PrintInfo(sheetData.FileName + " " + sheetData.CopySheetTabName, sheetData.PersistencyName);
            var excelSheetData = GetExcelSheetData<T>(sheetData).ToList();

            if (typeof(IHasDataProp).IsAssignableFrom(typeof(T)))
            {
                excelSheetData = excelSheetData.OrderByDateProperty();
            }

            var mainAdrTuple = ("Persistency", "03");
            var year = "2022";

            var approachesAdrTuple = repoService
                .GetItemList<Approaches>(year);

            //var persistedData = persistency.GetData<List<T>>(sheetData.PersistedIndexes);

            //if (IsDataCorrupted(excelSheetData, persistedData))
            //{
            //    return;
            //}

            if (true)
            {
                //string yamlResult = persistency.Serialize(excelSheetData);
                //persistency.SaveData(sheetData.PersistedIndexes, excelSheetData);
            }
        }

        private (List<T> mergedData, JoinDataCount changesInfo) JoinData<T>(
            List<T> excelSheetData,
            List<T> persistedData)
            where T : CommonIdObject, IHasIdProp
        {
            CheckDistinctIds(excelSheetData.Select(x => (IHasIdProp)x));
            CheckDistinctIds(persistedData.Select(x => (IHasIdProp)x));

            var removedData = persistedData.Where(x => !excelSheetData.Any(y => y.Id == x.Id));
            
            var newData = excelSheetData.Where(x => !persistedData.Any(y => y.Id == x.Id));

            var changes = GetChanges(persistedData, excelSheetData);

            var changed = changes.Where(x => x.Item1 == true).ToList();
            var existedSheetData = changes.Select(x => x.Item2);
            var allDataToSave = new List<T>(removedData.Concat(existedSheetData).Concat(newData));//.OrderByDescending(x => x.Date).ToList();
            var orderedDataToSave = allDataToSave.OrderByDateProperty();

            var dataCount = new JoinDataCount(
               persistedData.Count(),
               excelSheetData.Count(),
               removedData.Count(),
               newData.Count(),
               changes.Count(x => x.Item1 == false),
               changes.Count(x => x.Item1 == true),
               existedSheetData.Count(),
               orderedDataToSave.Count());

            return (orderedDataToSave, dataCount);
        }

        private bool IsDataCorrupted<T>(List<T> excelSheetData, List<T> persistedData) where T : CommonObject
        {
            var genericType = typeof(T);
            var gg = typeof(T).GetInterfaces();
            var baseType = typeof(T).BaseType.Name;

            var type2 = typeof(CommonIdObject).Name;
            var type1 = typeof(CommonObject).Name;

            if (baseType == type2)
            {
                var temp1 = excelSheetData.Select(x => (CommonObject)x).ToList();
                var temp2 = persistedData.Select(x => (CommonObject)x).ToList();

                if (CommonObject.IsDataListCorrupted(temp1))
                {
                    return true;
                }

                if (CommonObject.IsDataListCorrupted(temp2))
                {
                    return true;
                }

                return false;
            }

            if (baseType == type1)
            {
                var temp1 = excelSheetData.Select(x => (CommonObject)x).ToList();
                var temp2 = persistedData.Select(x => (CommonObject)x).ToList();
                if (CommonObject.IsDataListCorrupted(temp1))
                {
                    return true;
                }
                if (CommonObject.IsDataListCorrupted(temp2))
                {
                    return true;
                }

                return false;
            }

            throw new Exception();
        }

        private void SynchForCommonIdObjects<T>() where T : CommonIdObject, IHasIdProp
        {
            var sheetData = info.GetSheetData(typeof(T));
            PrintInfo("Sheet; " + sheetData.FileName + "; " + sheetData.CopySheetTabName, "File; " + sheetData.PersistencyName);
            List<T> excelSheetData = GetExcelSheetData<T>(sheetData);
            var gg = ListDataNotCorrupted(excelSheetData);
            if (!AllPropertiesAreNotNull(excelSheetData.First())) { excelSheetData = excelSheetData.Skip(1).ToList(); }

            var orderedExcelSheetData = excelSheetData.OrderByDateProperty().ToList();
            var year = "2022";
            var persistedData = repoService.GetItemList<T>(year);


            if (IsDataCorrupted(excelSheetData, persistedData))
            {
                return;
            }

            var (mergedData, changesInfo) = JoinData(excelSheetData, persistedData);

            if (changesInfo.CountOfRemovedData > 0 ||
                changesInfo.CountOfNewData > 0 ||
                changesInfo.CountOfChangedData > 0 &&
                changesInfo.CountOfChangedData <= 1)
            {
                //string yamlResult = persistency.Serialize(mergedData);
                CheckDistinctIds(mergedData.Select(x => (IHasIdProp)x));
                //persistency.SaveData(sheetData.PersistedIndexes, mergedData);
                var dataToSave = CommonIdObject.ToIList(mergedData.ConvertAll(x => (CommonIdObject)x));

                var headerNames = GetPropertyNames(typeof(T));
                var sheetToUpdate = info.GetSheetData(typeof(T));
                //var formulas = GetFormulas(typeof(T));

                //googleSheetService.Worker.PasteDataAndFunctionsToSheet(
                //sheetToUpdate.SpreadSheetId,
                //sheetToUpdate.SheetId,
                //dataToSave,
                //headerNames,
                //formulas);
            }
        }

        public bool ListDataNotCorrupted(IEnumerable<IHasIdProp> objList)
        {
            var distictIds = CheckDistinctIds(objList);
            var allPropertiesAreNotNull = AllPropertiesAreNotNull(objList);

            var dataNotCorrupted = distictIds & allPropertiesAreNotNull;
            return dataNotCorrupted;
        }

        public bool AllPropertiesAreNotNull_InFirstData(ref IEnumerable<IHasIdProp> objList)
        {
            var first = objList.First();
            if (!AllPropertiesAreNotNull(first))
            {
                var modified = objList.Skip(1);
                objList = modified;
                return false;
            }

            return true;
        }

        public bool AllPropertiesAreNotNull(IEnumerable<object> objList)
        {
            var properties = this.GetType().GetProperties();

            foreach (var obj in objList)
            {
                var notNull = AllPropertiesAreNotNull(obj);
                if (!notNull)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AllPropertiesAreNotNull(object obj)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckDistinctIds(IEnumerable<IHasIdProp> data)
        {
            bool result = false;
            var distinctList = data.DistinctBy(x => x.Id).ToList();
            result = (data.Count() == distinctList.Count());

            var duplicates = data.GroupBy(x => x.Id)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            if (duplicates.Any())
            {
                Console.WriteLine("Error - duplicated ids in excel sheet:");
                foreach (var dup in duplicates)
                {
                    Console.WriteLine(dup);
                }

                //throw new Exception();
                return false;
            }

            return true;
        }

        public List<(string, string)> GetFormulas(Type type)
        {
            var typeName = type.Name;
            var embededFileName = "Formulas" + "." + typeName + "." + "yaml";
            var nameSpaceName = GetType().Namespace;
            var assembly = GetType().Assembly;
            var gg = GetEmbeddedResource(nameSpaceName, embededFileName, assembly);
            var tmp2 = yamlSerializerSharp.Deserialize<Dictionary<object, object>>(gg);
            var result = tmp2.Select(x => (x.Key.ToString(), x.Value.ToString())).ToList();
            return result;
        }

        private string GetEmbeddedResource(string namespacename, string filename, Assembly assembly)
        {
            var resourceName = namespacename + "." + filename;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }

            return string.Empty;
        }

        private List<(bool, T)> GetChanges<T>(List<T> persistedData, List<T> excelSheetData) where T : CommonIdObject
        {
            var result = new List<(bool, T)>();

            var existedData = excelSheetData.Where(x => persistedData.Any(y => y.Id == x.Id));
            var persistedIds = persistedData.Select(x => x.Id).OrderBy(x => x);
            var excelIds = excelSheetData.Select(x => x.Id).OrderBy(x => x);

            var persistedIdsDuplicated = persistedIds.Where(x => persistedIds.Count(y => y == x) > 1);
            var excelIdsDuplicated = excelIds.Where(x => excelIds.Count(y => y == x) > 1);

            foreach (var item in existedData)
            {
                T persisted = null;
                try
                {
                    persisted = persistedData.Single(x => x.Id == item.Id);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Sequence contains more than one matching element")
                    {
                        Console.WriteLine("Dwa lub więcej elementów z tym samym id");
                    }
                    //throw ex;
                }

                if (persisted.Equals(item))
                {
                    result.Add((false, item));
                }
                else
                {
                    result.Add((true, item));
                }
            }

            return result;
        }

        private List<T> GetExcelSheetData<T>(SheetData sheetData) where T : CommonObject
        {
            var sheetListOfList = googleSheetService.Worker
                .GetSheetData(sheetData.SpreadSheetId, sheetData.SheetId, sheetData.DataRange);
            var sheetListOfDictionaries = googleSheetService
                .Worker.ConvertToListOfDictionaries(sheetListOfList, sheetData.ColumnNames);
            var yamlText = fileService.Yaml.Custom03.Serialize(sheetListOfDictionaries);
            var result = fileService.Yaml.Custom03.Deserialize<List<T>>(yamlText);

            return result;
        }

        private void NamedApproachesToOtherSheets<T>(SheetInfoBase info) where T : CommonIdObject
        {
            var sheetData = info.GetSheetData(typeof(Approaches));
            //var persistedData = persistency.GetData<List<T>>(sheetData.PersistedIndexes).ToList();

            if (info.PreviousSheetInfo.Count() > 0)
            {
                var temp = new List<string>() { info.PreviousSheetInfo.First().Year, "Approaches" };
                //var persistedData2 = persistency.GetData<List<T>>(temp);
                //persistedData.AddRange(persistedData2);
            }

            //var orderedData = persistedData.OrderByDateProperty();
            //var result = CommonIdObject.ToIList(orderedData.ConvertAll(x => (CommonIdObject)x));
            var sheetsToUpdate = info.GetAllSheetData();

            var propertyNames = GetPropertyNames(typeof(T));
            var formulas = GetFormulas(typeof(T));

            foreach (var sheetToUpdate in sheetsToUpdate)
            {
                PrintInfo(sheetData.FileName + " " + sheetData.CopySheetTabName, sheetToUpdate.FileName + " " + sheetToUpdate.CopySheetTabName);
                //var sheetId = googleSheetService.GetSheetIdByTabName(sheetToUpdate.SpreadSheetId, sheetToUpdate.CopySheetTabName).ToString();
                //googleSheetService.PasteDataAndFunctionsToSheet(
                //   sheetToUpdate.SpreadSheetId,
                //   sheetId,
                //   result,
                //   propertyNames,
                //   formulas);
            }
        }

        private List<string> GetPropertyNames(Type type)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            return propertyNames;
        }
    }
}
