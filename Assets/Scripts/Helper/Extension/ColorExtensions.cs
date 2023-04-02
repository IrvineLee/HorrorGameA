using UnityEngine;

namespace Helper
{
	public static class ColorExtensions
	{
		// Replace with
		public static Color With(this Color original, float? r = null, float? g = null, float? b = null, float? a = null)
		{
			return new Color(r ?? original.r, g ?? original.g, b ?? original.b, a ?? original.a);
		}

		// Get color from basicColor enum
		public static Color GetColor(this BasicColor original)
		{
			switch (original)
			{
				case BasicColor.Blue:
					return Color.blue;
				case BasicColor.Red:
					return Color.red;
				case BasicColor.Green:
					return Color.green;
				case BasicColor.Yellow:
					return Color.yellow;
				case BasicColor.Purple:
					return Color.magenta;
				case BasicColor.Orange:
					return new Color(1.0f, 0.64f, 0.0f);
				case BasicColor.Grey:
					return Color.grey;
				case BasicColor.White:
					return Color.white;
				case BasicColor.Black:
					return Color.black;
			}

			return Color.white;
		}
	}
}
