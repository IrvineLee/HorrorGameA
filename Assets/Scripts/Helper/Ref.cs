
namespace Helper
{
	public class Ref<T>
	{
		public T Value { get; set; }
		public Ref(T reference) { Value = reference; }
	}
}