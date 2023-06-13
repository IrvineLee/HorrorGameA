using System;

using Personal.Save;

namespace Personal.GameState
{
	[Serializable]
	public class GameStateBehaviour : GameInitializeSingleton<GameStateBehaviour>
	{
		/// <summary>
		/// The setting for the entire game.
		/// </summary>
		public SaveProfile SaveProfile { get; private set; }

		/// <summary>
		/// The entire savable object of the current game. This will form one "Save Slot".
		/// </summary>
		public SaveObject SaveObject { get; private set; }

		public void InitializeProfile(SaveProfile saveProfile) { SaveProfile = saveProfile; }
		public void InitializeData(SaveObject saveObject) { SaveObject = saveObject; }
	}
}