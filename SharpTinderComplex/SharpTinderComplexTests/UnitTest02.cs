using Newtonsoft.Json;
using SharpTinderApiDataImport;
using SharpTinderComplexTests.JsonObjects;
using System.Diagnostics;
using SharpSQLiteProj;
using Microsoft.CodeAnalysis;

namespace SharpTinderComplexTests
{
    [TestClass]
    public class UnitTest02 : UnitTest02Base
    {
        [TestMethod]
        public void Phase_02_SaveApiTokenBySqliteFile()
        {
            // arrange
            configService.SettingsDict.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));
            var filePath = configService.SettingsDict["tinderSqlLitePath"].ToString();
            var columnNumbers = new List<int> { 0, 5 };
            var sqlQuery = "SELECT * FROM 'data' LIMIT 0,30";
            var sqlLiteService = new SQLiteService();

            // act
            var result = sqlLiteService.ReadFile(filePath, columnNumbers, sqlQuery);

            // save
            var itemName = "TinderWeb/APIToken";
            var apiToken = result.Single(x => x.First() == itemName).Last();
            Console.WriteLine(apiToken);
            configService.AddSetting("tinderApiToken", apiToken);

            //var configFilePath = configService.SettingsDict["tinderApiTokenPath"].ToString();
            //yamlWorker.SerializeToFile(configFilePath, apiToken);
        }

        [TestMethod]
        public void Phase_03_SaveApiTokenBySelenium()
        {
            try
            {
                // arrange
                //var configData = yamlWorker.Deserialize<Dictionary<string, object>>(configFilePath);
                //var options = new FirefoxOptions();
                //options.BrowserExecutableLocation = configData["browserPath"].ToString();
                ////options.AddArguments("--headless");
                //options.AddArguments("-profile");
                //options.AddArguments(configData["profilePath"].ToString());

                //// act
                //var driver = new FirefoxDriver(configData["driverPath"].ToString(), options);
                //var sh = new SeleniumHelper(driver);
                //driver.Navigate().GoToUrl("https://tinder.com");
                //var apiToken = driver.ExecuteScript("return window.localStorage.getItem('TinderWeb/APIToken')");
                //driver.Close();

                //// save
                ////TestContext.WriteLine("apiToken: " + apiToken);
                //yamlWorker.SerializeToFile(configData["tinderApiTokenPath"].ToString(), apiToken);
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
            }
        }

        [TestMethod]
        public string Phase_04_GetProfileAndSave()
        {
            // arrange
            configService.SettingsDict.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));
            var httpRequester = new HttpRequester();
            var apiToken = ReadApiTokenFromFile();
            var organizationUri01 = "https://api.gotinder.com";
            var projectNameOrId01 = "profile";

            var headers = new Dictionary<string, string>
            {
                { "app_version", "6.9.4" },
                { "platform", "ios" },
                { "User-agent", "Tinder/7.5.3 (iPhone; iOS 10.3.2; Scale/2.00)" },
                { "X-Auth-Token", apiToken }
            };

            // arrange2
            var repoName = configService.SettingsDict["winderRepoName"].ToString();
            var repo = (repoName, "");
            var repoItem = repoService.Methods.GetItem(repo);
            var newAddress = repoService.Methods.CreateFolder(repo, "tinder");
            var newAddress2 = repoService.Methods.CreateFolder(newAddress, "exportedApiData");

            // act
            var jsonBodyObj01 = httpRequester.InvokeGet(
                organizationUri01,
                projectNameOrId01,
                default,
                headers);

            // save
            var profileJson = jsonBodyObj01.ToString();
            var profile = JsonConvert.DeserializeObject<Profile>(profileJson);
            var newAddress3 = CreateAccountAddress(newAddress2,
                profile.create_date,
                profile.name,
                profile._id);

            var yamlProfile = yamlWorker.Serialize(profile);
            repoService.Methods.CreateText(newAddress3, "profile", yamlProfile);
            return profile._id;
        }

        [TestMethod]
        public void Phase_05_GetMatchesAndSave()
        {
            // arrange
            var accoutId = Phase_04_GetProfileAndSave();
            var accountAddress = GetAccountAddress(accoutId);
            var httpRequester = new HttpRequester();
            var apiToken = ReadApiTokenFromFile();
            var organizationUri = "https://api.gotinder.com";
            var projectNameOrId = "updates";
            var repo = configService.SettingsDict["winderRepoName"].ToString();

            var headers = new Dictionary<string, string>
            {
                { "app_version", "6.9.4" },
                { "platform", "ios" },
                { "User-agent", "Tinder/7.5.3 (iPhone; iOS 10.3.2; Scale/2.00)" },
                { "X-Auth-Token", apiToken }
            };

            var body = new Dictionary<string, string>
            {
                { "last_activity_date", "" },
            };

            // act
            var jsonBodyObj01 = httpRequester.InvokePost(
                organizationUri,
                projectNameOrId,
                default,
                headers,
                default,
                "application/json",
                body);

            // save
            var matchesJson = jsonBodyObj01["matches"].ToString();

            List<Match>? matchesList = JsonConvert.DeserializeObject<List<Match>>(matchesJson);
            if (matchesList != null && matchesList.Count > 0)
            {
                //var accoutAddress = TryGetAccountId(matchesList.First());
                var nameQContentList = matchesList.Select(x => (x.id, yamlWorker.Serialize(x))).ToList();
                repoService.Methods.CreateManyText(accountAddress, nameQContentList);
            }
        }

        [TestMethod]
        public void Phase_06_GitRepoUpdate()
        {
            // assert
            var buildSourceDir = GetEnvironmentVariable("BUILD_SOURCESDIRECTORY");
            var repo = configService.SettingsDict["winderAppDataPath"].ToString();
            var repoPath = buildSourceDir + "/" + repo;
            Console.WriteLine($"repoPath: {repoPath}");
            //repoPath = repoService.Methods.GetRepoPath("01_projects");

            // act
            var password = configService.SettingsDict["azureDevopsToken"].ToString();
            var result = GitRepoCommitUpdate(repoPath);
            var result2 = GitPushWithPassword(repoPath, password);

            // print
            PrintPsObjects(result);
        }

        [TestMethod]
        public void Phase_07_TryReadApiData()
        {
            var repo = configService.SettingsDict["winderAppDataPath"].ToString();
            var loca = "01/03/04";
            var contentsList = repoService.Methods.GetManyText((repo, loca));
            var profile2 = yamlWorker.Deserialize<Profile>(contentsList.First());
            contentsList.RemoveAt(0);
            var matches = contentsList.Select(x => yamlWorker.Deserialize<Match>(x)).ToList();

            //var profile = yamlWorker.Deserialize<Profile2>(contentsList.First());
            //var matchYaml = contentsList.ElementAt(2);
            //var match2 = yamlWorker.DeserializeByJson<Match>(matchYaml);
        }

        [TestMethod]
        public void Phase_08_ExportConversationsAsText()
        {
            // arrange
            var appData = configService.SettingsDict["winderAppDataPath"].ToString();
            var mainAddress = (appData, "");
            var mainAddress2 = repoService.Methods.GetExistingItem(mainAddress, "tinder");
            var addressIn = repoService.Methods.GetPathsByName(
                mainAddress2, new List<string> { "exportedApiData" });
            var addressOut = repoService.Methods.CreateFolder(mainAddress2, "conversationAsTest");
            var accountNamesList = repoService.Methods.GetAllFoldersNames(addressIn);
            accountNamesList.Sort();

            
            foreach (var accountName in accountNamesList)
            {
                ExportConversationAsText(accountName, addressIn, addressOut);
            }

            // assert
            Console.WriteLine("Finish");
        }

        [TestMethod]
        public void Phase_08c_ExportLastAccountAsText()
        {
            // arrange
            var appData = configService.SettingsDict["winderRepoName"].ToString();
            var mainAddress = (appData, "");
            var mainAddress2 = repoService.Methods.GetExistingItem(mainAddress, "tinder");
            var addressIn = repoService.Methods.GetPathsByName(
                mainAddress2, new List<string> { "exportedApiData" });
            var addressOut = repoService.Methods.CreateFolder(mainAddress2, "conversationAsTest");
            var accountNamesList = repoService.Methods.GetAllFoldersNames(addressIn);
            accountNamesList.Sort();
            var lastAccount = accountNamesList.Last(); // accountNamesList.Last();
            ExportConversationAsText(lastAccount, addressIn, addressOut);
        }

        public void ExportConversationAsText(
            string accountName,
            (string, string) addressIn,
            (string, string) addressOut)
        {
            var contentsList = repoService.Methods.GetManyItemByName(
                addressIn, new List<string> { accountName });
            contentsList.RemoveAt(0);
            var matchesDictionaries = contentsList.Select(x => yamlWorker.Deserialize<Dictionary<object, object>>(x))
            .ToList();
            matchesDictionaries.ForEach(x => dictOperations.NewShape01(x));
            var myAccoutId = accountName.Substring(accountName.Length - 24, 24);

            var conversationList = convOperations.Worker01.GetAllConversationForTextFile(matchesDictionaries, myAccoutId);
            var conversationString = string.Join("\n", conversationList);
            var convName = convOperations.Worker01.GetConvName(matchesDictionaries[0]);
            repoService.Methods.CreateText(addressOut, accountName, conversationString);
        }

        [TestMethod]
        public void Phase_08_LoadEntities()
        {
            var repo = configService.SettingsDict["winderAppDataPath"].ToString();
            var loca = "01/03/04";
            var contentsList = repoService.Methods.GetManyText((repo, loca));
            var firstMatch = yamlWorker.Deserialize<Object>(contentsList.First());
            var firstMatch2 = yamlWorker.Deserialize<Match>(contentsList.First());
            //var firstMatch3 = yamlWorker.Deserialize<Match>(contentsList.First());
            //var profile2 = yamlWorker.DeserializeByJson<SharpTinderGoogleDocsProg.Entities2.Person>(contentsList.First());
            //var profile2 = yamlWorker.DeserializeByJson<SharpTinderGoogleDocsProg.Entities2.Match>(contentsList.First());
        }
    }
}