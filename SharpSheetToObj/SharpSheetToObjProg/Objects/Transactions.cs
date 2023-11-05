using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSheetToObjProg.Objects
{
    internal class Transactions
    {
        public string Link { get; set; }

        public string Id { get; set; }

        public string Date { get; set; }

        public string Hour { get; set; }

        public string From { get; set; }

        public string FromSend { get; set; }

        public string FromCurrency { get; set; }

        public string To { get; set; }

        public string ToReceived { get; set; }

        public string ToCurrency { get; set; }
    }
}
