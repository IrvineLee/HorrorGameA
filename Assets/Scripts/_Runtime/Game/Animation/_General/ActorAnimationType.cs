
namespace Personal.Character.Animation
{
	// This should contain the basic animation for a character.
	// The real animation state name should be defined in another script.
	// Ex: Character_A with Idle_01 can have animation state name of Idle_Stand where else
	// Character_B with Idle_01 can have animation state name of Idle_Sit.
	public enum ActorAnimationType
	{
		None = 0,
		Idle_01 = 1,

		Walk_01 = 1000,
	}
}