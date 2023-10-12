using GoogleDocsServiceProj;
using GoogleDocsServiceProj.Service;
using GoogleDriveCoreApp;
using SharpGoogleDriveProg.Service;

namespace TinderImport.Repetition
{
    internal class Border
    {
        public static GoogleDocsService GetGoogleDocsService(string clientId, string clientSecret)
        {
            var aplicationName = "";
            var scopes = new List<string>();
            var googleDocsService = new GoogleDocsService(clientId, clientSecret, aplicationName, scopes);
            return googleDocsService;
        }

        public static GoogleDriveService GetGoogleDriveService(string clientId, string clientSecret)
        {
            var googleDriveService = new GoogleDriveService(clientId, clientSecret);
            return googleDriveService;
        }

        //public static FileService GetFileService()
        //{
        //    var fileService = new FileService();
        //    return fileService;
        //}
    }
}
