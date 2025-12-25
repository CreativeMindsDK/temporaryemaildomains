using System;

namespace CreativeMinds.TemporaryEmailDomains.Configuration {

	public class TemporaryEmailDomainsDefaultSettings : ITemporaryEmailDomainsSettings {
		public String DownloadPath => "https://raw.githubusercontent.com/CreativeMindsDK/temporaryemaildomains/main/src/CreativeMinds.TemporaryEmailDomains/Assets/tempdomains.txt";
		public String LocalStoragePath => "app_data";
	}
}
