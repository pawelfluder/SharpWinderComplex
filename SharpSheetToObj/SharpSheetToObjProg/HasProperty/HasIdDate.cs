namespace SharpSheetToObjProg.HasProperty
{
    public class HasIdDate : IHasId, IGetKey
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

        public Func<string> GetKey()
        {
            return () => Id;
        }
    }
}
