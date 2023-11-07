namespace SharpSheetToObjProg.Merge
{
    internal class MergeCount
    {
        public int PersistedData { get; }
        public int SheetData { get; }
        public int PersistedMore { get; }
        public int SheetMore { get; }
        public int Same { get; }
        public int Update { get; }

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
            PersistedData = countOfPersistedData;
            SheetData = countOfSheetData;
            PersistedMore = countOfPersistedMore;
            SheetMore = countOfSheetMore;
            Same = countOfSame;
            Update = countOfUpdate;
            //CountOfExistedData = countOfExistedData;
            //CountOfAllToSaveData = countOfAllToSaveData;
        }
    }
}
