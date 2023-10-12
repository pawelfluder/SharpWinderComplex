using SharpFileServiceProg.Service;
using OutBorder01 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder02 = SharpConfigProg.Repetition.OutBorder;

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
            fileService = OutBorder01.FileService();
        }

        [TestMethod]
        public void RecreateWorkspace()
        {
            var filePath = configService.SettingsDict["appDataPath"].ToString();
            CreateWorkspace(filePath);
        }

        [TestMethod]
        public void SaveTinderData()
        {
            //winderAppDataPath
            
            unitTest02.Phase_02_SaveApiTokenBySqliteFile();
            unitTest02.Phase_05_GetMatchesAndSave();
            unitTest02.Phase_08c_ExportLastAccountAsText();
            RecreateWorkspace();
        }
    }
}