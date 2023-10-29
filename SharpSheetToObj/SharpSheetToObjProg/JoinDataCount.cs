namespace GameSynchCoreProj
{
   internal class JoinDataCount
   {
      public int CountOfPersistedData { get; }
      public int CountOfExcelSheetData { get; }
      public int CountOfRemovedData { get; }
      public int CountOfNewData { get; }
      public int CountOfNotChanged { get; }
      public int CountOfChangedData { get; }
      public int CountOfExistedData { get; }
      public int CountOfAllToSaveData { get; }

      public JoinDataCount(
         int countOfPersistedData,
         int countOfExcelSheetData,
         int countOfRemovedData,
         int countOfNewData,
         int countOfNotChanged,
         int countOfChangedData,
         int countOfExistedData,
         int countOfAllToSaveData
         )
      {
         CountOfPersistedData = countOfPersistedData;
         CountOfExcelSheetData = countOfExcelSheetData;
         CountOfRemovedData = countOfRemovedData;
         CountOfNewData = countOfNewData;
         CountOfNotChanged = countOfNotChanged;
         CountOfChangedData = countOfChangedData;
         CountOfExistedData = countOfExistedData;
         CountOfAllToSaveData = countOfAllToSaveData;
      }      
   }
}
