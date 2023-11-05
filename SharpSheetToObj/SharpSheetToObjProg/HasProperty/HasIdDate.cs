using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSheetToObjProg.HasProperty
{
    public class HasId : IHasId
    {
        public string Id { get; set; }

        public HasId(string id)
        {
            this.Id = id;
        }
    }
}
