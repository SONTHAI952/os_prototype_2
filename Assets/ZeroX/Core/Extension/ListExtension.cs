using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ZeroX.Extensions
{
    public static class ListExtension
    {
        public static void Shuffle(this IList l)
        {
            int count = l.Count;
            for (int i = 0; i < count; i++)
            {
                object objA = l[i];
                int newIndex = Random.Range(0, count);
                object objB = l[newIndex];
                l[i] = objB;
                l[newIndex] = objA;
            }
        }

        public static void Foreach<T>(this List<T> list, Action<int, T> action)
        {
            int index = 0;
            foreach (T item in list)
            {
                action(index, item);
                index++;
            }
        }

        
        /// <summary>
        /// Lấy ngẫu nhiên 1 phần tử, nhưng không thay đổi list
        /// </summary>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("List is empty");
            
            int index = Random.Range(0, list.Count);
            return list[index];
        }
        
        /// <summary>
        /// Lấy ngẫu nhiên 1 phần tử và xóa phần tử đó khỏi list
        /// </summary>
        public static T ReleaseRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("List is empty");
            
            int index = Random.Range(0, list.Count);
            T value = list[index];
            list.RemoveAt(index);
            return value;
        }
    }
}