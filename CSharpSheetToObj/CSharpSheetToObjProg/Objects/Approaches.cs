using CommonTypesCoreProj.Interfaces;
using CSharpSheetToObjProg.Contracts;

namespace CommonTypesCoreProj.Objects
{
    public class Approaches : CommonIdObject, IHasTypeProp, IHasDataProp
    {
        private char underscore = '_';
        private char emptyChar = 'x';
        private char space = ' ';

        public override string Id { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Attributes { get; set; }

        public string FileName()
        {
            var result = Who1() + Who2();
            return result;
        }

        public string Who1()
        {
            var result = Date + underscore + Type;
            return result;
        }

        public string Who2()
        {
            string result =
               Name != emptyChar.ToString() ? Name : string.Empty +
               Surname != emptyChar.ToString() ? Surname : string.Empty +
               Attributes != emptyChar.ToString() ? Attributes.Replace(space, underscore) : string.Empty;
            return result;
        }
    }
}
