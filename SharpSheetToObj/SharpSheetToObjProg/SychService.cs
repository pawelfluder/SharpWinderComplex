﻿using CommonTypesCoreProj.Contracts;
using System.Reflection;
using SharpYaml.Serialization;
using CSharpGameSynchProg.Contracts;
using CSharpGameSynchProg.Extensions;
using CSharpGameSynchProg.Info;
using SharpGoogleDocsProg.AAPublic;
using CSharpGameSynchProg.Register;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpSetupProg21Private.AAPublic.Extensions;
using SharpSheetToObjProg.Objects;
using SharpSheetToObjProg;
using SharpSheetToObjProg.HasProperty;

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
        private readonly SheetCache sheetCache;
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
            sheetCache = new SheetCache();
            SetSheetsIdsForInfo();
            messagesWorker = new MessagesWorker();
            
        }

        private void SetSheetsIdsForInfo()
        {
            var yearFolder = googleDriveService.Worker
                .GetFolderByNameAndId(info.BaseFolder.Name, info.BaseFolder.Id);

            var sheetFiles = sheetCache.GetCache();

            //var sheetFiles = googleDriveService.Worker
            //    .GetFilesRequest($"'{yearFolder.Id}' in parents and mimeType='application/vnd.google-apps.spreadsheet'");
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

        public void SyncSheet<T>(params string[] names) where T : class
        {
            var hasId = true;
            if (hasId)
            {
                SynchIdDataObjects<T>(names);
                return;
            }
        }

        private void PrintInfo(string from, string to)
        {
            Console.WriteLine("From " + from + " To " + to);
        }

        private void SynchForCommonObjects<T>() where T : CommonObject
        {
            var sheetData = info.GetSheetData(typeof(T));
            PrintInfo(sheetData.FileName + " " + sheetData.CopySheetTabName, sheetData.PersistencyName);
            var excelSheetData = GetExcelSheetData<T>(sheetData).ToList();

            //if (typeof(IHasDate).IsAssignableFrom(typeof(T)))
            //{
            //    excelSheetData = excelSheetData.OrderByDateProperty();
            //}

            var mainAdrTuple = ("Persistency", "03");
            var year = "2022";

            //var approachesAdrTuple = repoService
            //    .GetItemList<Approaches>(year);

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

        private (IEnumerable<PkdObj<T, HasIdDate>> mergedData, JoinDataCount changesInfo) MergeData<T>(
            IEnumerable<PkdObj<T, HasIdDate>> sheetData,
            IEnumerable<PkdObj<T, HasIdDate>> persistedData) where T : class
        {
            CheckDistinctIds(sheetData.Select(x => (IHasId)x.Target));
            CheckDistinctIds(persistedData.Select(x => (IHasId)x.Target));

            var sheetMore = sheetData
                .Where(x1 => !(persistedData.Select(x2 => x2.Target)
                .Any(y => y.Id != x1.Target.Id)));
            var sheetMoreIds = sheetMore.Select(x => x.Target.Id).ToList();

            var persitedMore = persistedData
                .Where(x1 => !(sheetData.Select(x2 => x2.Target)
                .Any(y => y.Id != x1.Target.Id)));
            var persitedMoreIds = persitedMore.Select(x => x.Target.Id).ToList();

            var sameTuple2 = CompareLists<PkdObj<T, HasIdDate>, string> (
                sheetData,
                persistedData,
                x => x.Target.Id);

            var sameTuple3 = CompareLists3<T>(
                sheetData,
                persistedData);

            var sameIds = sameTuple3.Select(x => x.Item1.Target.Id).ToList();

            var updateTuple = FindDiffrentProperties<T>(sameTuple3);
            var updateIds = updateTuple.Select(x => x.Item1.Target.Id).ToList();

            var removedData = persistedData
                .Where(x1 => !(sheetData.Select(x2 => x2.Target)
                .Any(y => y.Id == x1.Target.Id)));

            var newData = sheetData
                .Where(x => !(persistedData.Select(x2 => x2.Target)
                .Any(y => y.Id == x.Target.Id)));

            var merge = GetMerge(persistedData, sheetData);

            var changed = merge.Where(x => x.Item1 == true).ToList();
            var existedSheetData = merge.Select(x => x.Item2);
            var allDataToSave = removedData.Concat(existedSheetData).Concat(newData)
                .OrderByDescending(x => x.Target.Date).ToList();
            var orderedDataToSave = allDataToSave.OrderByDateProperty();

            var dataCount = new JoinDataCount(
               persistedData.Count(),
               sheetData.Count(),
               removedData.Count(),
               newData.Count(),
               merge.Count(x => x.Item1 == false),
               merge.Count(x => x.Item1 == true),
               existedSheetData.Count(),
               allDataToSave.Count());

            return (allDataToSave, dataCount);
        }

        

        public IEnumerable<(T, T)> CompareLists<T, TKey>(IEnumerable<T> list1, IEnumerable<T> list2, Func<T, TKey> keySelector)
        {
            var commonElements = list1
                .Join(list2, keySelector, keySelector, (item1, item2) => (item1, item2))
                .ToList();

            return commonElements;
        }

        public IEnumerable<(PkdObj<T, HasIdDate>, PkdObj<T, HasIdDate>)>
            CompareLists3<T>(
            IEnumerable<PkdObj<T, HasIdDate>> list1,
            IEnumerable<PkdObj<T, HasIdDate>> list2)
        {
            var result = new List<(PkdObj<T, HasIdDate>, PkdObj<T, HasIdDate>)>();
            var count = list1.Count();

            for (int i = 0; i < count; i++)
            {
                var item1 = list1.ElementAt(i);
                var item2 = list2.ElementAt(i);

                if (item1.Target.Id == item2.Target.Id)
                {
                    result.Add((item1, item2));
                }
            }

            return result;
        }

        public List<Tuple<T, T>> CompareLists2<T, TKey>(List<T> list1, List<T> list2, Func<T, TKey> keySelector)
        {
            var commonElements = list1
                .Join(list2, keySelector, keySelector, (item1, item2) => Tuple.Create(item1, item2))
                .ToList();

            return commonElements;
        }

        private IEnumerable<(PkdObj<T, HasIdDate>, PkdObj<T, HasIdDate>)>
            FindDiffrentProperties<T>(
            IEnumerable<(PkdObj<T, HasIdDate>, PkdObj<T, HasIdDate>)> sameTuple)
            where T : class
        {
            var result = new List<(PkdObj<T, HasIdDate>, PkdObj<T, HasIdDate>)>();

            foreach (var item in sameTuple)
            {
                if (!Equals(item.Item1.Source, item.Item2.Source))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public bool Equals(object obj01, object obj02)
        {
            var thisTuples = GetTuples(obj02);
            var objTuples = GetTuples(obj01);

            if (thisTuples.Length != objTuples.Length)
            {
                return false;
            }

            for (int i = 0; i < thisTuples.Length; i++)
            {
                var tuple01 = thisTuples[i];
                var tuple02 = objTuples[i];
                if (tuple01 != tuple02)
                {
                    return false;
                }
            }

            return true;
        }

        public (string, string)[] GetTuples(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var tuples = properties.Select(x => (x.Name, x.GetValue(obj).ToString())).ToArray();
            return tuples;
        }

        private bool IsDataCorrupted<T>(
            IEnumerable<PkdObj<T, HasIdDate>> sheetData,
            IEnumerable<PkdObj<T, HasIdDate>> persistedData) where T: class
        {
            var genericType = typeof(T);
            var gg = typeof(T).GetInterfaces();
            var baseType = typeof(T).BaseType.Name;

            //var type2 = typeof(CommonIdObject).Name;
            //var type1 = typeof(CommonObject).Name;

            //if (baseType == type2)
            //{
            //    var temp1 = sheetData.Select(x => (CommonObject)x).ToList();
            //    var temp2 = persistedData.Select(x => (CommonObject)x).ToList();

            //    if (CommonObject.IsDataListCorrupted(temp1))
            //    {
            //        return true;
            //    }

            //    if (CommonObject.IsDataListCorrupted(temp2))
            //    {
            //        return true;
            //    }

            //    return false;
            //}

            //if (baseType == type1)
            //{
            //    var temp1 = sheetData.Select(x => (CommonObject)x).ToList();
            //    var temp2 = persistedData.Select(x => (CommonObject)x).ToList();
            //    if (CommonObject.IsDataListCorrupted(temp1))
            //    {
            //        return true;
            //    }
            //    if (CommonObject.IsDataListCorrupted(temp2))
            //    {
            //        return true;
            //    }

            //    return false;
            //}

            //throw new Exception();

            return false;
        }

        private void SynchIdDataObjects<T>(params string[] names) where T : class
        {
            var year = names[0]; // "2022";
            var sheetData = info.GetSheetData(typeof(T));
            PrintInfo("Sheet; " + sheetData.FileName + "; " + sheetData.CopySheetTabName, "File; " + sheetData.PersistencyName);
            List<T> excelSheetData = GetExcelSheetData<T>(sheetData);

            var objectPacker = new ObjectPacker();
            var pkdSheetData = objectPacker.Pack<T, HasIdDate>(excelSheetData);

            var gg = AllDataChecks(pkdSheetData);
            if (!AllPropertiesAreNotNull(pkdSheetData.First())) { pkdSheetData = pkdSheetData.Skip(1).ToList(); }

            var pkdOrderedSheetData = pkdSheetData.OrderByDateProperty().ToList();
            
            var persistedData = repoService.GetItemList<T>(year);
            var pkdPersistedData = objectPacker.Pack<T, HasIdDate>(persistedData);

            if (IsDataCorrupted(pkdOrderedSheetData, pkdPersistedData))
            {
                return;
            }

            var (mergedData, mergeInfo) = MergeData(pkdOrderedSheetData, pkdPersistedData);

            if (mergeInfo.CountOfRemovedData > 0 ||
                mergeInfo.CountOfNewData > 0 ||
                mergeInfo.CountOfChangedData > 0 &&
                mergeInfo.CountOfChangedData <= 1)
            {
                
                CheckDistinctIds(mergedData.Select(x => x.Target));
                var yamlResult = repoService.SaveItemList<T>(mergedData.Select(x => x.Source), year);
                var dataToSave = ToIListQIList(mergedData);

                var headerNames = GetPropertyNames(typeof(T));
                var sheetToUpdate = info.GetSheetData(typeof(T));
                //var formulas = GetFormulas(typeof(T));

                //googleSheetService.Worker.PasteDataToSheet(
                //sheetToUpdate.SpreadSheetId,
                //sheetToUpdate.SheetTabName,
                //dataToSave,
                //headerNames);
            }
        }

        public IList<IList<object>> ToIListQIList<T>(IEnumerable<PkdObj<T, HasIdDate>> inputList) where T : class
        {
            var result = inputList.Select(x => ToIList(x.Source)).ToList();
            return result;
        }

        //public  IList<IList<object>> ToIListQIList<T>(IList<T> inputList) where T: class
        //{
        //    var result = inputList.Select(x => ToIList(x)).ToList();
        //    return result;
        //}

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

        public IList<T> CastList<T>(IEnumerable<object> theList)
        {
            var result = new List<T>();
            foreach (var item in theList)
            {
                CastAndAdd(item, result);
            }
            return result;
        }

        //public List<T> CastAndAdd2<T>(
        //    object objThatImplementsMyInterface,
        //    IList<T> theList)
        //{
        //    typeof(T1).
        //    var tmp1 = (T1)objThatImplementsMyInterface;
        //    var tmp2 = (T2)tmp1;
        //    theList.Add();
        //}

        public void CastAndAdd<T>(object objThatImplementsMyInterface, IList<T> theList)
        {
            theList.Add((T)objThatImplementsMyInterface);
        }

        public bool AllDataChecks<T>(IEnumerable<PkdObj<T, HasIdDate>> objList) where T : class
        {
            var distictIds = CheckDistinctIds(objList.Select(x => (IHasId)x.Target));
            var allPropertiesAreNotNull = AllPropertiesAreNotNull(objList);

            var dataNotCorrupted = distictIds & allPropertiesAreNotNull;
            return dataNotCorrupted;
        }

        public bool AllPropertiesAreNotNull_InFirstData(ref IEnumerable<IHasId> objList)
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

        public bool CheckDistinctIds(IEnumerable<IHasId> data)
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

        private IEnumerable<(bool, PkdObj<T, HasIdDate>)> GetMerge<T>(
            IEnumerable<PkdObj<T, HasIdDate>> persistedData,
            IEnumerable<PkdObj<T, HasIdDate>> excelSheetData) where T : class
        {
            var result = new List<(bool, PkdObj<T, HasIdDate>)>();

            var existedData = excelSheetData.Where(x => persistedData.Any(y => y.Target.Id == x.Target.Id));
            var persistedIds = persistedData.Select(x => x.Target.Id).OrderBy(x => x);
            var excelIds = excelSheetData.Select(x => x.Target.Id).OrderBy(x => x);

            var persistedIdsDuplicated = persistedIds.Where(x => persistedIds.Count(y => y == x) > 1);
            var excelIdsDuplicated = excelIds.Where(x => excelIds.Count(y => y == x) > 1);

            foreach (var sheetItem in existedData)
            {
                PkdObj<T, HasIdDate> persistedItem = null;
                try
                {
                    persistedItem = persistedData.Single(x => x.Target.Id == sheetItem.Target.Id);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Sequence contains more than one matching element")
                    {
                        Console.WriteLine("Dwa lub więcej elementów z tym samym id");
                    }
                    //throw ex;
                }

                if (persistedItem == null ||
                    !persistedItem.Equals(sheetItem))
                {
                    result.Add((true, sheetItem));
                }
                else
                {
                    result.Add((false, sheetItem));
                }
            }

            return result;
        }

        private List<T> GetExcelSheetData<T>(SheetData sheetData) where T : class
        {
            var sheetListOfList = googleSheetService.Worker
                .GetSheetData(sheetData.SpreadSheetId, sheetData.SheetId, sheetData.DataRange);
            var sheetListOfDictionaries = googleSheetService
                .Worker.ConvertToListOfDictionaries(sheetListOfList, sheetData.ColumnNames);
            var yamlText = fileService.Yaml.Custom03.Serialize(sheetListOfDictionaries);
            var result = fileService.Yaml.Custom03.Deserialize<List<T>>(yamlText);

            return result;
        }

        private List<string> GetPropertyNames(Type type)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            return propertyNames;
        }
    }
}
