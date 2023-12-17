using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.UI;
using Personal.Data;

namespace Personal.FSM.Character
{
	public class ReadFileState : ActorStateBase
	{
		[SerializeField] FileData fileData = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			UIManager.Instance.ReadFileUI.Read(fileData);
			await UniTask.WaitUntil(() => UIManager.Instance.ActiveInterfaceType != UIInterfaceType.ReadFile, cancellationToken: this.GetCancellationTokenOnDestroy());
		}
	}
}