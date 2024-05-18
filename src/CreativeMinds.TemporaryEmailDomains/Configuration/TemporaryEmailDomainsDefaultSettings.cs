using System;
using System.IO;

namespace CreativeMinds.TemporaryEmailDomains.Configuration {

	public class TemporaryEmailDomainsDefaultSettings : ITemporaryEmailDomainsSettings {
		private String localPath = Path.Combine("app_data", "tempdomains.txt");

		public String DownloadPath => "";
		public String LocalStoragePath => this.localPath;
	}
}
