using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

using Personal.GameState;
using Personal.Manager;
using Helper;
using Cysharp.Threading.Tasks;

namespace Personal.UI.Option
{
	public class OptionControlUI : OptionMenuUI
	{
		//[SerializeField] Slider masterSlider = null;

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			HandleLoadDataToUI();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			base.Save_Inspector();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected override void HandleLoadDataToUI()
		{
			//audioData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.AudioData;
		}
	}
}