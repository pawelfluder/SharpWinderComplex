using SharpFileServiceProg.Repetition;
using SharpFileServiceProg.Service;

namespace SharpTinderComplexTests
{
    [TestClass]
    public class UnitTest01 : UnitTest01Base
    {
        private readonly UnitTest02 unitTest02;
        private readonly IFileService fileService;

        public UnitTest01()
        {
            unitTest02 = new UnitTest02();
            fileService = OutBorder.FileService();
        }

        [TestMethod]
        public void RecreateWorkspace()
        {
            var repo = (configService.SettingsDict["winderRepoName"].ToString(), "");
            var filePath = repoService.Methods.GetElemPath(repo);
            CreateWorkspace(filePath);
        }

        [TestMethod]
        public void SaveTinderData()
        {
            unitTest02.Phase_02_SaveApiTokenBySqliteFile();
            unitTest02.Phase_05_GetMatchesAndSave();
            unitTest02.Phase_08c_ExportLastAccountAsText();
            RecreateWorkspace();
        }
    }
}