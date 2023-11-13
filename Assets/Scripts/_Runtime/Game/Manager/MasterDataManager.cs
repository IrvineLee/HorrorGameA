using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

using Helper;
using Personal.GameState;
using Personal.Item;
using Personal.Localization;
using Personal.Quest;
using Personal.Constant;
using Personal.Achievement;

namespace Personal.Manager
{
	[CreateAssetMenu(fileName = "MasterDataManager", menuName = "ScriptableObjects/MasterDataManager")]
	public class MasterDataManager : ScriptableObject
	{
		public static MasterDataManager Instance { get; private set; }

		[SerializeField] MasterBGM bgm = null;
		[SerializeField] MasterSFX sfx = null;
		[SerializeField] MasterItem item = null;
		[SerializeField] MasterQuest quest = null;
		[SerializeField] MasterCashierNPC cashierNPC = null;
		[SerializeField] MasterWindowUI windowUI = null;
		[SerializeField] MasterLocalization localization = null;
		[SerializeField] MasterAchievement achievement = null;

		public MasterBGM Bgm { get => bgm; }
		public MasterSFX Sfx { get => sfx; }
		public MasterItem Item { get => item; }
		public MasterQuest Quest { get => quest; }
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

			Instance.Localization.InitializeAllLanguages();
			MasterLocalization.SetActiveLanguage(language);
		}

		/// <summary>
		/// Get the correct enum based on inputted id.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public T GetEnumType<T>(int id) where T : Enum
		{
			if (id.IsWithin(ConstantFixed.ITEM_START, ConstantFixed.ITEM_END)) return (T)(object)(ItemType)(id);
			else if (id.IsWithin(ConstantFixed.MAIN_QUEST_START, ConstantFixed.SUB_QUEST_END)) return (T)(object)(QuestType)(id);
			else if (id.IsWithin(ConstantFixed.ACHIEVEMENT_START, ConstantFixed.ACHIEVEMENT_END)) return (T)(object)(AchievementType)(id);
			return default;
		}
	}
}