using System.Reflection;
using SharpGoogleDocsProg.AAPublic;
using CSharpGameSynchProg.Register;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpSetupProg21Private.AAPublic.Extensions;
using SharpSheetToObjProg.HasProperty;
using SharpSheetToObjProg.Merge;
using SharpSheetToObjProg.Info;
using GameSynchCoreProj;
using SharpSheetToObjProg.CorrectnessCheck;

namespace SharpSheetToObjProg.Service
{
    public class SychService
    {
        private IGoogleDocsService googleDocsService;
        private IGoogleDriveService googleDriveService;
        private IGoogleSheetService googleSheetService;
        private SheetInfoGroup sheetGroup;
        private MessagesWorker messagesWorker;
        private readonly ObjectPacker objectPacker;
        private readonly IFileService fileService;
        private readonly IRepoService repoService;

        public SychService()
        {
            sheetGroup = new SheetInfoGroup();
            //googleDocsService = MyBorder.Container.Resolve<IGoogleDocsService>();
            //googleDriveService = MyBorder.Container.Resolve<IGoogleDriveService>();
            googleSheetService = MyBorder.Container.Resolve<IGoogleSheetService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
            repoService = MyBorder.Container.Resolve<IRepoService>();
            messagesWorker = new MessagesWorker();
            objectPacker = new ObjectPacker();
        }

        //public void RegisterSheet<T>(
        //    string[] names)
        //{
        //    var type = typeof(T);
        //    var sheetInfo = new SheetInfo(type, names);
        //    sheetGroup.Add<T>(sheetInfo);
        //}

        public void RegisterSheet(
            Type type,
            string fileName,
            string spreadSheetId,
            string sheetId,
            string[] names,
            Dictionary<char, string> formulas)
        {
            var sheetInfo = new SheetInfo(
                type,
                fileName,
                spreadSheetId,
                sheetId,
                names,
                formulas);
            sheetGroup.Add(type, sheetInfo);
        }

        public IEnumerable<T1>
                SyncSheet<T1>()
            where T1 : class
        {
            var sheetInfo = sheetGroup.Get<T1>();
            if (HasId.HasProps<T1>(fileService))
            {
                var result = SynchObjects<T1, HasId>(sheetInfo.Names);
                return result;
            }

            if (HasName.HasProps<T1>(fileService))
            {
                var result = SynchObjects<T1, HasName>(sheetInfo.Names);
                return result;
            }

            return null;
        }

        private (IEnumerable<PkdObj<T1, T2>> mergedData, MergeInfo<T1, T2> changesInfo)
            MergeData<T1, T2>(
                IEnumerable<PkdObj<T1, T2>> sheetData,
                IEnumerable<PkdObj<T1, T2>> persistedData)
            where T1 : class
            where T2 : class
        {
            if (HasId.HasProps<T1>(fileService))
            {
                CheckDistinctIds(sheetData.Select(x => (IHasId)x.Target));
            }
            if (HasId.HasProps<T1>(fileService))
            {
                CheckDistinctIds(persistedData.Select(x => (IHasId)x.Target));
            }

            var mergeInfo = new MergeInfo<T1, T2>(persistedData, sheetData);
            var merge = GetMerge<T1, T2>(mergeInfo);

            return (merge, mergeInfo);
        }

        private IEnumerable<PkdObj<T1, T2>>
            GetMerge<T1, T2>(MergeInfo<T1, T2> mergeInfo)
            where T1 : class
            where T2 : class
        {
            var merge = mergeInfo.SheetMore
                .Concat(mergeInfo.SameTuple.Select(x => x.Item1));

            //if (mergeInfo.Counts.SheetMore == 0 &&
            //    mergeInfo.Counts.PersistedMore > 0)
            //{
            //    merge = merge.Concat(mergeInfo.PersitedMore);
            //}

            return merge;
        }

        private bool IsDataCorrupted<T1, T2>(
            IEnumerable<PkdObj<T1, T2>> sheetData,
            IEnumerable<PkdObj<T1, T2>> persistedData) where T1 : class
        {
            var genericType = typeof(T1);
            var gg = typeof(T1).GetInterfaces();
            var baseType = typeof(T1).BaseType.Name;

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

        private IEnumerable<T1> SynchObjects<T1, T2>(string[] names)
            where T1 : class
            where T2 : class, IGetKeyFunc
        {
            var sheetInfo = sheetGroup.Get(typeof(T1));
            var excelSheetData = GetExcelSheetData<T1>(sheetInfo);
            var persistedData = repoService.GetItemList<T1>(names);

            var pkdSheetData = objectPacker.Pack<T1, T2>(excelSheetData);
            var pkdPersistedData = objectPacker.Pack<T1, T2>(persistedData);
            var success1 = FirstAllDataChecks(pkdSheetData, pkdPersistedData);

            var (mergedData, mergeInfo) = MergeData(pkdSheetData, pkdPersistedData);

            if (!(mergeInfo.Counts.PersistedMore > 0 ||
                mergeInfo.Counts.SheetMore > 0 ||
                mergeInfo.Counts.Update > 0))
            {
                return persistedData;
            }

            var sortedMergedData = mergedData.OrderByDateId().ToList();
            var persitedDataToSave = sortedMergedData.Select(x => x.Source);
            var sheetDataToSave = ToIListOfIList(sortedMergedData);

            var success2 = LastAllDataChecks(sortedMergedData);

            var yamlResult = repoService.SaveItemList<T1>(persitedDataToSave, names);
            var headerNames = fileService.Reflection.GetPropNames<T1>();

            googleSheetService.Worker.PasteDataToSheet(
                sheetInfo.SpreadSheetId,
                sheetInfo.SheetTabName,
                sheetDataToSave,
                headerNames);

            return persitedDataToSave;
        }

        private bool
            LastAllDataChecks<T1, T2>(
                List<PkdObj<T1, T2>> mergedData)
            where T1 : class
            where T2 : class
        {
            var success = CheckDistinctIds(mergedData.Select(x => x.Target));
            return success;
        }

        public IList<IList<object>>
            ToIListOfIList<T1, T2>(
                IEnumerable<PkdObj<T1, T2>> inputList)
            where T1 : class
            where T2 : class
        {
            var result = inputList.Select(x => ToIList(x.Source)).ToList();
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
                result.Add(value);
            }

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

        public void CastAndAdd<T>(object objThatImplementsMyInterface, IList<T> theList)
        {
            theList.Add((T)objThatImplementsMyInterface);
        }

        public bool FirstAllDataChecks<T1, T2>(
            List<PkdObj<T1, T2>> pkdSheetData,
            IEnumerable<PkdObj<T1, T2>> pkdPersistedData)
            where T1 : class
            where T2 : class
        {
            if (pkdSheetData.Count() > 0 &&
                !AllPropertiesAreNotNull(pkdSheetData.First()))
            {
                throw new Exception();
            }

            if (pkdPersistedData.Count() > 0 &&
                !AllPropertiesAreNotNull(pkdPersistedData.First()))
            {
                throw new Exception();
            }

            if (IsDataCorrupted(pkdSheetData, pkdPersistedData))
            {
                return false;
            }

            if (HasId.HasProps<T1>(fileService))
            {
                var success3 = CheckDistinctIds(pkdSheetData
                    .Select(x => (IHasId)x.Target));
                if (!success3) return false;
            }
            
            var allPropertiesAreNotNull = AllPropertiesAreNotNull(pkdSheetData);

            return allPropertiesAreNotNull;
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
            var properties = GetType().GetProperties();

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

        public bool
            CheckDistinctIds<T>(
                IEnumerable<T> data)
        {
            if (HasId.HasProps<T>(fileService))
            {
                var distinctList = data.EDistinctBy(x => ((IHasId)x).Id).ToList();
                var result = data.Count() == distinctList.Count();
                var duplicates = data.GroupBy(x => ((IHasId)x).Id)
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
            var tmp2 = fileService.Yaml.Custom03.Deserialize<Dictionary<object, object>>(gg);
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

        //private IEnumerable<(bool, PkdObj<T1, T2>)>
        //    GetMerge<T1, T2>(
        //        IEnumerable<PkdObj<T1, T2>> persistedData,
        //        IEnumerable<PkdObj<T1, T2>> excelSheetData) where T1 : class
        //{
        //    var result = new List<(bool, PkdObj<T1, T2>)>();

        //    var existedData = excelSheetData.Where(x => persistedData.Any(y => y.Target.Id == x.Target.Id));
        //    var persistedIds = persistedData.Select(x => x.Target.Id).OrderBy(x => x);
        //    var excelIds = excelSheetData.Select(x => x.Target.Id).OrderBy(x => x);

        //    var persistedIdsDuplicated = persistedIds.Where(x => persistedIds.Count(y => y == x) > 1);
        //    var excelIdsDuplicated = excelIds.Where(x => excelIds.Count(y => y == x) > 1);

        //    foreach (var sheetItem in existedData)
        //    {
        //        PkdObj<T1, T2> persistedItem = null;
        //        try
        //        {
        //            persistedItem = persistedData.Single(x => x.Target.Id == sheetItem.Target.Id);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.Message == "Sequence contains more than one matching element")
        //            {
        //                Console.WriteLine("Dwa lub więcej elementów z tym samym id");
        //            }
        //            //throw ex;
        //        }

        //        if (persistedItem == null ||
        //            !persistedItem.Equals(sheetItem))
        //        {
        //            result.Add((true, sheetItem));
        //        }
        //        else
        //        {
        //            result.Add((false, sheetItem));
        //        }
        //    }

        //    return result;
        //}

        private List<T> GetExcelSheetData<T>(SheetInfo sheetData) where T : class
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
