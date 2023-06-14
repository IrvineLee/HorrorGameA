using UnityEngine;

namespace Personal.Transition
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

