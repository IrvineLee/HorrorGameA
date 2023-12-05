using System;
using UnityEngine;

using Personal.Manager;
using Personal.Definition;
using Personal.UI;

namespace Personal.InputProcessing
{
	public class InputControllerBase : MonoBehaviour
	{
		protected InputReaderDefinition inputReaderDefinition;

		public Vector2 Move { get; protected set; }
		public Vector2 MoveNormalized { get => Move.normalized; }

		public Vector2 Look { get; protected set; }
		public Vector2 LookNormalized { get => Look.normalized; }

		public bool IsInteract { get; protected set; }
		public bool IsCancel { get; protected set; }

		public void Initialize()
		{
			inputReaderDefinition = InputManager.Instance.InputReaderDefinition;
		}

		protected virtual void OnEnable() { }

		void LateUpdate()
		{
			ResetClicks();
		}

		protected virtual void CloseMenu()
		{
			UIManager.Instance.CloseWindowStack();
		}

		protected virtual void ResetClicks()
		{
			IsInteract = false;
			IsCancel = false;
		}

		protected virtual void CheckInterfaceTypeAndAct(UIInterfaceType uiInterfaceType, Action action)
		{
			if (UIManager.Instance.ActiveInterfaceType != uiInterfaceType) return;
			action?.Invoke();
		}

		protected virtual void OnDisable()
		{
			Move = Vector2.zero;
			Look = Vector2.zero;

			ResetClicks();
		}
	}
}