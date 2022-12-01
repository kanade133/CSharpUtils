using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    static class LinqExtension
    {
        /// <summary>
        /// 可用keySelector方式进行去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<TSource, TKey>(keySelector));
        }
        /// <summary>
        /// 可用keySelector方式获取最大
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static TSource MaxElement<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey :IComparable
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("序列未包含任何元素");
            }
            TKey maxValue = keySelector(source.First());
            TSource maxElement = source.First();
            foreach (var item in source)
            {
                var value = keySelector(item);
                if (value.CompareTo(maxValue) > 0)
                {
                    maxValue = value;
                    maxElement = item;
                }
            }
            return maxElement;
        }
        /// <summary>
        /// 可用keySelector方式获取最小
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static TSource MinElement<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("序列未包含任何元素");
            }
            TKey minValue = keySelector(source.First());
            TSource minElement = source.First();
            foreach (var item in source)
            {
                var value = keySelector(item);
                if (value.CompareTo(minValue) < 0)
                {
                    minValue = value;
                    minElement = item;
                }
            }
            return minElement;
        }
        /// <summary>
        /// 改动原列表的方式对ObservableCollection排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sortedList = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sortedList.Count(); i++)
            {
                collection.Move(collection.IndexOf(sortedList[i]), i);
            }
        }
    }
}
