﻿// MIT License
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Keycloak.Net.Models.Common;
using Keycloak.Net.Models.IdentityProviders;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<IDictionary<string, object>> ImportIdentityProviderAsync(string authenticationRealm, string realm, string input, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/import-config")
            .PostMultipartAsync(content => content.AddFile(Path.GetFileName(input), Path.GetDirectoryName(input)), cancellationToken)
            .ReceiveJson<IDictionary<string, object>>()
            .ConfigureAwait(false);

        public async Task<bool> CreateIdentityProviderAsync(string authenticationRealm, string realm, IdentityProvider identityProvider, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances")
                .PostJsonAsync(identityProvider, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<IdentityProvider>> GetIdentityProviderInstancesAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances")
            .GetJsonAsync<IEnumerable<IdentityProvider>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<IdentityProvider> GetIdentityProviderAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}")
            .GetJsonAsync<IdentityProvider>(cancellationToken)
            .ConfigureAwait(false);

        /// <summary>
        /// <see cref="https://github.com/keycloak/keycloak-documentation/blob/master/server_development/topics/identity-brokering/tokens.adoc"/>
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="identityProviderAlias"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityProviderToken> GetIdentityProviderTokenAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/auth/realms/{realm}/broker/{identityProviderAlias}/token")
            .GetJsonAsync<IdentityProviderToken>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateIdentityProviderAsync(string authenticationRealm, string realm, string identityProviderAlias, IdentityProvider identityProvider, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}")
                .PutJsonAsync(identityProvider, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteIdentityProviderAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> ExportIdentityProviderPublicBrokerConfigurationAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/export")
                .GetAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }
        
        public async Task<ManagementPermission> GetIdentityProviderAuthorizationPermissionsInitializedAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/management/permissions")
            .GetJsonAsync<ManagementPermission>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<ManagementPermission> SetIdentityProviderAuthorizationPermissionsInitializedAsync(string authenticationRealm, string realm, string identityProviderAlias, ManagementPermission managementPermission, CancellationToken cancellationToken = default) => 
            await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/management/permissions")
                .PutJsonAsync(managementPermission, cancellationToken)
                .ReceiveJson<ManagementPermission>()
                .ConfigureAwait(false);
        
        public async Task<IDictionary<string, object>> GetIdentityProviderMapperTypesAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/mapper-types")
            .GetJsonAsync<IDictionary<string, object>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> AddIdentityProviderMapperAsync(string authenticationRealm, string realm, string identityProviderAlias, IdentityProviderMapper identityProviderMapper, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/mappers")
                .PostJsonAsync(identityProviderMapper, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }
        
        public async Task<IEnumerable<IdentityProviderMapper>> GetIdentityProviderMappersAsync(string authenticationRealm, string realm, string identityProviderAlias, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/mappers")
            .GetJsonAsync<IEnumerable<IdentityProviderMapper>>(cancellationToken)
            .ConfigureAwait(false);
        
        public async Task<IdentityProviderMapper> GetIdentityProviderMapperByIdAsync(string authenticationRealm, string realm, string identityProviderAlias, string mapperId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/mappers/{mapperId}")
            .GetJsonAsync<IdentityProviderMapper>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateIdentityProviderMapperAsync(string authenticationRealm, string realm, string identityProviderAlias, string mapperId, IdentityProviderMapper identityProviderMapper, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/mappers/{mapperId}")
                .PutJsonAsync(identityProviderMapper, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteIdentityProviderMapperAsync(string authenticationRealm, string realm, string identityProviderAlias, string mapperId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/identity-provider/instances/{identityProviderAlias}/mappers/{mapperId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IdentityProviderInfo> GetIdentityProviderByProviderIdAsync(string authenticationRealm, string realm, string providerId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/identity-provider/providers/{providerId}")
            .GetJsonAsync<IdentityProviderInfo>(cancellationToken)
            .ConfigureAwait(false);
    }
}