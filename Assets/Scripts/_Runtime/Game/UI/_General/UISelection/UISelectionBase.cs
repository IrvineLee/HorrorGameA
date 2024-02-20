using System;
using UnityEngine;

namespace Personal.UI
{
	public abstract class UISelectionBase : MonoBehaviour
	{
		public virtual void InitialSetup() { }

		public virtual void Initialize() { }

		/// <summary>
		/// Move the selection.
		/// </summary>
		/// <param name="isNext">Left/Right or Top/Bottom</param>
		/// <param name="endConfirmButtonAction">What will happen when you are at the end of selection and pressed the confirm button. Ex: Close the window</param>
		public virtual void NextSelection(bool isNext, Action endConfirmButtonAction = default) { }

		/// <summary>
		/// Submit pressed.
		/// </summary>
		public virtual void Submit() { }

		/// <summary>
		/// Cancel pressed.
		/// </summary>
		public virtual void Cancel() { }
	}
}
