namespace GameSynchCoreProj
{
    internal class Rest
    {
        private int idOfFile;

        public void SyncMp3Files()
        {
            //var persistedFileNames = (IList<object>)(persistency.GetData("Podejscia_Dane") as IList<DataApproach>).Select(x => x.FileName() as object).ToList();

            //SychPersistedWithDriveInput();

            //googleSheetService.SynchMp3FilesWithSheet(names, persistedFileNames);
        }

        private string GetNewId()
        {
            idOfFile++;
            return idOfFile.ToString();
        }

        private void SychPersistedWithDriveInput()
        {
            //var inputData = googleDriveService.GetAllMp3Files().Select(x => x.Name as object).ToList();
            //var persistedData = (persistency.GetData("DriveDisk_Mp3ApproachesFiles") as IList<DataFile>);

            //var removedData = persistedData.Where(x => !inputData.Any(y => y == x.Name)).ToList();
            //var newData = inputData.Where(x => !persistedData.Any(y => y.Name == x)).ToList();

            //var persistedData = (persistency.Get("Podejscia_Dane") as IList<DataApproach>);

            //var dataToAdd = newData.Select(x => new DataFile(GetNewId(), x.ToString(), "x")).ToList();

            //persistency.SaveData("DriveDisk_Mp3ApproachesFiles", dataToAdd);

            var gg = string.Empty;

            // Todo
            // Save (id, mp3fileName) to persistency

            //var excelSheetData = GetExcelSheetData();


            //var newData = persistedData.Where(x => !inputData.Any(y => y.Id == x.Id));
            //var changes = GetChanges(persistedData, excelSheetData);
            //
            //var existedSheetData = changes.Select(x => x.Item2);
            //var allDataToSave = new List<DataApproach>(removedData.Concat(existedSheetData).Concat(newData));

            //var countOfPersistedData = persistedData.Count();
            //var countOfExcelSheetData = excelSheetData.Count();
            //var countOfRemovedData = removedData.Count();
            //var countOfNewData = newData.Count();
            //var countOfNotChanged = changes.Count(x => x.Item1 == false);
            //var countOfChangedData = changes.Count(x => x.Item1 == true);
            //var countOfExistedData = existedSheetData.Count();
            //var countOfAllToSaveData = allDataToSave.Count();

            //if (countOfRemovedData > 0 ||
            //   countOfNewData > 0 ||
            //   countOfChangedData > 0 &&
            //   countOfChangedData <= 1)
            //{
            //   string yamlResult = yamlSerializerDotNet.Serialize(allDataToSave);
            //   File.WriteAllText(info.filePath, yamlResult);
            //   IList<IList<object>> result = allDataToSave.Select(x => x.ToIList()).ToList();
            //   googleSheetService.PasteDataToSheet(info.idOfMainSheet, info.nameOfMainSheet, result);
            //}
        }
    }
}
