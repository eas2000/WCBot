using Microsoft.Bot.Builder.Luis.Models;
using System.Linq;

namespace Microsoft.Bot.Builder.Luis
    {
        public static partial class Extensions
        {
        /// <summary>
        /// Finds by the parent type of the child entity
        /// </summary>
        /// <param name="result"></param>
        /// <param name="type"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
            public static bool TryFindEntityEx(this LuisResult result, string type, out EntityRecommendation entity)
            {
                entity = result.Entities?.FirstOrDefault(e => e.Type.StartsWith(type+"::"));
                return entity != null;
            }
            public static string GetParentType(this EntityRecommendation ent)
            {
                return ent.Type.Substring(0, ent.Type.IndexOf("::"));
            }
            public static string GetChildType(this EntityRecommendation ent)
            {
                return ent.Type.Substring(ent.Type.IndexOf("::") + 2);
            }
    }
  }

