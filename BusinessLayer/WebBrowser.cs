namespace BusinessLayer
{
    public class WebBrowser
	{
		public BrowserName Name { get; set; }
		public int MajorVersion { get; set; }

		public WebBrowser(string name, int majorVersion)
		{
			Name = TranslateStringToBrowserName(name);
			MajorVersion = majorVersion;
		}

		private BrowserName TranslateStringToBrowserName(string name)
		{
			if (name.Contains("IE")) return BrowserName.InternetExplorer;
			if (name.Contains("Firefox")) return BrowserName.Firefox;
			if (name.Contains("Chrome")) return BrowserName.Chrome;
			if (name.Contains("Opera")) return BrowserName.Opera;
			if (name.Contains("Safari")) return BrowserName.Safari;
			if (name.Contains("Dolphin")) return BrowserName.Dolphin;
			if (name.Contains("Konqueror")) return BrowserName.Konqueror;
			if (name.Contains("Linx")) return BrowserName.Linx;
			return BrowserName.Unknown;
		}

		public enum BrowserName
		{
			Unknown,
			InternetExplorer,
			Firefox,
			Chrome,
			Opera,
			Safari,
			Dolphin,
			Konqueror,
			Linx
		}
	}
}
