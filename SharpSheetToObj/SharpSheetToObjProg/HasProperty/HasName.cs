using SharpFileServiceProg.Service;

namespace SharpSheetToObjProg.HasProperty
{
    public class HasName : IHasName, IGetKeyFunc
    {
        public string Name { get; set; }

        public HasName(string name)
        {
            this.Name = name;
        }

        public Func<string> GetKeyFunc()
        {
            return () => Name;
        }

        public static bool HasProps<T>(IFileService fileService)
        {
            return fileService.Reflection.HasProp<T>("Name");
        }
    }
}
