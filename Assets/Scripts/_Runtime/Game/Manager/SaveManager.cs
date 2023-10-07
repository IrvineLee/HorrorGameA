using UnityEngine;
using System;
using Newtonsoft.Json;

using Helper;
using Personal.Save;
using Personal.GameState;
using System.IO;

namespace Personal.Manager
{
	public class SaveManager : GameInitializeSingleton<SaveManager>
	{
		const int ID_COUNT = 5;

		[SerializeField] bool isEncryptionEnabled = false;
		[SerializeField] bool isPrintData = false;
		[SerializeField] bool isFastSaveLoad = false;

		// User profile path.
		string profileDirectory = "/ProfileData";
		string profileFileName = "/MyProfile.json";

		// Save data path.
		string directory = "/SaveData";
		string fileName = "/MyData.json";

		// All save data.
		SaveProfile saveProfile = new SaveProfile();
		SaveObject saveObject = new SaveObject();

		IDataService dataService = new JsonDataService();

		/// <summary>
		/// Save the current user data.
		/// </summary>
		public void SaveProfileData()
		{
			SavePath(profileDirectory + profileFileName, saveProfile);
			UIManager.Instance.ToolsUI.LoadingIconTrans.gameObject.SetActive(true);
		}

		/// <summary>
		/// Load the current user data.
		/// </summary>
		public bool LoadProfileData()
		{
			saveProfile = LoadPath<SaveProfile>(profileDirectory + profileFileName, out bool isNewlyCreated);

			GameStateBehaviour.Instance.InitializeProfile(saveProfile);
			return isNewlyCreated;
		}

		/// <summary>
		/// Save data to indicated slot.
		/// </summary>
		/// <param name="slotID"></param>
		public void SaveSlotData(int slotID = 0)
		{
			SavePath(GetSlotPath(slotID), saveObject);
			UIManager.Instance.ToolsUI.LoadingIconTrans.gameObject.SetActive(true);
		}

		/// <summary>
		/// Load data from indicated slot.
		/// </summary>
		/// <param name="slotID"></param>
		public void LoadSlotData(int slotID = 0)
		{
			if (slotID < 0 || slotID > ID_COUNT)
			{
				Debug.Log("<Color=Red>Save ID is not within range. Check SaveManager's ID_COUNT.</Color>");
				return;
			}

			saveObject = LoadPath<SaveObject>(GetSlotPath(slotID), out bool isNewlyCreated);
			GameStateBehaviour.Instance.InitializeData(saveObject);
		}

		/// <summary>
		/// Delete data from indicated slot.
		/// </summary>
		public void DeleteSlotData(int slotID = 0)
		{
			dataService.ClearData(GetSlotPath(slotID));
		}

		/// <summary>
		/// Save data to designated path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="data"></param>
		void SavePath<T>(string path, T data) where T : GenericSave
		{
			long startTime = DateTime.Now.Ticks;

			try
			{
				string newPath = Application.persistentDataPath + path;

				if (isFastSaveLoad)
				{
					// Create folder path.
					string folderPath = newPath.SearchBehindRemoveFrontOrEnd('/', true);
					if (!Directory.Exists(folderPath))
						Directory.CreateDirectory(folderPath);

					File.WriteAllText(newPath, JsonUtility.ToJson(data));
				}
				else
				{
					dataService.SaveData(newPath, data, isEncryptionEnabled);
				}

				long saveTime = DateTime.Now.Ticks - startTime;

				string typeStr = typeof(T).ToString().SearchBehindRemoveFrontOrEnd('.', true, false);
				HandleDataPrint("Save : <color=green>" + typeStr + "</color> Time : ", saveTime, data);
			}
			catch (Exception e)
			{
				Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
			}
		}

		/// <summary>
		/// Load data from designated path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="onCompleteAction"></param>
		/// <returns></returns>
		T LoadPath<T>(string path, out bool isNewlyCreated, Action onCompleteAction = default) where T : GenericSave
		{
			T data = default(T);
			long startTime = DateTime.Now.Ticks;

			isNewlyCreated = false;
			try
			{
				string newPath = Application.persistentDataPath + path;

				if (isFastSaveLoad)
				{
					data = JsonUtility.FromJson<T>(File.ReadAllText(newPath));
				}
				else
				{
					data = dataService.LoadData<T>(newPath, isEncryptionEnabled);
				}

				if (data == default)
				{
					data = GetTConstructor(data);
					isNewlyCreated = true;
				}

				long loadTime = DateTime.Now.Ticks - startTime;

				string typeStr = typeof(T).ToString().SearchBehindRemoveFrontOrEnd('.', true, false);
				HandleDataPrint("Load : <color=yellow>" + typeStr + "</color> Time : ", loadTime, data);

				onCompleteAction?.Invoke();
			}
			catch (Exception e)
			{
				Debug.LogError($"Could not read file! " + e.Message);
			}
			return data;
		}

		/// <summary>
		/// Get generic constructor.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		T GetTConstructor<T>(T data) where T : GenericSave
		{
			if (typeof(T) == typeof(SaveProfile))
			{
				return data = (T)(object)new SaveProfile();
			}
			else if (typeof(T) == typeof(SaveObject))
			{
				return data = (T)(object)new SaveObject();
			}

			Debug.Log(data + "  " + typeof(T));
			return null;
		}

		/// <summary>
		/// Get slot path.
		/// </summary>
		/// <param name="slotID"></param>
		/// <returns></returns>
		string GetSlotPath(int slotID)
		{
			string file = fileName.SearchBehindRemoveFrontOrEnd('.', true);
			string ext = fileName.SearchBehindRemoveFrontOrEnd('.', false, false);

			return directory + file + slotID.ToString().AddSymbolInFront('0', 2) + ext;
		}

		void HandleDataPrint<T>(string headerStr, float loadTime, T data) where T : GenericSave
		{
			string loadTimeStr = headerStr + (loadTime / TimeSpan.TicksPerMillisecond).ToString("N4") + "ms";
			string dataStr = isPrintData ? "Loaded from file:\r\n" + JsonConvert.SerializeObject(data, Formatting.Indented) : "";

			Debug.Log(loadTimeStr + dataStr);
		}
	}
}