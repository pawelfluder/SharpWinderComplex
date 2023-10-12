using Google.Apis.Docs.v1.Data;
using GoogleDocsServiceProj.Service;
using SharpFileServiceProg.Operations.Conversations;
using SharpFileServiceProg.Operations.Date;
using SharpGoogleDriveProg.Service;
using SharpTinderGoogleDocsProg.Entities;
using TinderImport.Repetition;

namespace TinderImport
{
    public class TinderOperations
    {
        private string outputFolderPath;
        private readonly ConversationPrinter conversationPrinter;
        private readonly ApplicationDataExporter applicationDataExporter;
        private string exportedApiDataFolderPath;
        private ApiDataImporter apiDataImporter;

        private readonly GoogleDocsService docsService;
        private readonly GoogleDriveService driveService;
        private readonly YamlWorker yamlWorker;
        private readonly DateOperations dateOperations;
        private readonly ConvOperations conversationsOp;

        public TinderOperations(string outputFolderPath)
        {
            outputFolderPath = "D:/01_Synchronized/01_Programming_Files/8c0f7763-7149-4b4d-9d6a-b28d3984552f/15_Zbior/PythonTinderApiDataExport/Output";
            conversationsOp = new ConvOperations();
            var credentialWorker = new CredentialWorker();
            var credentials = credentialWorker.GetCredentials();
            docsService = Border.GetGoogleDocsService(credentials.clientId, credentials.clientSecret);
            driveService = Border.GetGoogleDriveService(credentials.clientId, credentials.clientSecret);
            yamlWorker = new YamlWorker();
            dateOperations = new DateOperations();

            this.outputFolderPath = outputFolderPath;
            this.conversationPrinter = new ConversationPrinter();
            this.applicationDataExporter = new ApplicationDataExporter(outputFolderPath, conversationPrinter);
            this.exportedApiDataFolderPath = outputFolderPath + "/" + "ExportedApiData";
            this.apiDataImporter = new ApiDataImporter(exportedApiDataFolderPath);
        }

        public bool IsInHerMessageRange(
            string accountId,
            Dictionary<object, object> match,
            (int Start, int Stop) herMessageRange)
        {
            var herCount = conversationsOp.Worker03.CountHerMessages(match, accountId);
            if (herCount >= herMessageRange.Start && herCount <= herMessageRange.Stop)
            {
                return true;
            }

            return false;
        }

        public void ExportGenerationMessage(
            string googleDocsId,
            List<Dictionary<object, object>> matchesDictionaries,
            string myAccoutId)
        {
            var machesCount = matchesDictionaries.Count();
            var date = dateOperations.UderscoreDate(DateTime.Now);
            var ids = matchesDictionaries.Select(x => x["id"]).ToList();
            var theirIds = ids.Select(x => conversationsOp.Worker01.GetHerId(x.ToString(), myAccoutId)).ToList();
            var idsString = string.Join(", ", theirIds);
            var textList = new List<string>()
            {
                $"Generation date: {date}",
                $"Matches count: {machesCount}",
                $"MyAccountId: {myAccoutId}",
                $"Ids: {idsString}",
                string.Empty,
            };
            docsService.Worker.AppendToDocument(googleDocsId, textList, 1, "\n");
        }

        public void ExportMatchToGoogleDoc(
            string googleDocsId,
            Dictionary<object, object> data,
            string accoutId)
        {
            var tmp1 = (List<object>)data["url"];
            var photoUrls = tmp1.Select(x => (string)x).ToList();
            data.Remove("url");

            var lastIndex = docsService.Worker.GetDocumentLastIndex(googleDocsId);
            ExportPhotosToGoogleDoc(photoUrls, googleDocsId, lastIndex);

            var linesList = conversationsOp.Worker02.GetConversationForGoogleDoc(data, accoutId);
            lastIndex = docsService.Worker.GetDocumentLastIndex(googleDocsId);
            docsService.Worker.AppendToDocument(googleDocsId, linesList, lastIndex, "\n");
        }

        public void DeleteTempFolder()
        {
            driveService.Worker.DeleteTempFolder();
        }

        public void ExportMatchToGoogleDoc(string googleDocsId, string accountId, string personId)
        {
            //var match = GetMatch(accountId, personId);
            //ExportProfileToGoogleDoc(match, googleDocsId);
        }

        public void ExportAllAccountMatchesToGoogleDoc(
            string googleDocsId,
            string accountId,
            (int Start, int Stop) msgCountRange,
            List<string> except = default)
        {
            //ExportMatchesToGoogleDocs(
            //    accountId,
            //    googleDocsId,
            //    msgCountRange,
            //    except);
                        
            //string googleDocsId,
            //(int Start, int Stop) range,
            //List<Match> matchesList)
        }

        public void ExportAllAccountMatchesToGoogleDoc(string googleDocsId, string accountId, List<string> except)
        {
            //ExportMatchesToGoogleDocs(accountId, googleDocsId, default, except);
        }

        public void ExportBlueprintsToGoogleDoc(string googleDocsId, string accountId, List<(string, string)> personIdQBluprintList)
        {
            personIdQBluprintList = new List<(string, string)>()
            {
                ("630a545ea84a800100f1c6e2", "z/szc"),
            };

            var gg = personIdQBluprintList.Where(x => x.Item2 != "?" && x.Item2 != "x").Count();
            var gg2 = personIdQBluprintList.Count();

            //ExportBluprintsToGoogleDocs(accountId, personIdQBluprintList, googleDocsId);
        }

        public Match GetMatch(string accountId, string matchId)
        {
            var matchesList = apiDataImporter.GetMatchesInfos(accountId);
            var match = matchesList.SingleOrDefault(x => x.Person.Id == matchId);
            return match;
        }

        public void ExportOneMatchPhotos(string accountId)
        {
            ExportAllPhotos(accountId);
        }

        private string GetAccountIdFolderPath(string accountId)
        {
            var folderPaths = Directory.GetDirectories(exportedApiDataFolderPath);
            var accountIdsList = folderPaths.Select(x => Path.GetFileName(x));
            var accountIdFolderPath = accountIdsList.SingleOrDefault(x => x.Equals(accountId));
            return accountIdFolderPath;
        }

        public IEnumerable<string> GetAllAccountMatchesIds(string accountId)
        {
            var matches = apiDataImporter.GetMatchesInfos(accountId);
            var personIdsList = matches.Select(x => x.Person.Id);
            return personIdsList;
        }

        public void ExportMatchesToGoogleDocs(
            string googleDocsId,
            (int Start, int Stop) range,
            List<Match> matchesList)
        {

            //foreach (var match in matchesList)
            //{
            //    //var matchToExport2 = matches.FirstOrDefault(x => x.Person.Id == "5bad27e6d143a7450a33967d");

            //    var herMessages = match.Messages.Where(x => x.OwnerDescription == Labels.Her);
            //    if (IsMessagesCountInRange(range, match))
            //    {
            //        ExportProfileToGoogleDoc(match, googleDocsId);
            //    }
            //    else
            //    {
            //    }
            //}
            //driveService.Worker.DeleteTempFolder();
        }

        //public void ExportMatchesToGoogleDocs(
        //    string accountId,
        //    string googleDocsId,
        //    (int Start, int Stop) range = default,
        //    List<string> except = default)
        //{
        //    var matches = apiDataImporter.GetMatchesInfos(accountId);

        //    foreach (var match in matches)
        //    {
        //        //var matchToExport2 = matches.FirstOrDefault(x => x.Person.Id == "5bad27e6d143a7450a33967d");

        //        var herMessages = match.Messages.Where(x => x.OwnerDescription == Labels.Her);
        //        if (IsMessagesCountInRange(range, match) &&
        //            (except == default || except!.Contains(match.Person.Id)))
        //        {
        //            ExportProfileToGoogleDoc(match, googleDocsId);
        //        }
        //        else
        //        {
        //        }
        //    }
        //    driveService.Worker.DeleteTempFolder();
        //}

        private bool IsMessagesCountInRange((int Start, int Stop) range, Match match)
        {
            if (range == default)
            {
                return true;
            }

            if (match.Messages.Count < range.Start)
            {
                return false;
            }

            if (match.Messages.Count > range.Stop)
            {
                return false;
            }

            return true;
        }

        //public void UpdateMatchesWithBlueprintCode(List<Match> matches, List<(string, string)> personIdsQBluprintsList)
        //{
        //    foreach (var idQbpcode in personIdsQBluprintsList)
        //    {
        //        var match = matches.SingleOrDefault(x => x.Person.Id == idQbpcode.Item1.ToString());
        //        if (match != null)
        //        {
        //            match.Person.SetBluePrintCode(idQbpcode.Item2);
        //        }
        //    }
        //}

        

        public List<string> GetFullText(Dictionary<object, object> data)
        {
            var text = yamlWorker.Serialize(data);
            var textList = text.Split('\n').ToList();
            textList.Insert(0, "\n");
            textList.Add("\n");
            return textList;

            //var id = data["id"].ToString();
            //var name = data["name"].ToString();
            //var birthDate = data["birth_date"].ToString();
            //var bio = (List<string>)data["birth_date"];
            //var bluprintsList = new List<string>();//  match.Person.BlueprintsList;
            //var messages = (List<Message>)data["messages"];

            //var lineList = new List<string>();
            //lineList.Add(string.Empty);
            //lineList.Add(name + " " + birthDate);
            //lineList.Add("Id: " + id);
            //lineList.Add("BIO:");
            //lineList.AddRange(bio);
            //if (bluprintsList.Count > 0)
            //{
            //    lineList.Add("BLUEPRINTY: " + string.Join(", ", bluprintsList));
            //}
            //lineList.Add("KONWERSACJA:");
            //lineList.AddRange(conversationPrinter.GetMsgListOutput(messages));
            //lineList.Add(string.Empty);
            //return lineList;
        }

        public void ExportPhotosToGoogleDoc(List<string> photoUrls, string googleDocsId, int index)
        {
            var tempPath = outputFolderPath + "/" + "Temp";
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }

            Directory.CreateDirectory(tempPath);

            ExportOneMatchPhoto(photoUrls, tempPath);

            var filePathList = Directory.GetFiles(tempPath).ToList();
            var tempFileIdQNames = new List<(string, string)>();
            var photoRequests = new List<Request>();
            var photoFileStreamsList = new List<FileStream>();

            foreach (var filePath in filePathList)
            {
                var photoFileStream = File.Open(filePath, FileMode.Open);
                photoFileStreamsList.Add(photoFileStream);
                var idQName = driveService.UploadTempPhotoFile(photoFileStream);
                tempFileIdQNames.Add(idQName);
                var uri = $"https://drive.google.com/u/0/uc?id={idQName.Item1}&export=download";
                var request = docsService.Worker.GetInsertPhotosRequests(100, uri, index);
                photoRequests.Add(request);
            }

            Thread.Sleep(1000);
            docsService.Worker.ExecuteBatchUpdate(photoRequests, googleDocsId);
            driveService.Worker.RemoveFiles(tempFileIdQNames.Select(x => x.Item1).ToList());

            photoFileStreamsList.ForEach(x => x.Dispose());
            filePathList.ForEach(x => File.Delete(x));
        }

        public void ExportEverything()
        {
            var folderPaths = Directory.GetDirectories(exportedApiDataFolderPath);
            for (int i = 0; i < folderPaths.Length; i++)
            {
                var folderPath = folderPaths.ElementAt(2);
                var accountId = Path.GetFileName(folderPath);
                ExportTextData(accountId);
            }

            for (int i = 0; i < folderPaths.Length; i++)
            {
                var folderPath = folderPaths.ElementAt(3);
                var accountId = Path.GetFileName(folderPath);
                ExportAllPhotos(accountId);
            }
        }

        private void ExportTextData(string accountId)
        {
            var matches = apiDataImporter.GetMatchesInfos(accountId);

            var openers = conversationPrinter.GetMatchListOutput3(matches);
            var uniqOpeners = new HashSet<string>(openers);
            var uniqOpners = RealDistinct(openers);
            //openers.Distinct();

            applicationDataExporter.ExportConversations(matches, accountId);
            applicationDataExporter.ExportAllConversations(matches, accountId);

            var output = conversationPrinter.GetMatchListOutput(matches);
            conversationPrinter.Print(output);
        }

        private IEnumerable<T> RealDistinct<T>(IEnumerable<T> source)
        {
            List<T> uniques = new List<T>();
            foreach (T item in source)
            {
                if (!uniques.Contains(item)) uniques.Add(item);
            }
            return uniques;
        }

        private void ExportAllPhotos(string accountId)
        {
            var matches = apiDataImporter.GetMatchesInfos(accountId);
            applicationDataExporter.ExportPhotos(matches, accountId);
        }

        private void ExportOneMatchPhoto(List<string> photoUrls, string tempPath)
        {
            //var matches = apiDataImporter.GetMatchesInfos(accountId);
            //var match = matches.SingleOrDefault(x => x.Person.Id == accountId);
            applicationDataExporter.ExportOneMatchPhoto(photoUrls, tempPath);
        }

        //private void ExportOneMatchPhoto(Match match)
        //{
        //    applicationDataExporter.ExportOneMatchPhoto(match);
        //}
    }
}
