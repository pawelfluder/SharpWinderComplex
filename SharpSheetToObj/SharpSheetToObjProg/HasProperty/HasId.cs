namespace SharpSheetToObjProg.HasProperty
{
    public class HasId : IHasId, IGetKey
    {
        public string Id { get; set; }

        public HasId(string id)
        {
            this.Id = id;
        }

        public Func<string> GetKey()
        {
            return () => Id;
        }
    }
}
