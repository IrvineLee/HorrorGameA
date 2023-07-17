using UnityEngine;

namespace Personal.UI
{
	public abstract class UISelectionBase : MonoBehaviour
	{
		void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize() { }

		public virtual void NextSelection(bool isNext) { }
	}
}
