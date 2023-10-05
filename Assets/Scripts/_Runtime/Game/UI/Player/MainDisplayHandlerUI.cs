using UnityEngine;

using TMPro;

namespace Personal.UI
{
	public class MainDisplayHandlerUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI questTMP = null;

		public void SetQuest(string text)
		{
			questTMP.text = text;
		}
	}
}

