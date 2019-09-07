using PostSharp.Patterns.Collections;
using System;
using System.Linq;

namespace Sit
{
    public static class Extensions
    {
        public static void MySort<TSource, TKey>(this AdvisableCollection<TSource> observableCollection, Func<TSource, TKey> keySelector)
        {
            var a = observableCollection.OrderBy(keySelector).ToList();
            observableCollection.Clear();
            foreach (var b in a)
            {
                observableCollection.Add(b);
            }
        }
    }
}
