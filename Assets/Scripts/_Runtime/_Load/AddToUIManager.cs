using UnityEngine;

using Personal.Manager;

namespace Personal.GameState
{
	public class AddToUIManager : MonoBehaviour
	{
		void Awake()
		{
			transform.SetParent(UIManager.Instance.transform);
		}
	}
}