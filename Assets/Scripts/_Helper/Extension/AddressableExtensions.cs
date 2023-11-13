using UnityEngine;

using UnityEngine.AddressableAssets;

namespace Helper
{
	public static class AddressableExtensions
	{
		// Release the instance that has been instantiated with Addressables.InstantiateAsync.
		public static void ReleaseInstance(this GameObject original)
		{
			Addressables.ReleaseInstance(original);
		}
	}
}
