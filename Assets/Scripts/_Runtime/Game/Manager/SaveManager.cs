﻿using UnityEngine;
using System;
using Newtonsoft.Json;

using Helper;
using Personal.Save;
using Personal.GameState;

namespace Personal.Manager
{
	public class SaveManager : GameInitializeSingleton<SaveManager>
	{
		const int ID_COUNT = 5;

		[SerializeField] bool isEncryptionEnabled = false;

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
			SavePath(GetSlotPath(slotID), saveObject);
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

			Action onCompleteAction = () => GameStateBehaviour.Instance.SetGameDataAndLoad(saveObject);

			saveObject = LoadPath<SaveObject>(GetSlotPath(slotID), onCompleteAction);
			saveObject.PlayerSavedData.SlotID = slotID;

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
			if (dataService.SaveData(path, data, isEncryptionEnabled))
			{
				long saveTime = DateTime.Now.Ticks - startTime;
				string s = $"Save Time: {(saveTime / TimeSpan.TicksPerMillisecond):N4}ms " +
						   "Save file:\r\n" + JsonConvert.SerializeObject(data, Formatting.Indented);

				Debug.Log(s);
			}
			else
			{
				Debug.LogError("Could not save file!");
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
				data = dataService.LoadData<T>(path, isEncryptionEnabled);
				if (data == default)
				{
					data = GetTConstructor(data);
				}

				long loadTime = DateTime.Now.Ticks - startTime;

				string s = $"Load Time: {(loadTime / TimeSpan.TicksPerMillisecond):N4}ms " +
						   "Loaded from file:\r\n" + JsonConvert.SerializeObject(data, Formatting.Indented);

				Debug.Log(s);
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
			string file = StringHelper.RemoveAllWhenReachCharFromBehind(fileName, '.', true);
			string ext = StringHelper.RemoveAllWhenReachCharFromBehind(fileName, '.', false, false);

			return directory + file + StringHelper.AddSymbolInFront('0', 2, slotID.ToString()) + ext;
		}
	}
}