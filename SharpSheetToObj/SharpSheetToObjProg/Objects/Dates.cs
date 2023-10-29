using CommonTypesCoreProj.Contracts;
using CSharpGameSynchProg.Contracts;
using CSharpGameSynchProg.Interfaces;

namespace CSharpGameSynchProg.Objects
{
    public class Dates : CommonObject, IHasDataProp
    {
        public string TookPlace { get; set; }
        public string ApproachId { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string Sex { get; set; }
        public string Date { get; set; }
    }
}
