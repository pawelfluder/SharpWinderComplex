using CSharpGameSynchProg.Register;
using FileServiceCoreApp;
using SharpFileServiceProg.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSynchCoreProj
{
    internal class MessagesWorker
    {
        public IFileService fileService;

        public MessagesWorker()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
        }

        public List<(string, string, string)> GetAllMessages()
        {
            var all = new List<(string, string, string)>();

            all.AddRange(AddFromRepo("MsgWinder"));
            //all.AddRange(AddFromRepo("MsgPhone"));
            //all.AddRange(AddFromRepo("MsgFacebook"));
            //all.AddRange(AddFromRepo("MsgInstagram"));
            //all.AddRange(AddFromRepo("MsgWhatsup"));

            return all;
        }


        private IEnumerable<(string, string, string)> AddFromRepo(string repoName)
        {
            var all = new List<(string, string, string)>();

            //var repo1 = (new Guid("ebf8d4ba-06c2-43eb-a201-4d32d13656e4"), repoName);
            var closed1 = (repoName, "01");
            var open1 = (repoName, "02");
            var open2 = (repoName, "03");
            all.AddRange(GetAllSubItems(closed1));
            all.AddRange(GetAllSubItems(open1));

            if (repoName == "MsgWinder")
            {
                all.AddRange(GetAllSubItems(open2));
            }

            return all;
        }

        private IEnumerable<(string, string, string)> GetAllSubItems(
            (string Repo, string Loca) adrTuple)
        {
            //var subAddresses = fileService.GetAllSubAddress(adrTuple);
            //var nameQRelativeAddressQRepoName = subAddresses.Select(x => fileService.GetNameQRelativeAddressQRepoName(x));
            //return nameQRelativeAddressQRepoName;
            return default;
        }
    }
}
