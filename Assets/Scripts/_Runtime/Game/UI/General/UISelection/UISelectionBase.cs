using UnityEngine;

namespace Personal.UI
{
	public abstract class UISelectionBase : MonoBehaviour
	{
		public virtual void Initialize() { }

		public virtual void NextSelection(bool isNext) { }
	}
}
