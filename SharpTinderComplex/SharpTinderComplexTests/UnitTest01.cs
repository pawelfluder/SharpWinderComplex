using SharpFileServiceProg.AAPublic;
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
            var max = 5; var i = 0;
            Phase_02_SaveApiTokenBySqliteFile();
            Phase_04_GetProfileAndSave();
            while (string.IsNullOrEmpty(AccountItem?.Address)
                && i < max)
            {
                i++;
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