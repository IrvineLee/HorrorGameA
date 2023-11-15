using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class ToolsHandlerUI : MonoBehaviour
	{
		[SerializeField] Transform loadingIconTrans = null;
		[SerializeField] CinematicBars cinematicBars = null;
		[SerializeField] Image blackScreen = null;
		[SerializeField] Image inputBlocker = null;

		public void LoadingIconTrans(bool isFlag) { loadingIconTrans.gameObject.SetActive(isFlag); }

		public void CinematicBars(bool isFlag)
		{
			if (isFlag) cinematicBars.Show();
			else cinematicBars.Hide();
		}

		public void BlackScreen(bool isFlag) { blackScreen.gameObject.SetActive(isFlag); }
		public void BlockInput(bool isFlag) { inputBlocker.gameObject.SetActive(isFlag); }
	}
}
