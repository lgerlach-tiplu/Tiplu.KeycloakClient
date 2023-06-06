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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Keycloak.Net.Common.Extensions;
using Keycloak.Net.Models.Groups;
using Keycloak.Net.Models.Users;
using Newtonsoft.Json;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<bool> CreateUserAsync(string authenticationRealm, string realm, User user, CancellationToken cancellationToken = default)
        {
            var response = await InternalCreateUserAsync(authenticationRealm, realm, user, cancellationToken).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        private async Task<HttpResponseMessage> InternalCreateUserAsync(string authenticationRealm, string realm, User user, CancellationToken cancellationToken)
        {
            var response = await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users")
            .PostJsonAsync(user, cancellationToken)
            .ConfigureAwait(false);

            return response.ResponseMessage;
        }

        public async Task<string> CreateAndRetrieveUserIdAsync(string authenticationRealm, string realm, User user, CancellationToken cancellationToken = default)
        {
            var response = await InternalCreateUserAsync(authenticationRealm, realm, user, cancellationToken).ConfigureAwait(false);
            string locationPathAndQuery = response.Headers.Location.PathAndQuery;
            string userId = response.IsSuccessStatusCode ? locationPathAndQuery.Substring(locationPathAndQuery.LastIndexOf("/", StringComparison.Ordinal) + 1) : null;
            return userId;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string authenticationRealm, string realm, bool? briefRepresentation = null, string email = null, bool? exact = null, int? first = null,
            string firstName = null, string lastName = null, int? max = null, string search = null, string username = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(briefRepresentation)] = briefRepresentation,
                [nameof(email)] = email,
                [nameof(exact)] = exact,
                [nameof(first)] = first,
                [nameof(firstName)] = firstName,
                [nameof(lastName)] = lastName,
                [nameof(max)] = max,
                [nameof(search)] = search,
                [nameof(username)] = username
            };

            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<User>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetUsersCountAsync(string authenticationRealm, string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/count")
            .GetJsonAsync<int>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<User> GetUserAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/{userId}")
            .GetJsonAsync<User>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateUserAsync(string authenticationRealm, string realm, string userId, User user, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}")
                .PutJsonAsync(user, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        [Obsolete("Not working yet")]
        public async Task<string> GetUserConsentsAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default)
        {
            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/consents")
                .GetStringAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> RevokeUserConsentAndOfflineTokensAsync(string authenticationRealm, string realm, string userId, string clientId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/consents/{clientId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Credentials>> GetUserCredentialsAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default)
        {
            return await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/credentials")
                .GetJsonAsync<IEnumerable<Credentials>>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> DisableUserCredentialsAsync(string authenticationRealm, string realm, string userId, IEnumerable<string> credentialTypes, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/disable-credential-types")
                .PutJsonAsync(credentialTypes, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> SendUserUpdateAccountEmailAsync(string authenticationRealm, string realm, string userId, IEnumerable<string> requiredActions, string clientId = null, int? lifespan = null, string redirectUri = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>
            {
                ["client_id"] = clientId,
                [nameof(lifespan)] = lifespan,
                ["redirect_uri"] = redirectUri
            };

            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/execute-actions-email")
                .SetQueryParams(queryParams)
                .PutJsonAsync(requiredActions, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<FederatedIdentity>> GetUserSocialLoginsAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/federated-identity")
            .GetJsonAsync<IEnumerable<FederatedIdentity>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> AddUserSocialLoginProviderAsync(string authenticationRealm, string realm, string userId, string provider, FederatedIdentity federatedIdentity, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/federated-identity/{provider}")
                .PostJsonAsync(federatedIdentity, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveUserSocialLoginProviderAsync(string authenticationRealm, string realm, string userId, string provider, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/federated-identity/{provider}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/groups")
            .GetJsonAsync<IEnumerable<Group>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<int> GetUserGroupsCountAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default)
        {
            var result = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/groups/count")
                .GetJsonAsync(cancellationToken)
                .ConfigureAwait(false);
            return Convert.ToInt32(DynamicExtensions.GetFirstPropertyValue(result));
        }

        public async Task<bool> UpdateUserGroupAsync(string authenticationRealm, string realm, string userId, string groupId, Group group, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/groups/{groupId}")
                .PutJsonAsync(group, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserGroupAsync(string authenticationRealm, string realm, string userId, string groupId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/groups/{groupId}")
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IDictionary<string, object>> ImpersonateUserAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/impersonation")
            .PostAsync(new StringContent(""), cancellationToken)
            .ReceiveJson<IDictionary<string, object>>()
            .ConfigureAwait(false);

        public async Task<bool> RemoveUserSessionsAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/logout")
                .PostAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        [Obsolete("Not working yet")]
        public async Task<IEnumerable<UserSession>> GetUserOfflineSessionsAsync(string authenticationRealm, string realm, string userId, string clientId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/offline-sessions/{clientId}")
            .GetJsonAsync<IEnumerable<UserSession>>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> RemoveUserTotpAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/remove-totp")
                .PutAsync(new StringContent(""), cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> ResetUserPasswordAsync(string authenticationRealm, string realm, string userId, Credentials credentials, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/reset-password")
                .PutJsonAsync(credentials, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> ResetUserPasswordAsync(string authenticationRealm, string realm, string userId, string password, bool temporary = true, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await InternalResetUserPasswordAsync(authenticationRealm, realm, userId, password, temporary, cancellationToken).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        private async Task<HttpResponseMessage> InternalResetUserPasswordAsync(string authenticationRealm, string realm, string userId, string password, bool temporary, CancellationToken cancellationToken)
        {
            var credentials = new Credentials
            {
                Value = password,
                Temporary = temporary
            };
            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/reset-password")
                .PutJsonAsync(credentials, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage;
        }

        public async Task<SetPasswordResponse> SetUserPasswordAsync(string authenticationRealm, string realm, string userId, string password, CancellationToken cancellationToken = default)
        {
            var response = await InternalResetUserPasswordAsync(authenticationRealm, realm, userId, password, false, cancellationToken);
            if (response.IsSuccessStatusCode)
                return new SetPasswordResponse { Success = response.IsSuccessStatusCode };

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SetPasswordResponse>(jsonString);
        }

        public async Task<bool> VerifyUserEmailAddressAsync(string authenticationRealm, string realm, string userId, string clientId = null, string redirectUri = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(clientId))
            {
                queryParams.Add("client_id", clientId);
            }

            if (!string.IsNullOrEmpty(redirectUri))
            {
                queryParams.Add("redirect_uri", redirectUri);
            }

            var response = await GetBaseUrl(authenticationRealm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/send-verify-email")
                .SetQueryParams(queryParams)
                .PutJsonAsync(null, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserSession>> GetUserSessionsAsync(string authenticationRealm, string realm, string userId, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/sessions")
            .GetJsonAsync<IEnumerable<UserSession>>(cancellationToken)
            .ConfigureAwait(false);
    }
}
