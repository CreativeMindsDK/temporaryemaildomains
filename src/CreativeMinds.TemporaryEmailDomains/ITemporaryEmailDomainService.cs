using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.TemporaryEmailDomains {

	public interface ITemporaryEmailDomainService {
		Task<Boolean> IsDomainDisposableOrTemporaryAsync(String domain, CancellationToken cancellationToken);
	}
}
