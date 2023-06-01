using Cysharp.Threading.Tasks;
using Personal.Manager;
using UnityEngine;

namespace Personal.FSM.Character
{
	public class CinematicState : StateBase
	{
		[SerializeField] bool isOn = true;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			if (isOn) UIManager.Instance.CinematicBars.Show();
			else UIManager.Instance.CinematicBars.Hide();
		}
	}
}