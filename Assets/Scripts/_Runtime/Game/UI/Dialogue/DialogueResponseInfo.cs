using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Personal.UI;

namespace Personal.Dialogue
{
	public class DialogueResponseInfo : MonoBehaviour
	{
		[SerializeField] Button button = null;
		[SerializeField] UISelectable uiSelectable = null;

		public Button Button { get => button; }
		public UISelectable UISelectable { get => uiSelectable; }

		UnityAction unityAction = null;

		public void SetupButton(UnityAction unityAction)
		{
			this.unityAction = unityAction;

			button.onClick.AddListener(unityAction);
			button.enabled = true;
			button.transform.SetAsFirstSibling();
		}

		public void ResetButton()
		{
			if (unityAction == null) return;

			button.onClick.RemoveListener(unityAction);
			button.enabled = false;
		}
	}
}
