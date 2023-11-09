using SharpSheetToObjProg.HasProperty;

namespace SharpSheetToObjProg.Merge
{
    internal class MergeInfo<T1, T2> where T1 : class
    {
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
            SetCollections(persistedData, sheetData);
            SetCounts(persistedData, sheetData);
            SetIds(persistedData, sheetData);
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

        private void SetIds(
            IEnumerable<PkdObj<T1, T2>> persistedData,
            IEnumerable<PkdObj<T1, T2>> sheetData)
        {
            Ids = new MergeIds(
               persistedData.Select(x => ((IHasId)x.Target).Id).ToList(),
               sheetData.Select(x => ((IHasId)x.Target).Id).ToList(),
               PersitedMore.Select(x => ((IHasId)x.Target).Id).ToList(),
               SheetMore.Select(x => ((IHasId)x.Target).Id).ToList(),
               SameTuple.Select(x => ((IHasId)x.Item1.Target).Id).ToList(),
               UpdateTuple.Select(x => ((IHasId)x.Item1.Target).Id).ToList());
        }

        private void SetCollections(
            IEnumerable<PkdObj<T1, T2>> persistedData,
            IEnumerable<PkdObj<T1, T2>> sheetData)
        {
            SheetMore = sheetData
                .Where(x1 => !(persistedData
                .Select(x2 => x2.Target)
                .Any(y => ((IHasId)y).Id == ((IHasId)x1.Target).Id)));

            PersitedMore = persistedData
                .Where(x1 => !(sheetData
                .Select(x2 => x2.Target)
                .Any(y => ((IHasId)y).Id == ((IHasId)x1.Target).Id)));

            //SameTuple = CompareLists(
            //    sheetData,
            //    persistedData);

            SameTuple = CompareLists2(
                sheetData,
                persistedData,
                x => ((IHasId)x.Target).Id);

            UpdateTuple = FindDiffrentProperties(SameTuple);
        }

        public IEnumerable<(T, T)> CompareLists2<T, TKey>(IEnumerable<T> list1, IEnumerable<T> list2, Func<T, TKey> keySelector)
        {
            var commonElements = list1
                .Join(list2, keySelector, keySelector, (item1, item2) => (item1, item2))
                .ToList();

            return commonElements;
        }

        public IEnumerable<(PkdObj<T, T2>, PkdObj<T, T2>)>
            CompareLists<T>(
            IEnumerable<PkdObj<T, T2>> list1,
            IEnumerable<PkdObj<T, T2>> list2)
        {
            var result = new List<(PkdObj<T, T2>, PkdObj<T, T2>)>();
            var count = list1.Count();

            for (int i = 0; i < count; i++)
            {
                var item1 = list1.ElementAt(i);
                var item2 = list2.ElementAt(i);

                if (((IHasId)item1.Target).Id == ((IHasId)item2.Target).Id)
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

        private IEnumerable<(PkdObj<T, T2>, PkdObj<T, T2>)>
            FindDiffrentProperties<T>(
            IEnumerable<(PkdObj<T, T2>, PkdObj<T, T2>)> sameTuple)
            where T : class
        {
            var result = new List<(PkdObj<T, T2>, PkdObj<T, T2>)>();

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
    }
}
