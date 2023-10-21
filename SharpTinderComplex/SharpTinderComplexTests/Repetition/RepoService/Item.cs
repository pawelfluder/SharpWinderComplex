namespace SharpTinderComplexTests.Repetition.RepoService
{
    public record Item
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Body { get; set; }

        public string Address { get; set; }

        public (string, string) AdrTuple => AddressToTuple(Address);

        public Dictionary<string, object> Config { get; set; }

        private (string, string) AddressToTuple(string address)
        {
            var tmp = Address.Split('/');
            var repo = tmp[0];

            if (tmp.Length <= 1)
            {
                return (repo, "");
            }

            var loca = string.Join('/', tmp.Skip(1));

            return (repo, loca);
        }
    }
}
