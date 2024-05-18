using System;

namespace CreativeMinds.TemporaryEmailDomains.Configuration {

	public interface ITemporaryEmailDomainsSettings {
		String DownloadPath { get; }
		String LocalStoragePath { get; }
	}
}
