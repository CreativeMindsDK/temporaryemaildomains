using System;

namespace CreativeMinds.TemporaryEmailDomains.Configuration {

	public class TemporaryEmailDomainsSettingsReader : ITemporaryEmailDomainsSettings {
		public String DownloadPath { get; set; }
		public String LocalStoragePath { get; set; }
	}
}
