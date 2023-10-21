using SharpFileServiceProg.Repetition;
using SharpFileServiceProg.Service;

namespace SharpTinderComplexTests
{
    [TestClass]
    public class UnitTest01 : UnitTest02
    {
        private readonly IFileService fileService;

        public UnitTest01()
        {
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
            Phase_02_SaveApiTokenBySqliteFile();
            Phase_04_GetProfileAndSave();
            if (string.IsNullOrEmpty(AccountItem.Address))
            {
                Phase_01_OpenBrowserOnPage();
                Phase_02_SaveApiTokenBySqliteFile();
                Phase_04_GetProfileAndSave();
            }

            Phase_05_GetMatchesAndSave();
            Phase_xx_ExportAccountAsText();
            //RecreateWorkspace();
        }
    }
}