using UnityEngine;

using Personal.Manager;
using Personal.GameState;
using Personal.Definition;

namespace Personal.InputProcessing
{
	public class InputControllerBase : GameInitialize
	{
		protected InputReaderDefinition inputReaderDefinition;

		public Vector2 Move { get; protected set; }
		public Vector2 Look { get; protected set; }

		public bool IsInteract { get; protected set; }
		public bool IsCancel { get; protected set; }

		protected override void Initialize()
		{
			inputReaderDefinition = InputManager.Instance.InputReaderDefinition;
			isAwakeCompleted = true;
		}

		void LateUpdate()
		{
			ResetClicks();
		}

		protected void CloseMenu()
		{
			UIManager.Instance.CloseWindowStack();
		}

		protected virtual void ResetClicks()
		{
			IsInteract = false;
			IsCancel = false;
		}

		protected override void OnPostDisable()
		{
			Move = Vector2.zero;
			Look = Vector2.zero;

			ResetClicks();
		}
	}
}