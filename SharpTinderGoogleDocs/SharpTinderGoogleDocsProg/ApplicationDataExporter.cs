using System.Net;
using SharpTinderGoogleDocsProg.Entities;

namespace TinderImport
{
    internal class ApplicationDataExporter
    {
        private readonly string outputFolderPath;
        private readonly string applicationDataFolder;
        private readonly ConversationPrinter printer;

        public ApplicationDataExporter(string outputFolderPath, ConversationPrinter printer)
        {
            this.outputFolderPath = outputFolderPath;
            this.applicationDataFolder = outputFolderPath + "/" + "ExportedApplicationData";
            this.printer = printer;
        }

        //public void ExportOneMatchPhoto(Match match)
        //{
        //    var accountFolderPath = applicationDataFolder + "/" + accountId;
        //    string matchFolder = accountFolderPath + "/" + match.Person.Id;

        //    ExportOneMatchPhoto(match, matchFolder);
        //}

        public void ExportOneMatchPhoto(List<string> photoUrls, string tempPath)
        {
            if (!Directory.Exists(tempPath))
    
            {
                Directory.CreateDirectory(tempPath);
            }

            foreach (var photoUrl in photoUrls)
            {
                DownloadPhoto(photoUrl, tempPath);
            }
        }


        public void ExportPhotos(List<Match> matchList, string accountId)
        {
            //foreach (Match match in matchList)
            //{
            //    ExportOneMatchPhoto(match);
            //}
        }

        public void ExportAllConversations(List<Match> matchList, string accountId)
        {
            var accountFolderPath = applicationDataFolder + "/" + accountId;

            var outputText = printer.GetMatchListOutput(matchList);
            ExportConveration(accountFolderPath, "AllConversations.txt", outputText);
        }

        public void ExportConversations(List<Match> matchList, string accountId)
        {
            var accountFolderPath = applicationDataFolder + "/" + accountId;

            foreach (Match match in matchList)
            {
                var matchFolder = accountFolderPath + "/" + match.Person.Id;
                if (!Directory.Exists(matchFolder))
                {
                    Directory.CreateDirectory(matchFolder);
                }
                if (matchFolder.EndsWith("6211389b064e8201006a80b3"))
                {

                }

                var outputText = printer.GetMsgListOutput(match.Messages);
                ExportConveration(matchFolder, "Conversation.txt", outputText);
            }
        }

        public void ExportConveration(string outputFolder, string fileName, List<string> linesList)
        {
            var convFilePath = outputFolder + "/" + fileName;
            try
            {
                using (StreamWriter stream = new StreamWriter(convFilePath))
                {
                    var output = string.Join("\n", linesList);
                    stream.Write(output);
                }
            }
            catch { }
        }

        public void DownloadPhoto(string url, string matchFolder)
        {
            var photoName = Path.GetFileName(url);
            var photoFilePath = matchFolder + "/" + photoName;
            if (!File.Exists(photoFilePath))
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(url), photoFilePath);
                    }
                }
                catch { }
            }
        }
    }
}
