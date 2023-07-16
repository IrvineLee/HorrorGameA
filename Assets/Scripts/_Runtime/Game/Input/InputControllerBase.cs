using UnityEngine;

using Personal.Manager;
using Personal.Definition;

namespace Personal.InputProcessing
{
	public class InputControllerBase : MonoBehaviour
	{
		protected InputReaderDefinition inputReaderDefinition;

		public Vector2 Move { get; protected set; }
		public Vector2 MoveOnce { get; protected set; }
		public Vector2 Look { get; protected set; }

		public bool IsInteract { get; protected set; }
		public bool IsCancel { get; protected set; }

		public void Initialize()
		{
			inputReaderDefinition = InputManager.Instance.InputReaderDefinition;
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

			MoveOnce = Vector2.zero;
		}

		protected virtual void OnDisable()
		{
			Move = Vector2.zero;
			Look = Vector2.zero;

			ResetClicks();
		}
	}
}