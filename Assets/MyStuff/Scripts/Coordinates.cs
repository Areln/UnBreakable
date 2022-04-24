
using System;
using System.Collections.Generic;

[Serializable]
public class Coordinates
{
	public int X;

	public int Y;

	public class EqualityComparer : IEqualityComparer<Coordinates>
	{
		public bool Equals(Coordinates x, Coordinates y)
		{
			return x.X == y.X && y.Y == x.Y;
		}

		public int GetHashCode(Coordinates obj)
		{
			return obj.X ^ obj.Y;
		}
	}

	public bool Equals(Coordinates other)
	{
		return X == other.X && Y == other.Y;
	}

}
