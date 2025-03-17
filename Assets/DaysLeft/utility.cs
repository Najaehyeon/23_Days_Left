using System;
using System.Collections.Generic;

public static class Utility
{
    public static T GetAndRemoveAt<T>(this List<T> list, int index)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("List is empty or null.");
        }

        T item = list[index];
        list.RemoveAt(index);
        return item;
    }
}
