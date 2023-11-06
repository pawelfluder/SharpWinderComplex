namespace SharpSheetToObjProg.Merge
{
    internal class MergeCount
    {
        public int CountOfPersistedData { get; }
        public int CountOfSheetData { get; }
        public int CountOfPersistedMore { get; }
        public int CountOfSheetMore { get; }
        public int CountOfSame { get; }
        public int CountOfUpdate { get; }

        //public int CountOfExistedData { get; }
        //public int CountOfAllToSaveData { get; }

        public MergeCount(
           int countOfPersistedData,
           int countOfSheetData,
           int countOfPersistedMore,
           int countOfSheetMore,
           int countOfSame,
           int countOfUpdate)
        {
            CountOfPersistedData = countOfPersistedData;
            CountOfSheetData = countOfSheetData;
            CountOfPersistedMore = countOfPersistedMore;
            CountOfSheetMore = countOfSheetMore;
            CountOfSame = countOfSame;
            CountOfUpdate = countOfUpdate;
            //CountOfExistedData = countOfExistedData;
            //CountOfAllToSaveData = countOfAllToSaveData;
        }
    }
}
