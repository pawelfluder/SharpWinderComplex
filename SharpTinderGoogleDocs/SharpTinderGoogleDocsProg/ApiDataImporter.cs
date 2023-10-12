using Newtonsoft.Json.Linq;
using SharpTinderGoogleDocsProg.Entities;
using TinderImport;

public class ApiDataImporter
{
    private readonly YamlWorker yamlWorker;
    private string accountId;
    private readonly string exportedApiDataFolderPath;
    private readonly string meString = "_ja";
    private readonly string sheString = "ona";

    public ApiDataImporter(string exportedApiDataFolderPath)
	{
        yamlWorker = new YamlWorker();
        this.exportedApiDataFolderPath = exportedApiDataFolderPath;
        if (!Directory.Exists(exportedApiDataFolderPath))
        {
            Directory.CreateDirectory(exportedApiDataFolderPath);
            //Todo - Try to import again
        }
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
            var messages = GetMatchMessages(filePath);
            result.Add(messages);
        }

        return result;
    }

    private Message GetMessage(JToken jtoken)
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

        var message = new Message(text, ownerId, description);
		return message;
    }

    private Match GetMatchMessages(string filePath)
    {
        var yamlText = yamlWorker.Deserialize(filePath);
        var w = new StringWriter();
        var js = new Newtonsoft.Json.JsonSerializer();
        js.Serialize(w, yamlText);
        string jsonText = w.ToString();

        var jsonObj = JObject.Parse(jsonText);

        
        var person = jsonObj["person"];
        var personName = person["name"].ToString();
        var personBirth = DateTime.Parse(person["birth_date"].ToString());
        var photos = person["photos"];
        var processedFiles = photos.Select(x => x["processedFiles"]).ToList();
        var processedFilesUrls = processedFiles.Select(x => x[0]["url"].ToString()).ToList();
        var personId = person["_id"].ToString();
        var bio = person["bio"]?.ToString();
        var bioLineList = new List<string>();
        if (bio != null)
        {
            bioLineList = bio?.Split("\n").ToList();
        }

        var messagesObjList = jsonObj["messages"].Select(x => GetMessage(x)).ToList();
        var personObj = new Person(personName, personBirth, processedFilesUrls, personId, bioLineList);
        var matchObj = new Match(personObj, messagesObjList);

        return matchObj;
    }
}
