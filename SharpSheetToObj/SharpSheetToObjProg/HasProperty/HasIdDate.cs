using SharpFileServiceProg.Service;

namespace SharpSheetToObjProg.HasProperty
{
    public class HasIdDate : IHasId, IGetKeyFunc
    {
        public string Id { get; set; }
        public string Date { get; set; }

        public HasIdDate(
            string id,
            string date)
        {
            this.Id = id;
            this.Date = date;
        }

        public Func<string> GetKeyFunc()
        {
            return () => Id;
        }

        public static bool HasProps<T>(IFileService fileService)
        {
            return fileService.Reflection.HasProp<T>("Id", "Date");
        }
    }
}
