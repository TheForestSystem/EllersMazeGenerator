using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static IEnumerable<(int, T)> Enumerate<T>(
        this IEnumerable<T> input,
        int start = 0
    )
    {
        int i = start;
        foreach (var t in input)
            yield return (i++, t);
    }
}
