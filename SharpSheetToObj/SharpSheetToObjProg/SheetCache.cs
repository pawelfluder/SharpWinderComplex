using CSharpGameSynchProg.Register;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleDriveProg.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SharpSheetToObjProg
{
    internal class SheetCache
    {
        private readonly IGoogleDriveService googleDriveService;

        public SheetCache()
        {
            googleDriveService = MyBorder.Container.Resolve<IGoogleDriveService>();
        }

        public List<(string Id, string Name)> GetCache()
        {
            var gg = googleDriveService.Worker
                .GetFileByName("2022_CryptoTransactions");

            var result = new List<(string Id, string Name)>();
            result.Add(gg);
            return result;
        }
    }
}
