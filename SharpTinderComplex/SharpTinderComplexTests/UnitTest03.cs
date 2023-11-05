using FileServiceCoreApp;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpTinderComplexTests.OldEntities;
using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder02 = SharpConfigProg.AAPublic.OutBorder;

namespace SharpTinderComplexTests
{
    [TestClass]
    public class UnitTest03 : UnitTest03Base
    {
        private readonly IFileService fileService;

        public UnitTest03()
        {
            fileService = OutBorder01.FileService();
        }

        [TestMethod]
        public void TestMethod3()
        {
            var exportedPath = "D:/01_Synchronized/01_Programming_Files/8c0f7763-7149-4b4d-9d6a-b28d3984552f/01_projects/PythonTinderApiDataExport/Output/ExportedApiData";
            var dirs = Directory.GetDirectories(exportedPath)
                .Select(x => Path.GetFileName(x))
                .ToList();
            dirs.RemoveAll(x => Path.GetFileName(x) == ".git");

            foreach (var dir in dirs)
            {
                var gg = dir.Split('_');
                GetAndSaveOldContent(gg[2], dir);
            }

            var repo = configService.SettingsDict["winderAppDataPath"].ToString();
            var localPath = repoService.Methods.GetRepoPath(repo);
        }

        public void GetAndSaveOldContent(string accoutId, string name)
        {
            // arrange
            //var accoutId = "62c89c9a4e95f00100eb6623";
            var repo = configService.SettingsDict["winderAppDataPath"].ToString();

            var newAddress = repoService.Methods.CreateFolder((repo, ""), "tinder");
            var newAddress2 = repoService.Methods.CreateFolder(newAddress, "exportedApiData2");
            var newAddress3 = repoService.Methods.CreateFolder(newAddress2, name);
            var namesAndContentsList = GetNamesAndContents(accoutId);

            // act
            repoService.Methods.CreateManyText(newAddress3, namesAndContentsList);

            // assert
        }

        public List<(string, string)> GetNamesAndContents(string accoutId)
        {
            string exportedApiDataFolderPath = "D:/01_Synchronized/01_Programming_Files/8c0f7763-7149-4b4d-9d6a-b28d3984552f/01_projects/PythonTinderApiDataExport/Output/ExportedApiData";
            var apiDataImporter = new OldApiDataImporter(exportedApiDataFolderPath);
            var matches = apiDataImporter.GetMatchesInfos(accoutId);
            var profile = apiDataImporter.GetProfile(accoutId);
            var yamlWorker = fileService.Yaml.Custom03;
            var namesAndContentsList = new List<(string _id, string)>();
            namesAndContentsList.AddRange(matches.Select(x =>
                (x._id, yamlWorker.Serialize(x)
            )).ToList());
            //int tmp2 = namesAndContentsList.FindIndex(x => x._id == accoutId);
            //var profileItem = namesAndContentsList[tmp2];
            //namesAndContentsList.RemoveAt(tmp2);
            namesAndContentsList.Insert(0, ("profile", yamlWorker.Serialize(profile)));

            return namesAndContentsList;
        }
    }
}