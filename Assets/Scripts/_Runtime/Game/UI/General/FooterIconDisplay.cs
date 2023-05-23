using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnityEngine.Events;
using System.Text;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class FooterIconDisplay : GameInitialize
	{
		//List<Transform> 

		protected override async UniTask Awake()
		{
			await base.Awake();
		}
	}
}
