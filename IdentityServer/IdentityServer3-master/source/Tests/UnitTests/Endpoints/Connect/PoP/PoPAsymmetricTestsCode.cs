﻿/*
 * Copyright 2014, 2015 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using FluentAssertions;
using IdentityModel;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Tests.Conformance;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace IdentityServer3.Tests.Endpoints.Connect.PoP
{
    public class PoP_Asymmetrc_Tests_Code : IdentityServerHostTest
    {
        const string Category = "Endpoints.PoP.Asymmetric.Code";

        string client_id = "code_client";
        string client_id_reference = "code_client_reference";
        string redirect_uri = "https://code_client/callback";
        string client_secret = "secret";

        protected override void PreInit()
        {
            var api = new Scope
            {
                Name = "api",
                Type = ScopeType.Resource,

                ScopeSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                }
            };

            host.Scopes.Add(StandardScopes.OpenId);
            host.Scopes.Add(api);

            host.Clients.Add(new Client
            {
                Enabled = true,
                ClientId = client_id,
                ClientSecrets = new List<Secret>
                {
                    new Secret(client_secret.Sha256())
                },

                Flow = Flows.AuthorizationCode,
                AllowAccessToAllScopes = true,

                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    redirect_uri
                }
            });

            host.Clients.Add(new Client
            {
                Enabled = true,
                ClientId = client_id_reference,
                ClientSecrets = new List<Secret>
                {
                    new Secret(client_secret.Sha256())
                },

                Flow = Flows.AuthorizationCode,
                AllowAccessToAllScopes = true,
                AccessTokenType = AccessTokenType.Reference,

                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    redirect_uri
                }
            });
        }

        [Fact]
        [Trait("Category", Category)]
        public void Valid_Asymmetric_Key()
        {
            host.Login();

            var nonce = Guid.NewGuid().ToString();
            var query = host.RequestAuthorizationCode(client_id, redirect_uri, "openid", nonce);
            var code = query["code"];

            host.NewRequest();
            host.Client.SetBasicAuthentication(client_id, client_secret);

            var jwk = Helper.CreateJwk();
            var key = Helper.CreateJwkString(jwk);

            var result = host.PostForm(host.GetTokenUrl(),
                new
                {
                    grant_type = "authorization_code",
                    code,
                    redirect_uri,
                    token_type = "pop",
                    alg = "RS256",
                    key
                }
            );

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.CacheControl.NoCache.Should().BeTrue();
            result.Headers.CacheControl.NoStore.Should().BeTrue();

            var data = result.ReadJsonObject();
            data["token_type"].Should().NotBeNull();
            data["token_type"].ToString().Should().Be("pop");

            data["alg"].ToString().Should().NotBeNull();
            data["alg"].ToString().Should().Be("RS256");

            data["access_token"].Should().NotBeNull();
            data["expires_in"].Should().NotBeNull();
            data["id_token"].Should().NotBeNull();

            var payload = data["access_token"].ToString().Split('.')[1];
            var json = Encoding.UTF8.GetString(Base64Url.Decode(payload));
            var claims = JObject.Parse(json);

            claims["cnf"].Should().NotBeNull();
            var jjwk = claims["cnf"]["jwk"];
            
            jjwk["kty"].ToString().Should().Be("RSA");
            jjwk["e"].ToString().Should().Be(jwk.e);
            jjwk["n"].ToString().Should().Be(jwk.n);
            jjwk["alg"].ToString().Should().Be("RS256");
        }

        [Fact]
        [Trait("Category", Category)]
        public void Valid_Asymmetric_Key_Reference()
        {
            host.Login();

            var nonce = Guid.NewGuid().ToString();
            var query = host.RequestAuthorizationCode(client_id_reference, redirect_uri, "openid api", nonce);
            var code = query["code"];

            host.NewRequest();
            host.Client.SetBasicAuthentication(client_id_reference, client_secret);

            var jwk = Helper.CreateJwk();
            var key = Helper.CreateJwkString(jwk);

            var result = host.PostForm(host.GetTokenUrl(),
                new
                {
                    grant_type = "authorization_code",
                    code,
                    redirect_uri,
                    token_type = "pop",
                    alg = "RS256",
                    key
                }
            );

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.CacheControl.NoCache.Should().BeTrue();
            result.Headers.CacheControl.NoStore.Should().BeTrue();

            var data = result.ReadJsonObject();
            data["token_type"].Should().NotBeNull();
            data["token_type"].ToString().Should().Be("pop");

            data["alg"].ToString().Should().NotBeNull();
            data["alg"].ToString().Should().Be("RS256");

            data["access_token"].Should().NotBeNull();
            data["expires_in"].Should().NotBeNull();
            data["id_token"].Should().NotBeNull();

            var referenceToken = data["access_token"].ToString();

            host.NewRequest();
            var introspectionResponse = host.Introspect("api", "secret", referenceToken);

            introspectionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            data = introspectionResponse.ReadJsonObject();

            data["cnf"].Should().NotBeNull();
            var jjwk = data["cnf"]["jwk"];

            jjwk["kty"].ToString().Should().Be("RSA");
            jjwk["e"].ToString().Should().Be(jwk.e);
            jjwk["n"].ToString().Should().Be(jwk.n);
            jjwk["alg"].ToString().Should().Be("RS256");
        }
        
        [Fact]
        [Trait("Category", Category)]
        public void No_Alg()
        {
            host.Login();

            var nonce = Guid.NewGuid().ToString();
            var query = host.RequestAuthorizationCode(client_id, redirect_uri, "openid", nonce);
            var code = query["code"];

            host.NewRequest();
            host.Client.SetBasicAuthentication(client_id, client_secret);

            var jwk = Helper.CreateJwk();
            var key = Helper.CreateJwkString(jwk);

            var result = host.PostForm(host.GetTokenUrl(),
                new
                {
                    grant_type = "authorization_code",
                    code,
                    redirect_uri,
                    token_type = "pop",
                    key
                }
            );

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.CacheControl.NoCache.Should().BeTrue();
            result.Headers.CacheControl.NoStore.Should().BeTrue();

            var data = result.ReadJsonObject();
            data["token_type"].Should().NotBeNull();
            data["token_type"].ToString().Should().Be("pop");

            data["alg"].Should().BeNull();

            data["access_token"].Should().NotBeNull();
            data["expires_in"].Should().NotBeNull();
            data["id_token"].Should().NotBeNull();
        }

        [Fact]
        [Trait("Category", Category)]
        public void Invalid_Alg()
        {
            host.Login();

            var nonce = Guid.NewGuid().ToString();
            var query = host.RequestAuthorizationCode(client_id, redirect_uri, "openid", nonce);
            var code = query["code"];

            host.NewRequest();
            host.Client.SetBasicAuthentication(client_id, client_secret);

            var jwk = Helper.CreateJwk();
            var key = Helper.CreateJwkString(jwk);

            var result = host.PostForm(host.GetTokenUrl(),
                new
                {
                    grant_type = "authorization_code",
                    code,
                    redirect_uri,
                    token_type = "pop",
                    key,
                    alg = "none"
                }
            );

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var data = result.ReadJsonObject();
            data["error"].ToString().Should().Be(Constants.TokenErrors.InvalidRequest);
            data["error_description"].ToString().Should().Be("invalid alg.");

        }

        [Fact]
        [Trait("Category", Category)]
        public void No_Key()
        {
            host.Login();

            var nonce = Guid.NewGuid().ToString();
            var query = host.RequestAuthorizationCode(client_id, redirect_uri, "openid", nonce);
            var code = query["code"];

            host.NewRequest();
            host.Client.SetBasicAuthentication(client_id, client_secret);

            var result = host.PostForm(host.GetTokenUrl(),
                new
                {
                    grant_type = "authorization_code",
                    code,
                    redirect_uri,
                    token_type = "pop",
                    alg = "RS256"
                }
            );

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var data = result.ReadJsonObject();
            data["error"].ToString().Should().Be(Constants.TokenErrors.InvalidRequest);
            data["error_description"].ToString().Should().Be("key is required.");
        }

        [Fact]
        [Trait("Category", Category)]
        public void Key_too_long()
        {
            host.Login();

            var nonce = Guid.NewGuid().ToString();
            var query = host.RequestAuthorizationCode(client_id, redirect_uri, "openid", nonce);
            var code = query["code"];

            host.NewRequest();
            host.Client.SetBasicAuthentication(client_id, client_secret);

            var result = host.PostForm(host.GetTokenUrl(),
                new
                {
                    grant_type = "authorization_code",
                    code,
                    redirect_uri,
                    token_type = "pop",
                    alg = "RS256",
                    key = "a".Repeat(3000)
                }
            );

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var data = result.ReadJsonObject();
            data["error"].ToString().Should().Be(Constants.TokenErrors.InvalidRequest);
            data["error_description"].ToString().Should().Be("invalid key.");
        }
    }
}