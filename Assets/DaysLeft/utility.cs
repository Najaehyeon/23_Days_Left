using System;
using System.Collections.Generic;

public static class Utility
{
    public static T TryGetAndRemoveAt<T>(this List<T> list, int index)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }

        T item = list[index];
        list.RemoveAt(index);
        return item;
    }
}
