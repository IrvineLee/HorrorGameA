using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Helper;
using Personal.Save;
using Personal.GameState;
using Personal.Preloader;

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

		IEnumerable<IDataPersistence> dataPersistenceObjects;

		protected override void OnTitleScene()
		{
			// Reset the slot save data.
			saveObject = new SaveObject();
			dataPersistenceObjects = null;
		}

		protected override void OnEarlyMainScene()
		{
			// This will only happen in editor mode, when you enter the scene without going through the title scene.
			if (!PreloadGame.PreviousSceneName.Equals(SceneName.Title))
			{
				LoadSlotData();
			}

			// Since there might be other main scenes, you want to update their data first before getting the new objects in the new scene.
			UpdateDataPersistence();

			dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
			foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
			{
				dataPersistenceObj.LoadData(saveObject);
			}
		}

		/// <summary>
		/// Save the current user data.
		/// </summary>
		public void SaveProfileData()
		{
			SavePath(profileDirectory + profileFileName, saveProfile);
			UIManager.Instance.ToolsUI.LoadingIconTrans(true);
		}

		/// <summary>
		/// Load the current user data.
		/// </summary>
		public void LoadProfileData()
		{
			saveProfile = LoadPath<SaveProfile>(profileDirectory + profileFileName);

			GameStateBehaviour.Instance.InitializeProfile(saveProfile);
		}

		/// <summary>
		/// Save data to indicated slot.
		/// </summary>
		/// <param name="slotID"></param>
		public void SaveSlotData(int slotID = 0)
		{
			saveObject.PlayerSavedData.SceneName = GameSceneManager.Instance.CurrentSceneName;
			UpdateDataPersistence();

			SavePath(GetSlotPath(slotID), saveObject);

			if (saveProfile.LatestSaveSlot == slotID) return;

			// Update the latest slot data for title scene.
			saveProfile.LatestSaveSlot = slotID;
			SaveProfileData();
		}

		/// <summary>
		/// Load data from indicated slot.
		/// </summary>
		/// <param name="slotID"></param>
		public void LoadSlotData(int slotID = 0)
		{
			if (slotID >= 0 && slotID < ID_COUNT)
			{
				saveObject = LoadPath<SaveObject>(GetSlotPath(slotID));
			}

			GameStateBehaviour.Instance.InitializeData(saveObject);
		}

		/// <summary>
		/// Delete profile data.
		/// </summary>
		public void DeleteProfileData()
		{
			dataService.ClearData(profileDirectory + profileFileName);
		}

		/// <summary>
		/// Delete data from indicated slot.
		/// </summary>
		public void DeleteSlotData(int slotID = 0)
		{
			dataService.ClearData(GetSlotPath(slotID));

			saveProfile.LatestSaveSlot = -1;
			SaveProfileData();
		}

		/// <summary>
		/// This saves data that happened in a scene. Ex: Whether an object has been taken/Has been interacted with/Cleared puzzle.
		/// </summary>
		void UpdateDataPersistence()
		{
			if (dataPersistenceObjects == null) return;

			foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
			{
				dataPersistenceObj.SaveData(saveObject);
			}
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
				HandleDataPrint("<color=cyan>Save : " + typeStr + "</color> Time : ", saveTime, data);
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
		T LoadPath<T>(string path, Action onCompleteAction = default) where T : GenericSave
		{
			T data = default(T);
			long startTime = DateTime.Now.Ticks;

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
				}

				long loadTime = DateTime.Now.Ticks - startTime;

				string typeStr = typeof(T).ToString().SearchBehindRemoveFrontOrEnd('.', true, false);
				HandleDataPrint("<color=yellow>Load : " + typeStr + "</color> Time : ", loadTime, data);

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