using CommonTypesCoreProj.Contracts;
using CommonTypesCoreProj.Interfaces;

namespace CommonTypesCoreProj.Objects
{
    public class Rejections : CommonObject, IHasCountProp, IHasDataProp
   {
      public string Date { get; set; }
      public string Type { get; set; }
      public string Count { get; set; }
   }
}
