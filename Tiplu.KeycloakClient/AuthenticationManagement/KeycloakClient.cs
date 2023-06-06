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
using Keycloak.Net.Models.AuthenticationManagement;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<IEnumerable<IDictionary<string, object>>> GetAuthenticatorProvidersAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/authenticator-providers")
            .GetJsonAsync<IEnumerable<IDictionary<string, object>>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<IEnumerable<IDictionary<string, object>>> GetClientAuthenticatorProvidersAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/client-authenticator-providers")
            .GetJsonAsync<IEnumerable<IDictionary<string, object>>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<AuthenticatorConfigInfo> GetAuthenticatorProviderConfigurationDescriptionAsync(string authenticationRealm, string realm, string providerId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/config-description/{providerId}")
            .GetJsonAsync<AuthenticatorConfigInfo>(cancellationToken)
            .ConfigureAwait(false);

        [Obsolete("Not working yet")]
        public async Task<AuthenticatorConfig> GetAuthenticatorConfigurationAsync(string authenticationRealm, string realm, string configurationId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/config/{configurationId}")
            .GetJsonAsync<AuthenticatorConfig>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateAuthenticatorConfigurationAsync(string authenticationRealm, string realm, string configurationId, AuthenticatorConfig authenticatorConfig, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/config/{configurationId}")
                .PutJsonAsync(authenticatorConfig, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAuthenticatorConfigurationAsync(string authenticationRealm, string realm, string configurationId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/config/{configurationId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> AddAuthenticationExecutionAsync(string authenticationRealm, string realm, AuthenticationExecution authenticationExecution, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/executions")
                .PostJsonAsync(authenticationExecution, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<AuthenticationExecutionById> GetAuthenticationExecutionAsync(string authenticationRealm, string realm, string executionId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/executions/{executionId}")
            .GetJsonAsync<AuthenticationExecutionById>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> DeleteAuthenticationExecutionAsync(string authenticationRealm, string realm, string executionId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/executions/{executionId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAuthenticationExecutionConfigurationAsync(string authenticationRealm, string realm, string executionId, AuthenticatorConfig authenticatorConfig, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/executions/{executionId}/config")
                .PostJsonAsync(authenticatorConfig, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> LowerAuthenticationExecutionPriorityAsync(string authenticationRealm, string realm, string executionId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/executions/{executionId}/lower-priority")
                .PostAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> RaiseAuthenticationExecutionPriorityAsync(string authenticationRealm, string realm, string executionId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/executions/{executionId}/raise-priority")
                .PostAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> CreateAuthenticationFlowAsync(string authenticationRealm, string realm, AuthenticationFlow authenticationFlow, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows")
                .PostJsonAsync(authenticationFlow, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<AuthenticationFlow>> GetAuthenticationFlowsAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/flows")
            .GetJsonAsync<IEnumerable<AuthenticationFlow>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> DuplicateAuthenticationFlowAsync(string authenticationRealm, string realm, string flowAlias, string newName, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowAlias}/copy")
                .PostJsonAsync(new Dictionary<string, object> { [nameof(newName)] = newName }, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<AuthenticationFlowExecution>> GetAuthenticationFlowExecutionsAsync(string authenticationRealm, string realm, string flowAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowAlias}/executions")
            .GetJsonAsync<IEnumerable<AuthenticationFlowExecution>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateAuthenticationFlowExecutionsAsync(string authenticationRealm, string realm, string flowAlias, AuthenticationExecutionInfo authenticationExecutionInfo, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowAlias}/executions")
                .PutJsonAsync(authenticationExecutionInfo, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> AddAuthenticationFlowExecutionAsync(string authenticationRealm, string realm, string flowAlias, IDictionary<string, object> dataWithProvider, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowAlias}/executions/execution")
                .PostJsonAsync(dataWithProvider, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> AddAuthenticationFlowAndExecutionToAuthenticationFlowAsync(string authenticationRealm, string realm, string flowAlias, IDictionary<string, object> dataWithAliasTypeProviderDescription, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowAlias}/executions/flow")
                .PostJsonAsync(dataWithAliasTypeProviderDescription, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<AuthenticationFlow> GetAuthenticationFlowByIdAsync(string authenticationRealm, string realm, string flowId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowId}")
            .GetJsonAsync<AuthenticationFlow>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateAuthenticationFlowAsync(string authenticationRealm, string realm, string flowId, AuthenticationFlow authenticationFlow, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowId}")
                .PutJsonAsync(authenticationFlow, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAuthenticationFlowAsync(string authenticationRealm, string realm, string flowId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/flows/{flowId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetFormActionProvidersAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/form-action-providers")
            .GetJsonAsync<IEnumerable<IDictionary<string, object>>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<IEnumerable<IDictionary<string, object>>> GetFormProvidersAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/form-providers")
            .GetJsonAsync<IEnumerable<IDictionary<string, object>>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<IDictionary<string, object>> GetConfigurationDescriptionsForAllClientsAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/per-client-config-description")
            .GetJsonAsync<IDictionary<string, object>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> RegisterRequiredActionAsync(string authenticationRealm, string realm, IDictionary<string, object> dataWithProviderIdName, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/register-required-action")
                .PostJsonAsync(dataWithProviderIdName, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<RequiredActionProvider>> GetRequiredActionsAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/required-actions")
            .GetJsonAsync<IEnumerable<RequiredActionProvider>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<RequiredActionProvider> GetRequiredActionByAliasAsync(string authenticationRealm, string realm, string requiredActionAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/required-actions/{requiredActionAlias}")
            .GetJsonAsync<RequiredActionProvider>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateRequiredActionAsync(string authenticationRealm, string realm, string requiredActionAlias, RequiredActionProvider requiredActionProvider, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/required-actions/{requiredActionAlias}")
                .PutJsonAsync(requiredActionProvider, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRequiredActionAsync(string authenticationRealm, string realm, string requiredActionAlias, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/required-actions/{requiredActionAlias}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> LowerRequiredActionPriorityAsync(string authenticationRealm, string realm, string requiredActionAlias, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/required-actions/{requiredActionAlias}/lower-priority")
                .PostAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> RaiseRequiredActionPriorityAsync(string authenticationRealm, string realm, string requiredActionAlias, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/authentication/required-actions/{requiredActionAlias}/raise-priority")
                .PostAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetUnregisteredRequiredActionsAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/authentication/unregistered-required-actions")
            .GetJsonAsync<IEnumerable<IDictionary<string, object>>>(cancellationToken)
            .ConfigureAwait(false);
    }
}
