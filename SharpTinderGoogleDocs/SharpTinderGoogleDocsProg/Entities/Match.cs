namespace SharpTinderGoogleDocsProg.Entities
{
    public class Match
    {
        public Person Person { get; private set; }
        public List<Message> Messages { get; private set; }

        public Match(Person person, List<Message> messages)
        {
            Person = person;
            Messages = messages;
        }
    }
}
