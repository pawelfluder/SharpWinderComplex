using CommonTypesCoreProj.Contracts;

namespace CommonTypesCoreProj.Objects
{
   public class Messages : CommonObject
   {
      public string Repository { get; set; }
      public string PersistencyName { get; set; }
      public string LocalLink { get; set; }      
      public string ServerLink { get; set; }
   }
}
