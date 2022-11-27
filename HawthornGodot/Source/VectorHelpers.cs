namespace Hawthorn;
using Godot;

public static class VectorHelpers
{
	public static bool TryGetPosition(this Blackboard state, string key, out Vector3 position)
	{
		var value = state.Get(key);

		if (value != null)
		{
			if (value is Vector3 pos)
			{
				position = pos;
				return true;
			}

			if (value is Node3D node)
			{
				position = node.GlobalPosition;
				return true;
			}
		}

		position = default(Vector3);
		return false;
	}
}
