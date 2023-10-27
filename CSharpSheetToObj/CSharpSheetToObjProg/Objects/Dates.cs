using CommonTypesCoreProj.Contracts;
using CommonTypesCoreProj.Interfaces;

namespace CommonTypesCoreProj.Objects
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
