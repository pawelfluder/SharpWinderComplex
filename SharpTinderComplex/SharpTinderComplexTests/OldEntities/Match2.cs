namespace SharpTinderComplexTests.OldEntities
{
    internal class Match2
    {
        public Person2 Person { get; private set; }
        public List<Message2> Messages { get; private set; }

        public Match2(Person2 person, List<Message2> messages)
        {
            Person = person;
            Messages = messages;
        }
    }
}
