// MIT License
//
// Copyright (c) 2019 Luk Vermeulen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Keycloak.Net.Models.Common;
using Keycloak.Net.Models.Roles;

namespace Keycloak.Net
{
	public partial class KeycloakClient
	{
		public async Task<Mapping> GetScopeMappingsAsync(string authenticationRealm, string realm, string clientScopeId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings")
			.GetJsonAsync<Mapping>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> AddClientRolesToClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, string clientId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/clients/{clientId}")
				.PostJsonAsync(roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetClientRolesForClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/clients/{clientId}")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> RemoveClientRolesFromClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, string clientId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/clients/{clientId}")
				.SendJsonAsync(HttpMethod.Delete, roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetAvailableClientRolesForClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/clients/{clientId}/available")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<IEnumerable<Role>> GetEffectiveClientRolesForClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/clients/{clientId}/composite")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> AddRealmRolesToClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/realm")
				.PostJsonAsync(roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetRealmRolesForClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/realm")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> RemoveRealmRolesFromClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/realm")
				.SendJsonAsync(HttpMethod.Delete, roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetAvailableRealmRolesForClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/realm/available")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<IEnumerable<Role>> GetEffectiveRealmRolesForClientScopeAsync(string authenticationRealm, string realm, string clientScopeId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}/scope-mappings/realm/composite")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<Mapping> GetScopeMappingsForClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings")
			.GetJsonAsync<Mapping>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> AddClientRolesScopeMappingToClientAsync(string authenticationRealm, string realm, string clientId, string scopeClientId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/clients/{scopeClientId}")
				.PostJsonAsync(roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetClientRolesScopeMappingsForClientAsync(string authenticationRealm, string realm, string clientId, string scopeClientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/clients/{scopeClientId}")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> RemoveClientRolesFromClientScopeForClientAsync(string authenticationRealm, string realm, string clientId, string scopeClientId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/clients/{scopeClientId}")
				.SendJsonAsync(HttpMethod.Delete, roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetAvailableClientRolesForClientScopeForClientAsync(string authenticationRealm, string realm, string clientId, string scopeClientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/clients/{scopeClientId}/available")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<IEnumerable<Role>> GetEffectiveClientRolesForClientScopeForClientAsync(string authenticationRealm, string realm, string clientId, string scopeClientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/clients/{scopeClientId}/composite")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> AddRealmRolesScopeMappingToClientAsync(string authenticationRealm, string realm, string clientId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/realm")
				.PostJsonAsync(roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetRealmRolesScopeMappingsForClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/realm")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<bool> RemoveRealmRolesFromClientScopeForClientAsync(string authenticationRealm, string realm, string clientId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
		{
			var response = await GetBaseUrl(authenticationRealm)
				.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/realm")
				.SendJsonAsync(HttpMethod.Delete, roles, cancellationToken)
				.ConfigureAwait(false);
			return response.ResponseMessage.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<Role>> GetAvailableRealmRolesForClientScopeForClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/realm/available")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);

		public async Task<IEnumerable<Role>> GetEffectiveRealmRolesForClientScopeForClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
			.AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/scope-mappings/realm/composite")
			.GetJsonAsync<IEnumerable<Role>>(cancellationToken)
			.ConfigureAwait(false);
	}
}
