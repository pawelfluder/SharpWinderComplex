using FileServiceCoreApp;
using SharpConfigProg.Service;
using SharpFileServiceProg.Operations.Conversations;
using SharpFileServiceProg.Operations.Dictionaries;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpTinderComplexTests.JsonObjects;
using SharpTinderComplexTests.Repetition;
using SharpTinderComplexTests.Repetition.Names;
using SharpTinderComplexTests.Repetition.RepoService;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Net.Mail;
using Unity;
using OutBorder01 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder02 = SharpConfigProg.Repetition.OutBorder;
using OutBorder03 = SharpRepoServiceProg.Repetition.OutBorder;

namespace SharpTinderComplexTests
{
    public class UnitTest02Base
    {
        protected readonly string configFilePath;
        protected readonly IFileService.IYamlOperations yamlWorker;
        protected readonly DictionaryOperations dictOperations;
        protected readonly ConvOperations convOperations;
        protected readonly IRepoService repoService;
        private readonly IFileService fileService;
        protected readonly IConfigService configService;

        protected Item RepoItem { get; private set; }
        protected Item WinderItem { get; private set; }
        protected Item ApiDataItem { get; private set; }
        protected Item TextConversationsItem { get; private set; }
        protected Item AccountItem { get; set; }

        protected UnitTest02Base()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            configService = MyBorder.Container.Resolve<IConfigService>();
            string gg = configService.ConfigFilePath;
            configFilePath = configService.ConfigFilePath;
            yamlWorker = fileService.Yaml.Custom03;
            //LoadConfigData();
            //configService.Load();

            
            dictOperations = new DictionaryOperations();
            convOperations = new ConvOperations();
            repoService = MyBorder.Container.Resolve<IRepoService>();
            //repoService.Methods.InitializeSearchFoldersPaths();
            yamlWorker = fileService.Yaml.Custom03;
            SetItems();
        }

        protected void CreateWorkspace(string path)
        {
            var pathsList = new List<string>() { path };
            var xmlWorker = new XmlWorker(fileService);
            var path2 = path + "/" + "workspace.txt";
            xmlWorker.Load(path2);
            //xmlDocument.Save();
        }

        private void SetItems()
        {
            var repoName = configService.SettingsDict["winderRepoName"].ToString();
            var repoAdrTuple = (repoName, "");
            RepoItem = repoService.GetItem(repoAdrTuple);

            var winderAdrTuple = repoService.Methods.CreateFolder(repoAdrTuple, FolderNames.Winder);
            WinderItem = repoService.GetItem(winderAdrTuple);

            var apiDataAdrTuple = repoService.Methods.CreateFolder(winderAdrTuple, FolderNames.ExportedApiData);
            ApiDataItem = repoService.GetItem(apiDataAdrTuple);

            var conversationsAsTextAdrTuple = repoService.Methods.CreateFolder(winderAdrTuple, FolderNames.ConversationsAsText);
            TextConversationsItem = repoService.GetItem(conversationsAsTextAdrTuple);
        }

        protected List<string> GetRepoRootPaths()
        {
            var repoRootPaths = new List<string>();
            if (File.Exists(configFilePath) &&
                configService.SettingsDict.TryGetValue("repoRootPaths", out var tmp))
            {
                var tmp2 = ((List<object>)tmp);
                repoRootPaths = tmp2.Select(x => x.ToString()).ToList();
            }
            return repoRootPaths;
        }

        protected List<PSObject> GitPushWithPassword(string repoPath, string password)
        {
            var cmd1 = "git remote -v";
            var results = new List<PSObject>();
            var cmdList = new List<string> { cmd1 };
            results.AddRange(InvokePsCommandList(cmdList));
            cmdList.Clear();

            //git clone https://username:password@myrepository.biz/file.git
            //https://MvpProjects@dev.azure.com/MvpProjects/FirstMvp/_git/01_projects
            //git push https://username:password@myrepository.biz/file.git --all
            //https://www.geeksforgeeks.org/how-to-set-git-username-and-password-in-gitbash

            var cmd2 = "git push;";
            cmdList = new List<string> { cmd1 };
            results.AddRange(InvokePsCommandList(cmdList));
            return results;
        }

        protected List<PSObject> GitRepoCommitUpdate(string repoPath)
        {
            var cmd1 = $"cd {repoPath};";
            var cmd2 = "git config --global user.email \"buildservice@mvpprojects.com\";";
            var cmd3 = "git config --global user.name \"Build Service\";";
            var cmd4 = "git checkout main;";
            var cmd5 = "git add .;";
            var cmd6 = "git commit -m \"update\";";
            var cmd7 = "git push;" ;
            var cmd8 = "git status;";

            var cmdList = new List<string>
            {
                cmd1,
                cmd2,
                cmd3,
                cmd4,
                cmd5,
                cmd6,
                cmd8,
            };
            var result = InvokePsCommandList(cmdList);
            //cmdList = cmdList.Select(x => x = cmd1 + x).ToList();
            //cmdList.ForEach(x => InvokePsCommand(x));

            return result;
        }

        protected void PrintPsObjects(List<PSObject> psObjects)
        {
            var tmp1 = psObjects.Select(x => x.BaseObject).ToList();
            //var tmp2 = tmp1.Select(x => x.Value);
            var result = string.Join("\n", tmp1);
            Console.WriteLine("PsObjects:");
            Console.WriteLine(result);
        }

        protected List<PSObject> InvokePsCommandList(List<string> cmdList)
        {
            var ps = PowerShell.Create();
            var results = new List<PSObject>();
            foreach (var cmd in cmdList)
            {
                ps.AddScript(cmd);
                var result = ps.Invoke();
                results.AddRange(result);
            }
            ps.Dispose();
            return results;
        }

        protected Collection<PSObject> InvokePsCommand(string cmd)
        {
            var ps = PowerShell.Create();
            ps.AddScript(cmd);
            var result = ps.Invoke();
            ps.Dispose();
            return result;
        }

        protected string GetEnvironmentVariable(string varName)
        {
            var cmd = $"Get-Item -Path Env:{varName}";
            var ps = PowerShell.Create();
            ps.AddScript(cmd);
            var result = ps.Invoke();
            var tmp1 = result.SelectMany(x => x.Properties).ToList();
            var tmp2 = tmp1.Select(x => x.Value.ToString());
            var buildPath = string.Join(" ", tmp2);
            var buildPath2 = string.Empty;
            if (buildPath.Count() > 0)
            {
                buildPath2 = tmp2.Last().Replace('\\', '/');
            }
            
            ps.Dispose();
            Console.WriteLine("Build.SourcesDirectory: " + buildPath);
            Console.WriteLine("Build.SourcesDirectory2: " + buildPath2);
            return buildPath2;
        }

        protected string ReadApiTokenFromSettings()
        {
            var apiToken = configService.SettingsDict["tinderApiToken"].ToString();
            return apiToken;
        }

        //protected string ReadApiTokenFromFile()
        //{
        //    var path = configService.SettingsDict["tinderApiTokenPath"].ToString();
        //    var token = yamlWorker.DeserializeFile(path).ToString();
        //    return token;
        //}

        protected string DateTimeToString(DateTime date)
        {
            var us = "-";
            var tmp =
                date.Year.ToString().Substring(2,2) + us +
                IndexToString(date.Month) + us +
                IndexToString(date.Day);
            return tmp;
        }

        protected string GetAccountName(
            DateTime dataCreated,
            string profileName,
            string accountId)
        {
            var date = DateTimeToString(dataCreated);
            var accountName = date + "_" + profileName.ToLower() + "_" + accountId;
            return accountName;
        }

        protected (string, string) CreateAccountAddress(
            string accountName,
            string accountId)
        {
            var tmp = TryGetAccountAddress(accountId);
            if (tmp != default)
            {
                return tmp;
            }

            var accountAddress = repoService.Methods.CreateFolder(ApiDataItem.AdrTuple, accountName);
            return accountAddress;
        }

        protected (string, string) CreateAccountAddress(
            (string, string) parentAddress,
            DateTime dataCreated,
            string profileName,
            string accountId)
        {
            var date = DateTimeToString(dataCreated);
            var name = date + "_" + profileName.ToLower() + "_" + accountId;

            var tmp = TryGetAccountAddress(accountId);
            if (tmp != default)
            {
                return tmp;
            }

            var accountAddress = repoService.Methods.CreateFolder(parentAddress, name);
            return accountAddress;
        }

        //protected (string, string) GetAccountAddress(string accountId)
        //{
        //    var address = TryGetAccountAddress(accountId);
        //    if (address == default)
        //    {
        //        throw new Exception();
        //    }
            
        //    return address;
        //}

        private (string, string) TryGetAccountAddress(string accountId)
        {
            var tmp = new List<string>();

            try
            {
                var accountNames = repoService.Methods.GetAllFoldersNames(ApiDataItem.AdrTuple);
                tmp = accountNames.Where(x => x.EndsWith(accountId)).ToList();

                if (tmp.Count() == 1)
                {
                    var name = tmp.ElementAt(0);
                    var accountAddress = repoService.Methods.GetExistingItem(ApiDataItem.AdrTuple, name);
                    return accountAddress;
                }
            }
            catch
            {}

            if (tmp.Count > 1) { throw new Exception(); }

            return default;
        }

        protected (string, string) TryGetAccountId(Match match)
        {
            var repo = configService.SettingsDict["winderAppDataPath"].ToString();
            string matchId = match.id;
            var guid1 = matchId.Substring(0, 24);
            var guid2 = matchId.Substring(23, 24);
            
            var newAddress1 = repoService.Methods.CreateFolder((repo, ""), "tinder");
            var newAddress2 = repoService.Methods.CreateFolder(newAddress1, "exportedApiData");

            var accountNames = repoService.Methods.GetAllFoldersNames(newAddress2);
            var gg1 = accountNames.SingleOrDefault(x => x.EndsWith(guid1));
            var gg2 = accountNames.SingleOrDefault(x => x.EndsWith(guid2));

            var accoutAddress = repoService.Methods.GetExistingItem(newAddress2, gg1);
            if (accoutAddress == default)
            {
                accoutAddress = repoService.Methods.GetExistingItem(newAddress2, gg2);
            }

            return accoutAddress;
        }

        private string IndexToString(int index)
        {
            if (index < 10)
            {
                return "0" + index;
            }
            if (index < 100)
            {
                return index.ToString();
            }

            throw new Exception();
        }
    }
}