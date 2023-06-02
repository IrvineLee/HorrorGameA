using UnityEngine;

namespace Personal.UI
{
	public class ToolsHandlerUI : MonoBehaviour
	{
		[SerializeField] Transform loadingIconTrans = null;
		[SerializeField] CinematicBars cinematicBars = null;

		public Transform LoadingIconTrans { get => loadingIconTrans; }
		public CinematicBars CinematicBars { get => cinematicBars; }
	}
}
