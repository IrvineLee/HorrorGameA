using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class ToolsHandlerUI : MonoBehaviour
	{
		[SerializeField] Transform loadingIconTrans = null;
		[SerializeField] CinematicBars cinematicBars = null;
		[SerializeField] Image inputBlocker = null;

		public Transform LoadingIconTrans { get => loadingIconTrans; }
		public CinematicBars CinematicBars { get => cinematicBars; }
		public Image InputBlocker { get => inputBlocker; }
	}
}
