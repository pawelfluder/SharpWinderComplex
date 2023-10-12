using GoogleDocsServiceProj.Service;
using SharpGoogleDriveProg.Service;
using System.Collections.Generic;
using TinderImport;
using TinderImport.Repetition;

namespace TinderExportTests
{
    [TestClass]
    public class UnitTest3
    {
        private readonly GoogleDocsService docsService;
        private readonly GoogleDriveService driveService;

        public UnitTest3()
        {
            var credentialWorker = new CredentialWorker();
            var credentials = credentialWorker.GetCredentials();
            docsService = Border.GetGoogleDocsService(credentials.clientId, credentials.clientSecret);
            driveService = Border.GetGoogleDriveService(credentials.clientId, credentials.clientSecret);
        }

        //[DataRow("2023-02-25_paweł_rest", "63accf430996fb0100a34a72", 0, 2)]//2022-12-28_paweł_63accf430996fb0100a34a72
        //[DataRow("2023-02-25_paweł_63fa30e4e5862301002fe901", "63fa30e4e5862301002fe901", 3, int.MaxValue)]//2022-12-28_paweł_63accf430996fb0100a34a72

        //[DataRow("2022-12-28_paweł_rest", "63accf430996fb0100a34a72", 0, 2)] //2022-12-28_paweł_63accf430996fb0100a34a72
        //[DataRow("2022-12-28_paweł_min-her-three-messages", "63accf430996fb0100a34a72", 3, int.MaxValue)] //2022-12-28_paweł_63accf430996fb0100a34a72

        //[DataRow("2022-10-11_paweł_rest", "6345b8b52cc95901002584be", 0, 2)] //2022-10-11_paweł_6345b8b52cc95901002584be
        //[DataRow("2022-10-11_paweł_min-her-three-messages", "6345b8b52cc95901002584be", 3, int.MaxValue)] //2022-10-11_paweł_6345b8b52cc95901002584be

        [DataRow("2023-02-25_paweł", "63fa30e4e5862301002fe901", 0, 2)] //2023-02-25_paweł_63fa30e4e5862301002fe901
        //[DataRow("2023-02-25_paweł_min-her-three-messages", "63fa30e4e5862301002fe901", 3, int.MaxValue)] //2023-02-25_paweł_63fa30e4e5862301002fe901

        [TestMethod]
        public void TestMethod3(string docFileName, string accountId, int start, int stop)
        {
            //arrange
            (int Start, int Stop) msgCountRange = (start, stop);
            var tinderOperations = new TinderOperations(null);

            //act
            (var docId, var docName) = CreateNewDocFile(docFileName);

            //tinderOperations.ExportGenerationMessage(
            //    docId, );

            //ExportGenerationMessage(
            //string googleDocsId,
            //List < Dictionary<object, object> > matchesDictionaries,
            //string myAccoutId)

            tinderOperations.ExportAllAccountMatchesToGoogleDoc(
                docId,
                accountId,
                msgCountRange);

            //assert
            Console.WriteLine("Finished");
        }

        private (string id, string name) CreateNewDocFile(string fileName)
        {
            var folder2023 = "13gY7OdaPCMwHQKmJZWZpcou7xtMrxNlg";
            var result = driveService.Worker.CreateNewDocFile(folder2023, fileName);
            return result;
        }
    }
}