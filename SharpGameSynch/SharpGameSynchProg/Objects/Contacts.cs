using CommonTypesCoreProj.Contracts;
using CSharpGameSynchProg.Contracts;

namespace CSharpGameSynchProg.Objects
{
    public class Contacts : CommonObject
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
    }
}
