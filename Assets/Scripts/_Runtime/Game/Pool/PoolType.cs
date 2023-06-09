using Helper;

namespace Personal.Pool
{
	public enum PoolType
	{
		// 3D pool (>= 0 < 100,000)
		[StringValue(AssetAddress.Effects_Effect_01)]
		Effect3D_Effect01 = 0,

		// 2D pool (>= 100,000)
		Effect2D_Effect01,
	}
}