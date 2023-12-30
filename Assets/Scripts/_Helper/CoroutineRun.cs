using System;
using System.Collections;
using UnityEngine;

namespace Helper
{
	public class CoroutineRun
	{
		public bool IsDone { get; private set; } = true;
		public bool IsAbort { get; private set; }
		public bool IsPause { get; private set; }

		IEnumerator currentAct;

		public CoroutineRun() { }

		// Typically used for coroutine that does not start immediately.
		public CoroutineRun(bool isDone) { IsDone = isDone; }

		public CoroutineRun Initialize(IEnumerator act, Action doLast = default)
		{
			IsAbort = IsDone = IsPause = false;

			currentAct = act;
			if (currentAct != null)
			{
				HelperObj.Instance?.StartCoroutine(IE_Run(doLast));
				HelperObj.Instance?.StartCoroutine(IE_AbortCheck(doLast));
			}
			return this;
		}

		IEnumerator IE_Run(Action doLast)
		{
			IsDone = false;
			yield return currentAct;
			if (doLast != default && doLast != null) doLast();
			IsDone = true;
		}

		IEnumerator IE_AbortCheck(Action doLast)
		{
			while (!IsDone)
			{
				if (IsAbort)
				{
					StopCoroutine();
					if (doLast != default || doLast != null) doLast();
					IsDone = true;

					yield break;
				}
				yield return null;
			}
		}

		/// <summary>
		/// Stop current coroutine and doLast action. 
		/// </summary>
		public void StopCoroutine()
		{
			if (currentAct != null)
				HelperObj.Instance?.StopCoroutine(currentAct);

			IsDone = true;
		}

		/// <summary>
		/// Calling abort will stop the coroutine but still run the doLast action.
		/// </summary>
		public void Abort() { IsAbort = true; }

		/// <summary>
		/// Pause the coroutine.
		/// </summary>
		public void Pause() { IsPause = true; }

		/// <summary>
		/// UnPause the coroutine.
		/// </summary>
		public void UnPause() { IsPause = false; }
	}
}
