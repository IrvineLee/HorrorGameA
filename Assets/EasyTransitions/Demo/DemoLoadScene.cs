using UnityEngine;

namespace EasyTransition
{
	public class DemoLoadScene : MonoBehaviour
	{
		[SerializeField] TransitionType transitionType;

		public void LoadScene()
		{
			TransitionManager.Instance.Transition(transitionType);
		}
	}
}

