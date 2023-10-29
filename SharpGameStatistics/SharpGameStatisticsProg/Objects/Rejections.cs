using CommonTypesCoreProj.Interfaces;
using CSharpSheetToObjProg.Contracts;

namespace CommonTypesCoreProj.Objects
{
    public class Rejections : CommonObject, IHasCountProp, IHasDataProp
   {
      public string Date { get; set; }
      public string Type { get; set; }
      public string Count { get; set; }
   }
}
