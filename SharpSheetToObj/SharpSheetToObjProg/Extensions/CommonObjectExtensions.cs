using CommonTypesCoreProj.Contracts;
using CSharpGameSynchProg.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpGameSynchProg.Extensions
{
    public static class CommonObjectExtensions
    {
        public static bool IsDataCorrupted<T>(List<T> commonObjectsList) where T : CommonObject
        {
            if (commonObjectsList == null)
            {
                return true;
            }

            var corrupted = commonObjectsList.Any(x => x.IsDataCorrupted());

            if (corrupted)
            {
                var corruptedData = commonObjectsList.Where(x => x.IsDataCorrupted());
            }

            return false;
        }
    }
}
