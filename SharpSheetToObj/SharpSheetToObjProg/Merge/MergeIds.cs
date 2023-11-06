namespace SharpSheetToObjProg.Merge
{
    public class MergeIds
    {
        public List<string> PersistedData { get; }
        public List<string> SheetData { get; }
        public List<string> PersistedMore { get; }
        public List<string> SheetMore { get; }
        public List<string> Same { get; }
        public List<string> Update { get; }

        //public int CountOfExistedData { get; }
        //public int CountOfAllToSaveData { get; }

        public MergeIds(
           List<string> countOfPersistedData,
           List<string> countOfSheetData,
           List<string> countOfPersistedMore,
           List<string> countOfSheetMore,
           List<string> countOfSame,
           List<string> countOfUpdate)
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