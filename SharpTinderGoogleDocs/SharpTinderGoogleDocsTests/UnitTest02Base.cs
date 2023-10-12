using SharpConfigProg.Service;
using SharpFileServiceProg.Operations.Conversations;
using SharpFileServiceProg.Service;
using SharpGoogleDriveProg.Service;
using SharpRepoServiceProg.Service;
using SharpTinderComplexTests.Repetition;
using System.Collections.ObjectModel;
using System.Management.Automation;
using TinderImport.Repetition;
using Unity;

namespace SharpTinderGoogleDocsTests
{
    public class UnitTest02Base
    {
        private readonly IFileService fileService;
        protected readonly ConvOperations convOperations;
        protected readonly IConfigService configService;
        protected readonly string configFilePath;
        protected Dictionary<string, object> configData { get; }
        protected readonly IFileService.IYamlOperations yamlWorker;
        protected readonly IRepoService repoService;
        protected readonly GoogleDriveService driveService;

        protected UnitTest02Base()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            convOperations = new ConvOperations();
            configService = MyBorder.Container.Resolve<IConfigService>();
            configFilePath = configService.ConfigFilePath;
            var credentialWorker = new CredentialWorker();
            var credentials = credentialWorker.GetCredentials();
            yamlWorker = fileService.Yaml.Custom03;
            configData = new Dictionary<string, object>();
            driveService = new GoogleDriveService(credentials.clientId, credentials.clientSecret);
            if (File.Exists(configFilePath))
            {
                configData = yamlWorker.DeserializeFile<Dictionary<string, object>>(configFilePath);
            }
            var repoRootPaths = new List<string>();
            if (File.Exists(configFilePath) &&
                configData.TryGetValue("repoRootPaths", out var tmp))
            {
                var tmp2 = (List<object>)tmp;
                repoRootPaths = tmp2.Select(x => x.ToString()).ToList();
            }

            repoService = MyBorder.Container.Resolve<IRepoService>(); 
            //repoService.Initialize(repoRootPaths);
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
            var cmd7 = "git push;";
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

        protected string ReadApiTokenFromFile()
        {
            var path = configData["tinderApiTokenPath"].ToString();
            var token = yamlWorker.DeserializeFile<string>(path);
            return token;
        }

        protected string DateTimeToString(DateTime date)
        {
            var us = "-";
            var tmp =
                date.Year.ToString().Substring(2, 2) + us +
                IndexToString(date.Month) + us +
                IndexToString(date.Day);
            return tmp;
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

        protected (string id, string name) CreateNewDocFile(string fileName)
        {
            var folder2023 = "13gY7OdaPCMwHQKmJZWZpcou7xtMrxNlg";
            driveService.Initialize();
            var result = driveService.Worker.CreateNewDocFile(folder2023, fileName);
            return result;
        }
    }
}