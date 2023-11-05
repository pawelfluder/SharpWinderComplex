using Newtonsoft.Json.Linq;
using SharpFileServiceProg.AAPublic;
using SharpFileServiceProg.Service;
using SharpTinderComplexTests.JsonObjects;

namespace SharpTinderComplexTests.OldEntities
{
    public class OldApiDataImporter
    {
        private readonly IFileService fileService;
        private readonly IFileService.IYamlOperations yamlWorker;
        private string accountId;
        private readonly string exportedApiDataFolderPath;
        private readonly string meString = "_ja";
        private readonly string sheString = "ona";

        public OldApiDataImporter(string exportedApiDataFolderPath)
        {
            fileService = OutBorder.FileService();
            yamlWorker = fileService.Yaml.Custom01;
            this.exportedApiDataFolderPath = exportedApiDataFolderPath;
            if (!Directory.Exists(exportedApiDataFolderPath))
            {
                Directory.CreateDirectory(exportedApiDataFolderPath);
                //Todo - Try to import again
            }
        }

        internal Profile GetProfile(string accountId)
        {
            this.accountId = accountId;
            var accountFoldersList = Directory.GetDirectories(exportedApiDataFolderPath);
            var accountFolderPath = accountFoldersList.SingleOrDefault(x => Path.GetFileNameWithoutExtension(x).EndsWith(accountId));
            var allFilePaths = Directory.GetFiles(accountFolderPath);

            var filePath = allFilePaths.Where(x => Path.GetExtension(x) == ".txt")
            .Single(x => Path.GetFileName(x) == accountId + ".txt");

            var profile = yamlWorker.Deserialize<Profile>(filePath);
            //var profile2 = yamlWorker.Deserialize<Profile>(filePath);
            //var Profile2 = JsonConvert.SerializeObject(profile, Formatting.Indented);

            return profile;
        }

        internal List<Match> GetMatchesInfos(string accountId)
        {
            this.accountId = accountId;
            var accountFoldersList = Directory.GetDirectories(exportedApiDataFolderPath);
            var accountFolderPath = accountFoldersList.SingleOrDefault(x => Path.GetFileNameWithoutExtension(x).EndsWith(accountId));
            var allFilePaths = Directory.GetFiles(accountFolderPath);
            var apiFilePaths = allFilePaths.Where(x => Path.GetExtension(x) == ".txt")
            .Where(x => Path.GetFileName(x) != accountId + ".txt");

            var ouput = string.Empty;

            var result = new List<Match>();
            foreach (var filePath in apiFilePaths)
            {
                var messages = yamlWorker.Deserialize<Match>(filePath);
                //var messages2 = yamlWorker.Deserialize<Match>(filePath);
                result.Add(messages);
            }

            return result;
        }

        private Message2 GetMessage(JToken jtoken)
        {
            var text = jtoken["message"].ToString();
            var ownerId = jtoken["from"].ToString();
            var description = string.Empty;
            if (ownerId == accountId)
            {
                description = meString;
            }
            else
            {
                description = sheString;
            }

            var message = new Message2(text, ownerId, description);
            return message;
        }
    }
}