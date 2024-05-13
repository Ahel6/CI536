using System.Collections.Generic;
using System;

public static class Extensions
{
	public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		var comparer = Comparer<TKey>.Default;
		var yk = default(TKey);
		var y = default(TSource);
		var hasValue = false;
		foreach (var x in source)
		{
			var xk = keySelector(x);
			if (!hasValue)
			{
				hasValue = true;
				yk = xk;
				y = x;
			}
			else if (comparer.Compare(xk, yk) > 0)
			{
				yk = xk;
				y = x;
			}
		}

		if (!hasValue)
		{
			throw new InvalidOperationException("Sequence contains no elements");
		}

		return y;
	}
}