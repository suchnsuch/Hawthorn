namespace Hawthorn;

public class Blackboard
{
	Dictionary<string, object> Values = new Dictionary<string, object>();

	public object Get(string key)
	{
		if (Values.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	public bool TryGet<T>(string key, out T value)
	{
		if (Values.TryGetValue(key, out var v))
		{
			if (v is T typedValue)
			{
				value = typedValue;
				return true;
			}
		}
		value = default(T);
		return false;
	}

	public T Get<T>(string key)
	{
		var value = Get(key);
		if (value is T typedValue)
		{
			return typedValue;
		}
		return default(T);
	}

	public T Get<T>(string key, T defaultValue)
	{
		if (Values.TryGetValue(key, out var value))
		{
			if (value is T typedValue)
			{
				return typedValue;
			}
		}
		return defaultValue;
	}

	public void Set(string key, object value)
	{
		Values[key] = value;
	}

	public bool Delete(string key)
	{
		return Values.Remove(key);
	}

	public bool Has(string key)
	{
		return Values.ContainsKey(key);
	}
}
