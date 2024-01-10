
namespace Personal.Save
{
	public interface IDataPersistence
	{
		void SaveData(SaveObject data);
		void LoadData(SaveObject data);
	}
}
