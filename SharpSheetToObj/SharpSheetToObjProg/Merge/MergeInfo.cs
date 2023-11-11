using CSharpGameSynchProg.Register;
using SharpFileServiceProg.Service;
using SharpSheetToObjProg.CorrectnessCheck;

namespace SharpSheetToObjProg.Merge
{
    internal class MergeInfo<T1, T2>
        where T1 : class
        where T2 : class
    {
        private readonly IFileService fileService;

        public IEnumerable<PkdObj<T1, T2>> SheetMore { get; private set; }
        public IEnumerable<PkdObj<T1, T2>> PersitedMore { get; private set; }
        public IEnumerable<(PkdObj<T1, T2>, PkdObj<T1, T2>)> SameTuple { get; private set; }
        public IEnumerable<(PkdObj<T1, T2>, PkdObj<T1, T2>)> UpdateTuple { get; private set; }
        public MergeCount Counts { get; private set; }
        public MergeIds mergeIds { get; private set; }
        public MergeIds Ids { get; private set; }

        public MergeInfo(
            IEnumerable<PkdObj<T1, T2>> persistedData,
            IEnumerable<PkdObj<T1, T2>> sheetData)
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            SetCollections(persistedData, sheetData);
            SetCounts(persistedData, sheetData);
            if (HasId<T1>())
            {
                SetIds<T1,T2>(persistedData, sheetData);
            }
        }

        private bool HasId<T>()
        {
            return fileService.Reflection.HasProp<T>("Id");
        }

        private void SetCounts(
            IEnumerable<PkdObj<T1, T2>> persistedData,
            IEnumerable<PkdObj<T1, T2>> sheetData)
        {
            Counts = new MergeCount(
               persistedData.Count(),
               sheetData.Count(),
               PersitedMore.Count(),
               SheetMore.Count(),
               SameTuple.Count(),
               UpdateTuple.Count());
        }

        private void
            SetIds<T1, T2>(
                IEnumerable<PkdObj<T1, T2>> persistedData,
                IEnumerable<PkdObj<T1, T2>> sheetData)
            where T1 : class
            where T2 : class
        {
            Ids = new MergeIds(
               persistedData.Select(x => x.GetKey()).ToList(),
               sheetData.Select(x => x.GetKey()).ToList(),
               PersitedMore.Select(x => x.GetKey()).ToList(),
               SheetMore.Select(x => x.GetKey()).ToList(),
               SameTuple.Select(x => x.Item1.GetKey()).ToList(),
               UpdateTuple.Select(x => x.Item1.GetKey()).ToList());
        }

        private void SetCollections(
            IEnumerable<PkdObj<T1, T2>> persistedData,
            IEnumerable<PkdObj<T1, T2>> sheetData)
        {
            SheetMore = sheetData
                .Where(x1 => !persistedData
                .Any(x2 => x2.GetKey() == x1.GetKey()));

            PersitedMore = persistedData
                .Where(x1 => !sheetData
                .Any(x2 => x2.GetKey() == x1.GetKey()));

            SameTuple = CompareLists2(
                persistedData,
                sheetData);

            UpdateTuple = FindDiffrentProperties(SameTuple);
        }

        
        //private Func<PkdObj<T1, T2>, string> GetKeySelector<T>()
        //{
        //    Func<PkdObj<T1, T2>, string> keySelector;
        //    if (typeof(T2).GetInterface(nameof(IHasId)) != null)
        //    {
        //        keySelector = x => ((IHasId)x.Target).Id;
        //        return keySelector;
        //    }
        //    if (typeof(T2).GetInterface(nameof(IHasDate)) != null)
        //    {
        //        keySelector = x => ((IHasDate)x.Target).Date;
        //        return keySelector;
        //    }
        //    if (typeof(T2).GetInterface(nameof(IHasName)) != null)
        //    {
        //        keySelector = x => ((IHasName)x.Target).Name;
        //        return keySelector;
        //    }
        //    return null;
        //}

        public IEnumerable<(PkdObj<T1, T2>, PkdObj<T1, T2>)>
            CompareLists2(
                IEnumerable<PkdObj<T1, T2>> list1,
                IEnumerable<PkdObj<T1, T2>> list2)
        {
            Func<PkdObj<T1, T2>, string> keySelector = (x) => x.GetKey();
            var commonElements = list1
                .Join(list2, keySelector, keySelector, (item1, item2) => (item1, item2))
                .ToList();
            return commonElements;
        }

        //public IEnumerable<(PkdObj<T, T2>, PkdObj<T, T2>)>
        //    CompareLists<T>(
        //    IEnumerable<PkdObj<T, T2>> list1,
        //    IEnumerable<PkdObj<T, T2>> list2)
        //{
        //    var result = new List<(PkdObj<T, T2>, PkdObj<T, T2>)>();
        //    var count = list1.Count();

        //    for (int i = 0; i < count; i++)
        //    {
        //        var item1 = list1.ElementAt(i);
        //        var item2 = list2.ElementAt(i);

        //        if (((IHasId)item1.Target).Id == ((IHasId)item2.Target).Id)
        //        {
        //            result.Add((item1, item2));
        //        }
        //    }

        //    return result;
        //}

        //public List<Tuple<T, T>>
        //    CompareLists2<T, TKey>(
        //        List<T> list1,
        //        List<T> list2,
        //        Func<T, TKey> keySelector)
        //{
        //    var commonElements = list1
        //        .Join(list2, keySelector, keySelector, (item1, item2) => Tuple.Create(item1, item2))
        //        .ToList();

        //    return commonElements;
        //}

        private IEnumerable<(PkdObj<T1, T2>, PkdObj<T1, T2>)>
            FindDiffrentProperties(
                IEnumerable<(PkdObj<T1, T2>, PkdObj<T1, T2>)> sameTuple)
        {
            var result = new List<(PkdObj<T1, T2>, PkdObj<T1, T2>)>();

            foreach (var item in sameTuple)
            {
                var pkdObjEqual = new PkdObjEqualBySource<T1, T2>(fileService);
                if (!pkdObjEqual.Equal(item.Item1, item.Item2))
                {
                    result.Add(item);
                }

                //if (!Equals(item.Item1.Source, item.Item2.Source))
                //{
                //    result.Add(item);
                //}
            }

            return result;
        }

        //public bool Equals(object obj01, object obj02)
        //{
        //    var thisTuples = GetTuples(obj02);
        //    var objTuples = GetTuples(obj01);

        //    if (thisTuples.Length != objTuples.Length)
        //    {
        //        return false;
        //    }

        //    for (int i = 0; i < thisTuples.Length; i++)
        //    {
        //        var tuple01 = thisTuples[i];
        //        var tuple02 = objTuples[i];
        //        if (tuple01 != tuple02)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public (string, string)[] GetTuples(object obj)
        //{
        //    var properties = obj.GetType().GetProperties();
        //    var tuples = properties.Select(x => (x.Name, x.GetValue(obj).ToString())).ToArray();
        //    return tuples;
        //}
    }
}
