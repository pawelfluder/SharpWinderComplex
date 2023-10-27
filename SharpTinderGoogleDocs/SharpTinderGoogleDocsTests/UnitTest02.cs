using SharpFileServiceProg.Operations.Dictionaries;
using SharpTinderGoogleDocsTests.Repetition.Names;
using TinderImport;

namespace SharpTinderGoogleDocsTests
{
    [TestClass]
    public class UnitTest02 : UnitTest02Base
    {
        [TestMethod]
        public void Phase_01_PreparePaths()
        {
            //configService.Prepare(typeof(IPreparer.IWinder));
        }

        // done
        //[DataRow("23-05-09_paweł_min-1", "645a90a505ed8501009ff2fc", 1, int.MaxValue)]
        //[DataRow("23-02-25_paweł_min-3", "63fa30e4e5862301002fe901", 3, int.MaxValue)]
        //[DataRow("2023-01-02_kamil_min-3", "63b3446307fbca0100fd9d13", 3, int.MaxValue)]


        // todo
        // 01_2022-12-28_paweÅ_63accf430996fb0100a34a72
        // 02_2022-07-08_wiktor_62c89c9a4e95f00100eb6623
        // 03_2022-10-11_paweÅ_6345b8b52cc95901002584be
        // 04_2022-10-22_kamil_6353eca623d8310100fd8be6
        // 05_2022-10-30_kamil_635e47ac1e983b01004469ac; skończyłem - 604b73f4ea21880100fcd2e5635e47ac1e983b01004469ac
        // 06_2022-11-14_wojtek_63728e501d5d650100f36d82

        //[DataRow("22-12-28_paweł_min-3", "63accf430996fb0100a34a72", 3, int.MaxValue)]
        //[DataRow("22-07-08_wiktor_min-3", "62c89c9a4e95f00100eb6623", 3, int.MaxValue)]
        //[DataRow("22-10-11_paweł_min-3", "6345b8b52cc95901002584be", 3, int.MaxValue)]
        //[DataRow("22-10-22_kamil_min-3", "6353eca623d8310100fd8be6", 3, int.MaxValue)]
        //[DataRow("22-10-30_kamil_min-3", "635e47ac1e983b01004469ac", 3, int.MaxValue)]
        //[DataRow("22-11-14_wojtek_min-3", "63728e501d5d650100f36d82", 3, int.MaxValue)]

        // current
        //[DataRow("23-05-09_Paweł_min-1", "645a90a505ed8501009ff2fc", 1, int.MaxValue)]

        //[DataRow("20-07-22_Daniel", "5f187f301b5e900100fcf8b5", 3, int.MaxValue)]

        [DataRow("23-05-09_Paweł", "645a90a505ed8501009ff2fc", 3, int.MaxValue)]
        [TestMethod]
        public void Phase_10_CreateGoogleDoc(string docFileName, string myAccoutId, int start, int stop)
        //public void Phase_10_CreateGoogleDoc()
        {
            // arrange
            Console.WriteLine("Start test");
            (int Start, int Stop) herMessageRange = (start, stop);
            var tinderOperations = new TinderOperations(null);
            var appDataRepoName = configService.SettingsDict[SettingNames.WinderRepoName].ToString();
            var address = repoService.Methods.GetPathsByName(
                (appDataRepoName, ""), new List<string> { FolderNames.Winder, FolderNames.ExportedApiData });
            var accountNamesList = repoService.Methods.GetAllFoldersNames(address);
            var accountName = accountNamesList.SingleOrDefault(x => x.EndsWith(myAccoutId));
            var contentsList = repoService.Methods.GetManyItemByName(
                address, new List<string> { accountName });
            contentsList.RemoveAt(0);
            var matchesDictionaries = contentsList.Select(x => yamlWorker.Deserialize<Dictionary<object, object>>(x))
                .ToList();
            var operations = new DictionaryOperations();
            matchesDictionaries.ForEach(x => operations.NewShape01(x));

            // act
            (var docId, var docName) = CreateNewDocFile(docFileName); //var docId = "";
            matchesDictionaries.RemoveAll(x => !tinderOperations.IsInHerMessageRange(myAccoutId, x, herMessageRange));
            tinderOperations.ExportGenerationMessage(docId, matchesDictionaries, myAccoutId);

            foreach (var item in matchesDictionaries)
            {
                tinderOperations.ExportMatchToGoogleDoc(docId, item, myAccoutId);
            }

            tinderOperations.DeleteTempFolder();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            //tinderOperations.DeleteTempFolder();
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            //tinderOperations.DeleteTempFolder();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            //tinderOperations.DeleteTempFolder();
        }
    }
}