using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Personal.GameState;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace Personal.UI
{
	public class FooterIconDisplay : GameInitialize
	{
		[SerializeField] TextMeshProUGUI iconWithTMP = null;
		[SerializeField] int initialSpawnCount = 3;

		List<TextMeshProUGUI> iconWithTMPList = new();

		protected override async UniTask Awake()
		{
			await base.Awake();

			var goList = await AddressableHelper.SpawnMultiple(AssetAddress.UI_IconWithTMP, initialSpawnCount, Vector3.zero, transform);
			iconWithTMPList = goList.Select(r => r.GetComponentInChildren<TextMeshProUGUI>()).Where(g => g != null).ToList();
		}
	}
}
