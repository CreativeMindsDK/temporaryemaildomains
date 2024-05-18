using CreativeMinds.TemporaryEmailDomains.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeMinds.TemporaryEmailDomains {

	public static class ServiceCollectionExtensions {

		public static IServiceCollection AddDefaultTemporaryEmailDependencies(this IServiceCollection services) {
			services.AddSingleton<ITemporaryEmailDomainService, TemporaryEmailDomainService>();

			services.AddHttpClient();

			services.AddTransient<ITemporaryEmailDomainsSettings, TemporaryEmailDomainsDefaultSettings>();

			return services;
		}

		public static IServiceCollection AddTemporaryEmailDependencies(this IServiceCollection services, IConfiguration configuration) {
			services.AddSingleton<ITemporaryEmailDomainService, TemporaryEmailDomainService>();

			services.AddHttpClient();

			services.Configure<TemporaryEmailDomainsSettingsReader>(configuration.GetSection("TemporaryEmailDomains"));
			services.AddTransient<ITemporaryEmailDomainsSettings, TemporaryEmailDomainsSettingsBridge>();

			return services;
		}
	}
}
