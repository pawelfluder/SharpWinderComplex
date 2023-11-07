using SharpCryptoCalcProg.Register;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;

namespace SharpCryptoCalcProg.Info
{
    internal class SheetInfoCacheHelper
    {
        private readonly IGoogleDriveService googleDriveService;
        private readonly IGoogleSheetService googleSheetService;

        public SheetInfoCacheHelper()
        {
            googleDriveService = MyBorder.Container.Resolve<IGoogleDriveService>();
            googleSheetService = MyBorder.Container.Resolve<IGoogleSheetService>();
        }

        public (string Id, string Name) GetFileByName(string fileName)
        {
            var result = googleDriveService.Worker
                .GetFileByName(fileName);

            return result;
        }

        public string GetSheetId(string spreadSheetId, string spreadSheetName)
        {
            var sheetId = googleSheetService.Worker.GetSheetId(spreadSheetId, spreadSheetName);
            return sheetId.ToString();
        }
    }
}
