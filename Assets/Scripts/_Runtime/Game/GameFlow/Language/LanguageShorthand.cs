
namespace Personal.Localization
{
	public static class LanguageShorthand
	{
		/// <summary>
		/// This is typically used for Dialogue Manager's shorthand.
		/// </summary>
		/// <param name="language">SupportedLanguageType</param>
		/// <returns></returns>
		public static string Get(string language)
		{
			switch (language)
			{
				case ("Japanese"): return "ja";
				default: return "en";
			}
		}
	}
}