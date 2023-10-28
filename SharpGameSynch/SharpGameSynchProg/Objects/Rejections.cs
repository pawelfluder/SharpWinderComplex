using CommonTypesCoreProj.Contracts;
using CSharpGameSynchProg.Contracts;
using CSharpGameSynchProg.Interfaces;

namespace CSharpGameSynchProg.Objects
{
    public class Rejections : CommonObject, IHasCountProp, IHasDataProp
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Count { get; set; }
    }
}
