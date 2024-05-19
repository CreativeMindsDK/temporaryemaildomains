using CreativeMinds.TemporaryEmailDomains.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.TemporaryEmailDomains {

	public class TemporaryEmailDomainService : ITemporaryEmailDomainService {
		protected readonly ITemporaryEmailDomainsSettings settings;
		protected readonly IHostEnvironment hostEnvironment;
		protected readonly ILogger logger;
		protected readonly IHttpClientFactory clientFactory;
		protected FrozenSet<String> domains = null;

		public TemporaryEmailDomainService(IHostEnvironment hostEnvironment, IHttpClientFactory clientFactory, ITemporaryEmailDomainsSettings settings, ILogger<TemporaryEmailDomainService> logger) {
			this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
			this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
			this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Boolean> IsDomainDisposableOrTemporaryAsync(String domain, CancellationToken cancellationToken) {
			if (domains == null) {
				this.logger.LogDebug("Disposable/Temporary email domains, in-memory collection is null, let's fetch them");
				await this.FetchData(cancellationToken);
			}

			return this.domains.Contains(domain.ToLowerInvariant());
		}

		private async Task FetchData(CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			Boolean fetched = false;
			if (String.IsNullOrWhiteSpace(this.settings.LocalStoragePath) == false) {
				String fullPath = Path.Combine(this.hostEnvironment.ContentRootPath, this.settings.LocalStoragePath);
				String pathToFile = Path.Combine(fullPath, "tempdomains.txt");
				this.logger.LogDebug($"Disposable/Temporary email domains, we have a local path, {fullPath}, let's see if the file is present");

				try {
					Directory.CreateDirectory(fullPath);
					if (File.Exists(pathToFile) == true) {
						this.logger.LogDebug("Disposable/Temporary email domains, a local file with temp domains found, let's read it!");
						String[] data = File.ReadAllLines(pathToFile);
						this.domains = data.ToFrozenSet<String>();
						fetched = true;
					}
				}
				catch (Exception ex) {
					this.logger.LogError(ex, $"Trying to access the given path '{fullPath}' failed.");
				}
			}

			if (fetched == false && String.IsNullOrWhiteSpace(this.settings.DownloadPath) == false) {
				this.logger.LogDebug("Disposable/Temporary email domains, we have a download path, and no local file!");
				using var client = this.clientFactory.CreateClient();

				using (HttpResponseMessage response = await client.GetAsync($"{this.settings.DownloadPath}")) {
					response.EnsureSuccessStatusCode();
					String data = await response.Content.ReadAsStringAsync();
					if (String.IsNullOrWhiteSpace(data) == false) {
						this.logger.LogDebug("Disposable/Temporary email domains, we have email domain data, let's store it in memory and try to save on disk");
						this.domains = data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToFrozenSet();

						if (String.IsNullOrWhiteSpace(this.settings.LocalStoragePath) == false) {
							String fullPath = Path.Combine(this.hostEnvironment.ContentRootPath, this.settings.LocalStoragePath);
							String pathToFile = Path.Combine(fullPath, "tempdomains.txt");

							try {
								File.WriteAllText(pathToFile, data);
							}
							catch (Exception ex) {
								this.logger.LogError(ex, $"Trying to write to the given path '{fullPath}' failed.");
							}
						}
					}
					else {
						this.logger.LogWarning("No data found for disposable/temporary email domains");
					}
				}
			}
			else {
				this.domains = new List<String>().ToFrozenSet();
			}
		}
	}
}
