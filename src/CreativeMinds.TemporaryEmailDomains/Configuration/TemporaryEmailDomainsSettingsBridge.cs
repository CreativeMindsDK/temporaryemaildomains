using Microsoft.Extensions.Options;
using System;

namespace CreativeMinds.TemporaryEmailDomains.Configuration {

	public class TemporaryEmailDomainsSettingsBridge : ITemporaryEmailDomainsSettings {
		private IOptionsMonitor<TemporaryEmailDomainsSettingsReader> options;

		public TemporaryEmailDomainsSettingsBridge(IOptionsMonitor<TemporaryEmailDomainsSettingsReader> options) {
			this.options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public String DownloadPath => this.options.CurrentValue.DownloadPath;
		public String LocalStoragePath => this.options.CurrentValue.LocalStoragePath;
	}
}
