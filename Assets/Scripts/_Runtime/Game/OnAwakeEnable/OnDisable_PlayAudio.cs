using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.GameState;
using Personal.Setting.Audio;

namespace Personal.UI
{
	public class OnDisable_PlayAudio : GameInitialize
	{
		[SerializeField] AudioSFXType sfxType = AudioSFXType.None;
		[SerializeField] float volume = 1;

		protected override void OnDisabled()
		{
			if (App.IsQuitting || StageManager.Instance.IsBusy) return;

			AudioManager.Instance.PlaySFXAt(sfxType, transform.position, volume);
		}
	}
}
