using System;
using System.IO;

namespace CreativeMinds.TemporaryEmailDomains.Configuration {

	public class TemporaryEmailDomainsDefaultSettings : ITemporaryEmailDomainsSettings {
		private String localPath = "app_data";

		public String DownloadPath => "https://raw.githubusercontent.com/CreativeMindsDK/temporaryemaildomains/main/src/CreativeMinds.TemporaryEmailDomains/Assets/tempdomains.txt";
		public String LocalStoragePath => this.localPath;
	}
}
