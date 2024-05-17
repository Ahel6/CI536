using System.Collections;
using System.Collections.Generic;

public class ListStack<T> : IEnumerable<T>
{
	private List<T> _items = new();

	public int Count => _items.Count;

	public void Clear()
		=> _items.Clear();

	public void Push(T item)
	{
		_items.Add(item);
	}

	public T PopFromBack()
	{
		if (_items.Count == 0)
		{
			return default;
		}

		var firstElement = _items[0];
		_items.RemoveAt(0);
		return firstElement;
	}

	public bool Remove(T item)
		=> _items.Remove(item);

	public T this[int index]
	{
		get => _items[index];
		set => _items[index] = value;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
	public IEnumerator GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
}