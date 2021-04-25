using System.Collections.Generic;

namespace UnityTemplateProjects
{
    public static class Util
    {
        public static T RandomItem<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}