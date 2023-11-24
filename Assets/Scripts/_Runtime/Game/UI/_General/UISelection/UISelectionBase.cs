using UnityEngine;

namespace Personal.UI
{
	public abstract class UISelectionBase : MonoBehaviour
	{
		public virtual void InitialSetup() { }

		public virtual void Initialize() { }

		/// <summary>
		/// Move the selection left/right.
		/// </summary>
		/// <param name="isNext"></param>
		public virtual void NextSelection(bool isNext) { }

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
