using System.Collections.Generic;
using UnityEngine;

using Personal.GameState;
using Helper;

namespace Personal.InputProcessing
{
	public abstract class ControlInputBase : GameInitialize
	{
		public static ControlInputBase ActiveControlInput { get; protected set; }

		static Stack<ControlInputBase> activeControlStack = new();

		protected override void OnEnabled()
		{
			activeControlStack.Push(this);
			ActiveControlInput = activeControlStack.Peek();
		}

		protected override void OnDisabled()
		{
			if (App.IsQuitting) return;

			activeControlStack.Pop();
			ActiveControlInput = activeControlStack.Peek();
		}
	}
}