using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpSheetToObjProg.Contracts
{
    public class CommonObject
    {
        private static void PrintCorruptedData(IEnumerable<CommonObject> corruptedDatasList)
        {
            foreach (var corruptedData in corruptedDatasList)
            {
                Console.WriteLine(corruptedData);
            }
        }

        public static IList<IList<object>> ToIList<T>(IList<T> inputList) where T : CommonObject
        {
            var temp = inputList.Select(x => (CommonObject)x).ToList();
            var result = temp.Select(x => x.ChangeToIList()).ToList();
            return result;
        }

        public static bool IsDataListCorrupted(List<CommonObject> objects)
        {
            foreach (var obj in objects)
            {
                if (obj.IsDataCorrupted())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsDataCorrupted()
        {
            var properties = GetType().GetProperties();
            var values = properties.Select(x => x.GetValue(this, null));

            var isEmptyRow = !values.Any(x => x != null);

            if (isEmptyRow)
            {
                foreach (var property in properties)
                {
                    property.SetValue(this, string.Empty);
                }

                return false;
            }

            foreach (var value in values)
            {
                if (value == string.Empty || value == null)
                {
                    return true;
                }
            }

            return false;
        }

        public static IList<IList<object>> ToIList(IList<CommonObject> inputList)
        {
            var result = inputList.Select(x => x.ChangeToIList()).ToList();
            return result;
        }

        public override bool Equals(object obj)
        {
            var thisTuples = GetTuples(this);
            var objTuples = GetTuples(obj);

            if (thisTuples.Length != objTuples.Length)
            {
                return false;
            }

            for (int i = 0; i < thisTuples.Length; i++)
            {
                if (thisTuples[i] != objTuples[i])
                {
                    return false;
                }
            }

            return true;
        }

        public (string, string)[] GetTuples(object obj)
        {
            var properties = GetType().GetProperties();
            var tuples = properties.Select(x => (x.Name, x.GetValue(this).ToString())).ToArray();
            return tuples;
        }

        public IList<object> ChangeToIList()
        {
            var thisTuples = GetTuples(this);
            IList<object> valuesList = thisTuples.Select(x => (object)x.Item2).ToList();
            return valuesList;
        }

        public T IListToThis<T>(IList<object> input) where T : CommonObject, new()
        {
            var newObj = new T();
            var temp = input.Select(x => x.ToString()).ToArray();
            newObj.Initialize(temp);
            return newObj;
        }

        public void Initialize(string[] inputProperties)
        {
            var properties = GetType().GetProperties();
            if (properties.Count() != inputProperties.Length)
            {
                throw new Exception();
            }

            for (int i = 0; i < inputProperties.Length; i++)
            {
                properties.SetValue(inputProperties[i], i);
            }
        }
    }
}
