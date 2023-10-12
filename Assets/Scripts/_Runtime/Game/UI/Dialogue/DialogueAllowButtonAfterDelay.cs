using UnityEngine;
using UnityEngine.UI;

using Helper;

namespace Personal.Dialogue
{
	public class DialogueAllowButtonAfterDelay : MonoBehaviour
	{
		[SerializeField] float delayDuration = 0.25f;
		[SerializeField] bool isOnEnable = false;

		Button button;

		void Awake()
		{
			button = GetComponentInChildren<Button>();
		}

		void OnEnable()
		{
			if (!isOnEnable) return;

			DelayInteractable();
		}

		public void DelayInteractable()
		{
			button.interactable = false;
			CoroutineHelper.WaitFor(delayDuration, () => button.interactable = true);
		}
	}
}
