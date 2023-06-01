
namespace Personal.FSM
{
	public interface IFSMHandler
	{
		void OnBegin();
		void OnExit();
	}
}