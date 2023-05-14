using UnityEngine;

namespace EasyTransition
{
	public class DemoLoadScene : MonoBehaviour
	{
		[SerializeField] TransitionType transitionType;
		[SerializeField] float loadDelay;

		public void LoadScene()
		{
			TransitionManager.Instance.Transition(transitionType, loadDelay);
		}
	}
}

