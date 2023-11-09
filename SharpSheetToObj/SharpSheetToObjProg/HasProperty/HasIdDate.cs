namespace SharpSheetToObjProg.HasProperty
{
    public class T2 : IHasId
    {
        public string Id { get; set; }
        public string Date { get; set; }

        public T2(
            string id,
            string date)
        {
            this.Id = id;
            this.Date = date;
        }
    }
}
