namespace SharpSheetToObjProg.CorrectnessCheck
{
    public static class CommonIdObjectListExtensions
    {
        //public static List<T> GetData<T>(this List<T> inputList) where T : CommonIdObject
        //{

        //}

        public static List<T> OrderById<T>(this List<T> inputList) where T : CommonIdObject
        {
            var result = inputList.OrderBy(x => x.Id).ToList();
            return result;
        }
    }
}
