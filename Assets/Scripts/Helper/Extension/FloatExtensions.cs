using UnityEngine;

namespace Helper
{
	public static class FloatExtensions
	{
		// Fix -1.025547E-07 kinda value.
		public static float FloatPointFix(this float original)
		{
			return Mathf.Round(original * 10) / 10;
		}
	}
}
