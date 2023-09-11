using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

using Helper;

namespace Personal.Save
{
	public class JsonDataService : IDataService
	{
		const string KEY = "ggdPhkeOoiv6YMiPWa34kIuOdDUL7NwQFg6l1DVdwN8=";
		const string IV = "JZuM0HQsWSBVpRHTeRZMYQ==";

		/// <summary>
		/// Save data to path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="relativePath"></param>
		/// <param name="data"></param>
		/// <param name="encrypted"></param>
		/// <returns></returns>
		public bool SaveData<T>(string relativePath, T data, bool encrypted)
		{
			string path = Application.persistentDataPath + relativePath;

			try
			{
				// Create folder path.
				string folderPath = path.RemoveAllWhenReachCharFromBehind('/', true);
				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				using FileStream stream = File.Create(path);
				if (encrypted)
				{
					WriteEncryptedData(data, stream);
				}
				else
				{
					stream.Close();
					File.WriteAllText(path, JsonUtility.ToJson(data));
					//File.WriteAllText(path, JsonConvert.SerializeObject(data));
				}
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
				return false;
			}
		}

		/// <summary>
		/// Load data from path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="relativePath"></param>
		/// <param name="encrypted"></param>
		/// <returns></returns>
		public T LoadData<T>(string relativePath, bool encrypted)
		{
			string path = Application.persistentDataPath + relativePath;

			if (!File.Exists(path))
			{
				Debug.Log($"Cannot load file at {path}. File does not exist!");
				return default(T);
			}

			try
			{
				T data;
				if (encrypted)
				{
					data = ReadEncryptedData<T>(path);
				}
				else
				{
					data = JsonUtility.FromJson<T>(File.ReadAllText(path));
					//data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
				}
				return data;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
				throw e;
			}
		}

		/// <summary>
		/// Clear data from path.
		/// </summary>
		/// <param name="relativePath"></param>
		public void ClearData(string relativePath)
		{
			string path = Application.persistentDataPath + relativePath;
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		void WriteEncryptedData<T>(T data, FileStream stream)
		{
			Aes aesProvider = Aes.Create();
			aesProvider.Key = Convert.FromBase64String(KEY);
			aesProvider.IV = Convert.FromBase64String(IV);

			ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
			CryptoStream cryptoStream = new CryptoStream(
				stream,
				cryptoTransform,
				CryptoStreamMode.Write
			);

			// You can uncomment the below to see a generated value for the IV & key.
			// You can also generate your own if you wish
			//Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");
			//Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");

			cryptoStream.Write(Encoding.ASCII.GetBytes(JsonUtility.ToJson(data)));
			//cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
		}

		T ReadEncryptedData<T>(string path)
		{
			byte[] fileBytes = File.ReadAllBytes(path);
			Aes aesProvider = Aes.Create();

			aesProvider.Key = Convert.FromBase64String(KEY);
			aesProvider.IV = Convert.FromBase64String(IV);

			ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
				aesProvider.Key,
				aesProvider.IV
			);
			MemoryStream decryptionStream = new MemoryStream(fileBytes);
			CryptoStream cryptoStream = new CryptoStream(
				decryptionStream,
				cryptoTransform,
				CryptoStreamMode.Read
			);
			StreamReader reader = new StreamReader(cryptoStream);

			string result = reader.ReadToEnd();

			Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");
			return JsonConvert.DeserializeObject<T>(result);
		}
	}
}