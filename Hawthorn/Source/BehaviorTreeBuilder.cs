namespace Hawthorn;

/// <summary>
/// This serves as a launch point for extension methods to build off from
/// </summary>
public class BehaviorBuilder<A>
{
	/// <summary>
	/// These values can be used to make setting up builders more ergonomic
	/// </summary>
	Dictionary<string, object> sharedValues = new Dictionary<string, object>();

	public bool TryGet<T>(out T value)
	{
		return TryGet<T>(nameof(T), out value);
	}

	public bool TryGet<T>(string key, out T value)
	{
		if (sharedValues.TryGetValue(key, out var rawValue))
		{
			if (rawValue is T typedValue)
			{
				value = typedValue;
				return true;
			}
		}
		value = default(T);
		return false;
	}

	public T Get<T>()
	{
		return Get<T>(nameof(T));
	}

	public T Get<T>(string key)
	{
		if (TryGet<T>(key, out var value))
		{
			return value;
		}
		return default(T);
	}

	public void Set(string key, object value)
	{
		sharedValues[key] = value;
	}
}

public interface IBehaviorNodeBuilder<A>
{
	IBehaviorNode<A> Build();
}

public static class BehaviorBuilderExtensions
{
	public static BehaviorTree<A> ToTree<A>(this IBehaviorNodeBuilder<A> nodeBuilder)
	{
		var node = nodeBuilder.Build();
		if (node is IBehaviorNodeContainer<A> branch)
		{
			return new BehaviorTree<A>(branch);
		}

		throw new System.ArgumentException("Cannot build a behavior tree from a leaf node. Use a branch node instead.");
	}
}
