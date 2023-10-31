using System.Threading;
using UnityEngine;

namespace Helper
{
	public static class CancellationTokenSourceExtensions
	{
		// Refresh cancellation token source.
		public static CancellationTokenSource Refresh(this CancellationTokenSource original)
		{
			original.Cancel();
			original.Dispose();

			original = new CancellationTokenSource();
			return original;
		}
	}
}
