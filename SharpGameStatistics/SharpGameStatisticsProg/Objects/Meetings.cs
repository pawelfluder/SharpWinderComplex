using CommonTypesCoreProj.Interfaces;
using CSharpSheetToObjProg.Contracts;

namespace CommonTypesCoreProj.Objects
{
    public class Meetings : CommonObject, IHasDataProp
   {								
      public string TookPlace { get; set; }
      public string Id { get; set; }
      public string T2 { get; set; }
      public string Num { get; set; }
      public string Sex { get; set; }
      public string Date { get; set; }
      public string Day { get; set; }
      public string FileName { get; set; }
      public string ApproachName { get; set; }
      public string Address { get; set; }
      public string Comment { get; set; }

      //public Meetings(
      //   string tookPlace,
      //   string t2,
      //   string num,
      //   string sex,
      //   string date,
      //   string day,
      //   string fileName,
      //   string approachName,
      //   string address,
      //   string comment)
      //{
      //   TookPlace = tookPlace;
      //   T2 =  t2;
      //   Num = num;
      //   Sex = sex;
      //   Date = date;
      //   Day = day;
      //   FileName = fileName;
      //   ApproachName = approachName;
      //   Address = address;
      //   Comment = comment;
      //}
   }
}
