using CSharpSheetToObjProg.Repet;
using CSharpSheetToObjProg.Repet.Models;
using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;

namespace CommonTypesCoreProj.Extensions
{
    public static class RepoServiceExtensions
    {
        public static (string Repo, string Loca) GetAdrTuple<T>(
            this IRepoService repoService,
            (string Repo, string Loca) mainAdrTuple)
        {
            var name = typeof(T).Name;
            var adrTuple = repoService.Methods.GetAdrTupleByName(mainAdrTuple, name);
            return adrTuple;
        }

            public static T GetItem<T>(
            this IRepoService repoService,
            (string Repo, string Loca) adrTuple)
        {
            var jsonObj = repoService.Methods.GetItem(adrTuple);
            var item = JsonConvert.DeserializeObject<T>(jsonObj);
            return item;
        }

        public static List<T> GetItemList<T>(
            this IRepoService repoService,
            (string Repo, string Loca) adrTuple)
        {
            var jsonString = repoService.Methods.GetItem(adrTuple);
            var item = JsonConvert.DeserializeObject<ItemModel2>(jsonString);
            var body = item.Body.ToString();
            var fileService = MyBorder.Container.Resolve<IFileService>();
            var itemList = fileService.Yaml.Custom03.Deserialize<List<T>>(body);
            return itemList;
        }
    }
}
