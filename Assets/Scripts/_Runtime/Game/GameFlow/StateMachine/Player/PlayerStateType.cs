
namespace Personal.FSM.Character
{
	public enum PlayerStateType
	{
		Idle = 0,           // When the player is not doing anything.
		Standard,           // Default fps movement/look control.
		MoveTo,             // Move to target.
		LookAt,             // Turn to look at target, with no fps control during this time.
		POVControl,         // Forcefully look at target with limited look control during this time.
		Cashier,            // Movement locked at a place with a degree of look control during this time.
	}
}