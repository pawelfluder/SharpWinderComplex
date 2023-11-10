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
    }
}
