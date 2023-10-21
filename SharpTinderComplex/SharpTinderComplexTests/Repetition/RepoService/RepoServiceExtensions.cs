using Newtonsoft.Json;
using SharpRepoServiceProg.Service;

namespace SharpTinderComplexTests.Repetition.RepoService
{
    internal static class RepoServiceExtensions
    {
        public static Item GetItem(
            this IRepoService repoService,
            (string, string) address)
        {
            var jsonItem = repoService.Methods.GetItem(address);
            var item = JsonConvert.DeserializeObject<Item>(jsonItem);
            return item;
        }
    }
}
