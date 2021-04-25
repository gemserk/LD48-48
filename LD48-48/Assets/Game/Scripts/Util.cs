using System.Collections.Generic;

namespace UnityTemplateProjects
{
    public static class Util
    {
        public static T RandomItem<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static void RemoveAsBag<T>(this List<T> list, T item)
        {
            int indexOf = list.IndexOf(item);
            if (indexOf == -1)
                return;

            var lastIndex = list.Count - 1;

            list[indexOf] = list[lastIndex];
            list.RemoveAt(lastIndex);
        }

        public static T RemoveAsBagWithIndex<T>(this List<T> list, int indexOf)
        {
            var lastIndex = list.Count - 1;

            var item = list[indexOf];
            list[indexOf] = list[lastIndex];
            list.RemoveAt(lastIndex);

            return item;
        }
    }
}