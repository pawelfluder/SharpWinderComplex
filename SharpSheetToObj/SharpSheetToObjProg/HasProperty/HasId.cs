using SharpFileServiceProg.Service;

namespace SharpSheetToObjProg.HasProperty
{
    public class HasId : IHasId, IGetKeyFunc
    {
        public string Id { get; set; }

        public HasId(string id)
        {
            this.Id = id;
        }

        public Func<string> GetKeyFunc()
        {
            return () => Id;
        }

        public static bool HasProps<T>(IFileService fileService)
        {
            return fileService.Reflection.HasProp<T>("Id");
        }
    }
}
