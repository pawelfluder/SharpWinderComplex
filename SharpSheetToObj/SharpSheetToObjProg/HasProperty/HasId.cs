namespace SharpSheetToObjProg.HasProperty
{
    public class HasId : IHasId
    {
        public string Id { get; set; }

        public HasId(string id)
        {
            this.Id = id;
        }
    }
}
