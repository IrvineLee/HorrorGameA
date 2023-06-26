using Helper;

namespace Personal.Setting.Audio
{
	public enum AudioSFXType
	{
		None = 0,

		[StringValue("SFX_01_FirstAudio")]
		SFX_01_FirstAudio,
		[StringValue("SFX_02_SecondAudio")]
		SFX_02_SecondAudio,
		[StringValue("SFX_03_ThirdAudio")]
		SFX_03_ThirdAudio,

		[StringValue("PlayerFootsteps_01")]
		PlayerFootsteps_01 = 1000,
		[StringValue("PlayerFootsteps_02")]
		PlayerFootsteps_02,
		[StringValue("PlayerFootsteps_03")]
		PlayerFootsteps_03,
		[StringValue("PlayerFootsteps_04")]
		PlayerFootsteps_04,
		[StringValue("PlayerFootsteps_05")]
		PlayerFootsteps_05,
		[StringValue("PlayerFootsteps_06")]
		PlayerFootsteps_06,
		[StringValue("PlayerFootsteps_07")]
		PlayerFootsteps_07,
		[StringValue("PlayerFootsteps_08")]
		PlayerFootsteps_08,
		[StringValue("PlayerFootsteps_09")]
		PlayerFootsteps_09,
		[StringValue("PlayerFootsteps_010")]
		PlayerFootsteps_010,
		[StringValue("PlayerLand")]
		PlayerLand,

		[StringValue("PhoneRing")]
		PhoneRing = 2000,
	}
}