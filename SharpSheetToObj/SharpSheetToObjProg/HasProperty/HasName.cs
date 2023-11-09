namespace SharpSheetToObjProg.HasProperty
{
    public class HasName : IHasName
    {
        public string Name { get; set; }

        public HasName(string name)
        {
            this.Name = name;
        }
    }
}
