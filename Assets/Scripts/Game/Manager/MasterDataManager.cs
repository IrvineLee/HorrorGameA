using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Personal.Manager
{
	[CreateAssetMenu(fileName = "MasterDataManager", menuName = "ScriptableObjects/MasterDataManager")]
	public class MasterDataManager : ScriptableObject
	{
		public static MasterDataManager Instance { get; private set; }

		[SerializeField] MasterItem item = null;

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