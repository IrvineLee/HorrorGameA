
namespace Personal.Interface
{
	public interface IProcess
	{
		void Begin(bool isFlag);
		bool IsCompleted();
	}
}