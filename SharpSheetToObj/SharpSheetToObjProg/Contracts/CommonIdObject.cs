using CSharpGameSynchProg.Contracts;
using CSharpGameSynchProg.Interfaces;

namespace CommonTypesCoreProj.Contracts
{
    public abstract class CommonIdObject : CommonObject, IHasIdProp
    {
        public abstract string Id { get; set; }

        public static IList<IList<object>> ToIList(IList<CommonIdObject> inputList)
        {
            var result = inputList.Select(x => x.ToIList()).ToList();
            return result;
        }

        public static bool IsDataListCorrupted(List<CommonIdObject> objects)
        {
            var temp = objects.Select(x => (CommonObject)x).ToList();
            var corrupted = CommonObject.IsDataListCorrupted(temp);

            var ids = objects.Select(x => x.Id);
            var notDistinctIds = !(ids.Distinct().Count() == ids.Count());

            if (corrupted || notDistinctIds)
            {
                return true;
            }

            return false;
        }

        public IList<object> ToIList()
        {
            var properties = this.GetType().GetProperties();
            var result = new List<object>();
            var id = string.Empty;

            foreach (var property in properties)
            {
                var value = property.GetValue(this, null);

                if (property.Name == "Id")
                {
                    id = value.ToString();
                    continue;
                }

                result.Add(value);
            }

            result.Insert(0, id);

            return result;
        }
    }
}
