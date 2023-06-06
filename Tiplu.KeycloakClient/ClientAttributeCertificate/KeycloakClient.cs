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

using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Keycloak.Net.Models.ClientAttributeCertificate;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<Certificate> GetKeyInfoAsync(string authenticationRealm, string realm, string clientId, string attribute, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/certificates/{attribute}")
            .GetJsonAsync<Certificate>(cancellationToken)
            .ConfigureAwait(false);

        public async Task<byte[]> GetKeyStoreForClientAsync(string authenticationRealm, string realm, string clientId, string attribute, KeyStoreConfig keyStoreConfig, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/certificates/{attribute}/download")
            .PostJsonAsync(keyStoreConfig, cancellationToken)
            .ReceiveBytes()
            .ConfigureAwait(false);

        public async Task<Certificate> GenerateCertificateWithNewKeyPairAsync(string authenticationRealm, string realm, string clientId, string attribute, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/certificates/{attribute}/generate")
            .PostAsync(new StringContent(""), cancellationToken)
            .ReceiveJson<Certificate>()
            .ConfigureAwait(false);

        public async Task<byte[]> GenerateCertificateWithNewKeyPairAndGetKeyStoreAsync(string authenticationRealm, string realm, string clientId, string attribute, KeyStoreConfig keyStoreConfig, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/certificates/{attribute}/generate-and-download")
            .PostJsonAsync(keyStoreConfig, cancellationToken)
            .ReceiveBytes()
            .ConfigureAwait(false);

        public async Task<Certificate> UploadCertificateWithPrivateKeyAsync(string authenticationRealm, string realm, string clientId, string attribute, string fileName, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/certificates/{attribute}/upload")
            .PostMultipartAsync(content => content.AddFile(Path.GetFileName(fileName), Path.GetDirectoryName(fileName)), cancellationToken)
            .ReceiveJson<Certificate>()
            .ConfigureAwait(false);

        public async Task<Certificate> UploadCertificateWithoutPrivateKeyAsync(string authenticationRealm, string realm, string clientId, string attribute, string fileName, CancellationToken cancellationToken = default) => await GetBaseUrl(authenticationRealm)
            .AppendPathSegment($"/admin/realms/{realm}/clients/{clientId}/certificates/{attribute}/upload-certificate")
            .PostMultipartAsync(content => content.AddFile(Path.GetFileName(fileName), Path.GetDirectoryName(fileName)), cancellationToken)
            .ReceiveJson<Certificate>()
            .ConfigureAwait(false);
    }
}
