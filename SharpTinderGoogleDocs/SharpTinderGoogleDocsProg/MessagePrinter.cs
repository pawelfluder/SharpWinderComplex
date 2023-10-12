using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTinderGoogleDocsProg.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace TinderImport
{
    internal class ConversationPrinter
    {
        public List<string> GetMatchListOutput(List<Match> input)
        {
            var output = new List<string>();
            for (var i= 0; i < input.Count; i++)
            {
                var item = input[i];
                var header = i + " " + item.Person.Name + " " + item.Person.Age + " [" + item.Person.Id + "]";
                var text = GetMsgListOutput(item.Messages);
                output.Add(header);
                output.AddRange(text);
                output.Add(string.Empty);
            }

            return output;
        }

        public List<string> GetMatchListOutput2(List<Match> input)
        {
            var output = new List<string>();
            for (var i = 0; i < input.Count; i++)
            {
                var item = input[i];
                var header = i + " " + item.Person.Name + " " + item.Person.Age + " [" + item.Person.Id + "]";
                var text = GetFirstMsgOutput(item.Messages);
                //output.Add(header);
                output.AddRange(text);
                //output.Add(string.Empty);
            }

            return output;
        }

        public List<string> GetMatchListOutput3(List<Match> input)
        {
            var output = new List<string>();
            for (var i = 0; i < input.Count; i++)
            {
                
                var item = input[i];
                if (item.Messages.Count > 0)
                {
                    var text = GetMsgListOutput(item.Messages);
                    var tittle = item.Person.Id;
                    var text1 = GetMsgListOutput3(item.Messages, 4);
                    //output.Add(header);
                    output.Add(tittle);
                    output.AddRange(text1);
                    if (output.Last() != string.Empty)
                    {

                        output.Add(string.Empty);
                    }
                }
            }

            return output;
        }

        public List<string> GetMsgListOutput(List<Message> input)
        {
            var output = new List<string>();
            foreach (var item in input)
            {
                var text = item.OwnerDescription + ": " + item.Text;
                output.Add(text);
            }

            return output;
        }

        public List<string> GetMsgListOutput3(List<Message> input, int max)
        {
            var output = new List<string>();
            var gg = input.Where(x => x.OwnerDescription == "_Ja").ToList();
            var max2 = 0;
            if (max > gg.Count)
            {
                max2 = gg.Count;
            }
            else
            {
                max2 = max;
            }

            for (int i = 0; i < max2; i++)
            {
                var item = gg.ElementAt(i);
                    var text = item.Text;
                    output.Add(text);
            }

            return output;
        }

        public List<string> GetFirstMsgOutput(List<Message> input)
        {
            var output = new List<string>();
            var first = input.FirstOrDefault();
            if (first != null)
            {
                var text = input.First().Text;
                output.Add(text);
            }

            return output;
        }

        public void Print(List<string> input)
        {
            foreach (var item in input)
            {
                Console.WriteLine(item);
            }
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
