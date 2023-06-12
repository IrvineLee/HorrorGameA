using UnityEngine;

using Personal.Manager;

namespace Personal.GameState
{
	public class AddToUIManager : GameInitialize
	{
		protected override void Initialize()
		{
			transform.SetParent(UIManager.Instance.transform);
		}
	}
}