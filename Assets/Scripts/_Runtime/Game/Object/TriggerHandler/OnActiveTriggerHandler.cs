using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	/// <summary>
	/// Used for triggering a statemachine by setting the gameobject active. Useful for cutscenes etc. 
	/// </summary>
	public class OnActiveTriggerHandler : InteractableEventBegin
	{
		protected override void Initialize()
		{
			base.Initialize();

			//var stateMachine = StageManager.Instance.PlayerController.FSM;
			//HandleInteraction(stateMachine, stateMachine.IFSMHandler.OnExit).Forget();
		}
	}
}

