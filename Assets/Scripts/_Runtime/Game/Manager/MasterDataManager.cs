using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Personal.Manager
{
	[CreateAssetMenu(fileName = "MasterDataManager", menuName = "ScriptableObjects/MasterDataManager")]
	public class MasterDataManager : ScriptableObject
	{
		public static MasterDataManager Instance { get; private set; }

		[SerializeField] MasterBGM bgm = null;
		[SerializeField] MasterSFX sfx = null;
		[SerializeField] MasterItem item = null;

		public MasterBGM Bgm { get => bgm; }
		public MasterSFX Sfx { get => sfx; }
		public MasterItem Item { get => item; }

		static AsyncOperationHandle<MasterDataManager> handle;

		public static void CreateInstance()
		{
			if (Instance == null && !handle.IsValid())
			{
				handle = Addressables.LoadAssetAsync<MasterDataManager>(typeof(MasterDataManager).Name);
				handle.Completed += (op) => { Instance = op.Result; };
			}
		}
	}
}