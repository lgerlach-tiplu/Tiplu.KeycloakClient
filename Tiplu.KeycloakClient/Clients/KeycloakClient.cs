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

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Keycloak.Net.Common.Extensions;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.ClientScopes;
using Keycloak.Net.Models.Common;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Root;
using Keycloak.Net.Models.Users;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<bool> CreateClientAsync(string authenticationRealm, string realm, Client client, CancellationToken cancellationToken = default)
        {
            var response = await InternalCreateClientAsync(authenticationRealm,realm, client, cancellationToken).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<string> CreateClientAndRetrieveClientIdAsync(string authenticationRealm, string realm, Client client, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await InternalCreateClientAsync(authenticationRealm,realm, client, cancellationToken).ConfigureAwait(false);

            var locationPathAndQuery = response.Headers.Location.PathAndQuery;
            var clientId = response.IsSuccessStatusCode ? locationPathAndQuery.Substring(locationPathAndQuery.LastIndexOf("/", StringComparison.Ordinal) + 1) : null;
            return clientId;
        }

        private async Task<HttpResponseMessage> InternalCreateClientAsync(string authenticationRealm, string realm, Client client, CancellationToken cancellationToken)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients")
                .PostJsonAsync(client, cancellationToken)
                .ConfigureAwait(false);

            return response.ResponseMessage;
        }

        public async Task<IEnumerable<Client>> GetClientsAsync(string authenticationRealm, string realm, string clientId = null, bool? viewableOnly = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(clientId)] = clientId,
                [nameof(viewableOnly)] = viewableOnly
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<Client>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Client> GetClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}")
            .GetJsonAsync<Client>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateClientAsync(string authenticationRealm, string realm, string clientId, Client client, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}")
                .PutJsonAsync(client, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<Credentials> GenerateClientSecretAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/client-secret")
            .PostJsonAsync(new StringContent(""), cancellationToken)
            .ReceiveJson<Credentials>()
            .ConfigureAwait(false);

        public async Task<Credentials> GetClientSecretAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/client-secret")
            .GetJsonAsync<Credentials>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<IEnumerable<ClientScope>> GetDefaultClientScopesAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/default-client-scopes")
            .GetJsonAsync<IEnumerable<ClientScope>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateDefaultClientScopeAsync(string authenticationRealm, string realm, string clientId, string clientScopeId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/default-client-scopes/{clientScopeId}")
                .PutAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDefaultClientScopeAsync(string authenticationRealm, string realm, string clientId, string clientScopeId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/default-client-scopes/{clientScopeId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        [Obsolete("Not working yet")]
        public async Task<AccessToken> GenerateClientExampleAccessTokenAsync(string authenticationRealm, string realm, string clientId, string scope = null, string userId = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(scope)] = scope,
                [nameof(userId)] = userId
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/evaluate-scopes/generate-example-access-token")
                .SetQueryParams(queryParams)
                .GetJsonAsync<AccessToken>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClientScopeEvaluateResourceProtocolMapperEvaluation>> GetProtocolMappersInTokenGenerationAsync(string authenticationRealm, string realm, string clientId, string scope = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(scope)] = scope
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/evaluate-scopes/protocol-mappers")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<ClientScopeEvaluateResourceProtocolMapperEvaluation>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Role>> GetClientGrantedScopeMappingsAsync(string authenticationRealm, string realm, string clientId, string roleContainerId, string scope = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(scope)] = scope
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/evaluate-scopes/scope-mappings/{roleContainerId}/granted")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<Role>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Role>> GetClientNotGrantedScopeMappingsAsync(string authenticationRealm, string realm, string clientId, string roleContainerId, string scope = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(scope)] = scope
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/evaluate-scopes/scope-mappings/{roleContainerId}/not-granted")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<Role>>(cancellationToken)
                .ConfigureAwait(false);
        }

        [Obsolete("Not working yet")]
        public async Task<string> GetClientProviderAsync(string authenticationRealm, string realm, string clientId, string providerId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/installation/providers/{providerId}")
            .GetStringAsync(cancellationToken)
            .ConfigureAwait(false);

        public async Task<ManagementPermission> GetClientAuthorizationPermissionsInitializedAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/management/permissions")
            .GetJsonAsync<ManagementPermission>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<ManagementPermission> SetClientAuthorizationPermissionsInitializedAsync(string authenticationRealm, string realm, string clientId, ManagementPermission managementPermission, CancellationToken cancellationToken = default) =>
            await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/management/permissions")
                .PutJsonAsync(managementPermission, cancellationToken)
                .ReceiveJson<ManagementPermission>()
                .ConfigureAwait(false);

        public async Task<bool> RegisterClientClusterNodeAsync(string authenticationRealm, string realm, string clientId, IDictionary<string, object> formParams, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/nodes")
                .PostJsonAsync(formParams, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> UnregisterClientClusterNodeAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/nodes")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<int> GetClientOfflineSessionCountAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default)
        {
            var result = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/offline-session-count")
                .GetJsonAsync(cancellationToken)
                .ConfigureAwait(false);

            return Convert.ToInt32(DynamicExtensions.GetFirstPropertyValue(result));
        }

        public async Task<IEnumerable<UserSession>> GetClientOfflineSessionsAsync(string authenticationRealm, string realm, string clientId, int? first = null, int? max = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(first)] = first,
                [nameof(max)] = max
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/offline-sessions")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<UserSession>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClientScope>> GetOptionalClientScopesAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/optional-client-scopes")
            .GetJsonAsync<IEnumerable<ClientScope>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateOptionalClientScopeAsync(string authenticationRealm, string realm, string clientId, string clientScopeId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/optional-client-scopes/{clientScopeId}")
                .PutAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOptionalClientScopeAsync(string authenticationRealm, string realm, string clientId, string clientScopeId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/optional-client-scopes/{clientScopeId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<GlobalRequestResult> PushClientRevocationPolicyAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/push-revocation")
            .PostAsync(new StringContent(""), cancellationToken)
            .ReceiveJson<GlobalRequestResult>()
            .ConfigureAwait(false);

        public async Task<Client> GenerateClientRegistrationAccessTokenAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/registration-access-token")
            .PostJsonAsync(new StringContent(""), cancellationToken)
            .ReceiveJson<Client>()
            .ConfigureAwait(false);

        [Obsolete("Not working yet")]
        public async Task<User> GetUserForServiceAccountAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/service-account-user")
            .GetJsonAsync<User>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<int> GetClientSessionCountAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default)
        {
            var result = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/session-count")
                .GetJsonAsync(cancellationToken)
                .ConfigureAwait(false);

            return Convert.ToInt32(DynamicExtensions.GetFirstPropertyValue(result));
        }

        public async Task<GlobalRequestResult> TestClientClusterNodesAvailableAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/test-nodes-available")
            .GetJsonAsync<GlobalRequestResult>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<IEnumerable<UserSession>> GetClientUserSessionsAsync(string authenticationRealm, string realm, string clientId, int? first = null, int? max = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(first)] = first,
                [nameof(max)] = max
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/user-sessions")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<UserSession>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Resource>> GetResourcesOwnedByClientAsync(string authenticationRealm, string realm, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/realms/{realm}/protocol/openid-connect/token")
            .PostUrlEncodedAsync(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"),
                new KeyValuePair<string, string>("response_mode", "permissions"),
                new KeyValuePair<string, string>("audience", clientId)
            }, cancellationToken)
            .ReceiveJson<IEnumerable<Resource>>()
            .ConfigureAwait(false);
    }
}
