namespace Hawthorn;

public class Blackboard
{
	Dictionary<string, object> Values = new Dictionary<string, object>();

	public object? Get(string key)
	{
		if (Values.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	public T? Get<T>(string key)
	{
		var value = Get(key);
		if (value is T typedValue)
		{
			return typedValue;
		}
		return default(T);
	}

	public T? Get<T>(string key, T defaultValue)
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

	public void Delete(string key)
	{
		Values.Remove(key);
	}

	public bool Has(string key)
	{
		return Values.ContainsKey(key);
	}
}
