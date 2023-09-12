using Personal.GameState;
using Personal.Localization;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Personal.Manager
{
	[CreateAssetMenu(fileName = "MasterDataManager", menuName = "ScriptableObjects/MasterDataManager")]
	public class MasterDataManager : ScriptableObject
	{
		public static MasterDataManager Instance { get; private set; }

		[SerializeField] MasterBGM bgm = null;
		[SerializeField] MasterSFX sfx = null;
		[SerializeField] MasterItem item = null;
		[SerializeField] MasterCashierNPC cashierNPC = null;
		[SerializeField] MasterWindowUI windowUI = null;
		[SerializeField] MasterLocalization localization = null;
		[SerializeField] MasterAchievement achievement = null;

		public MasterBGM Bgm { get => bgm; }
		public MasterSFX Sfx { get => sfx; }
		public MasterItem Item { get => item; }
		public MasterCashierNPC CashierNPC { get => cashierNPC; }
		public MasterWindowUI WindowUI { get => windowUI; }
		public MasterLocalization Localization { get => localization; }
		public MasterAchievement Achievement { get => achievement; }

		public static void CreateInstance()
		{
			if (Instance != null) return;

			var handle = Addressables.LoadAssetAsync<MasterDataManager>(typeof(MasterDataManager).Name);
			handle.Completed += (op) => { Instance = op.Result; };
		}

		public static void Initialize()
		{
			SupportedLanguageType language = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData.SelectedLanguage;

			Instance.Localization.Initialize();
			Instance.Localization.UpdateActiveLanguage(language);
		}
	}
}