﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Services
{
        using System = global::System;

        [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.2.3.0 (NJsonSchema v10.1.5.0 (Newtonsoft.Json v12.0.0.0))")]
        public partial class ApiDochazkaClient
        {
            private string _baseUrl = "http://webapi/dochazka/";
            private System.Net.Http.HttpClient _httpClient;
            private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

            public ApiDochazkaClient(System.Net.Http.HttpClient httpClient)
            {
                _httpClient = httpClient;
                _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
            }

            private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            }

            public string BaseUrl
            {
                get { return _baseUrl; }
                set { _baseUrl = value; }
            }

            protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

            partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);
            partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
            partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
            partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task<FileResponse> GetAsync(int id)
            {
                return GetAsync(id, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task<FileResponse> GetAsync(int id, System.Threading.CancellationToken cancellationToken)
            {
                if (id == null)
                    throw new System.ArgumentNullException("id");

                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiDochazka/Get/{id}");
                urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        request_.Method = new System.Net.Http.HttpMethod("GET");
                        request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/octet-stream"));

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200" || status_ == "206")
                            {
                                var responseStream_ = response_.Content == null ? System.IO.Stream.Null : await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                var fileResponse_ = new FileResponse((int)response_.StatusCode, headers_, responseStream_, null, response_);
                                client_ = null; response_ = null; // response and client are disposed by FileResponse
                                return fileResponse_;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }

                            return default(FileResponse);
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task<FileResponse> GetListAsync()
            {
                return GetListAsync(System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task<FileResponse> GetListAsync(System.Threading.CancellationToken cancellationToken)
            {
                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiDochazka/GetList");

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        request_.Method = new System.Net.Http.HttpMethod("GET");
                        request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/octet-stream"));

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200" || status_ == "206")
                            {
                                var responseStream_ = response_.Content == null ? System.IO.Stream.Null : await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                var fileResponse_ = new FileResponse((int)response_.StatusCode, headers_, responseStream_, null, response_);
                                client_ = null; response_ = null; // response and client are disposed by FileResponse
                                return fileResponse_;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }

                            return default(FileResponse);
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task AddAsync(DochazkaModel msg)
            {
                return AddAsync(msg, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task AddAsync(DochazkaModel msg, System.Threading.CancellationToken cancellationToken)
            {
                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiDochazka/Add");

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(msg, _settings.Value));
                        content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                        request_.Content = content_;
                        request_.Method = new System.Net.Http.HttpMethod("POST");

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                return;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task DeleteAsync(int id)
            {
                return DeleteAsync(id, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task DeleteAsync(int id, System.Threading.CancellationToken cancellationToken)
            {
                if (id == null)
                    throw new System.ArgumentNullException("id");

                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiDochazka/Remove/{id}");
                urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        request_.Method = new System.Net.Http.HttpMethod("DELETE");

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                return;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task UpdateAsync(DochazkaModel msg)
            {
                return UpdateAsync(msg, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task UpdateAsync(DochazkaModel msg, System.Threading.CancellationToken cancellationToken)
            {
                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiDochazka/Update");

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(msg, _settings.Value));
                        content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                        request_.Content = content_;
                        request_.Method = new System.Net.Http.HttpMethod("POST");

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                return;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            protected struct ObjectResponseResult<T>
            {
                public ObjectResponseResult(T responseObject, string responseText)
                {
                    this.Object = responseObject;
                    this.Text = responseText;
                }

                public T Object { get; }

                public string Text { get; }
            }

            public bool ReadResponseAsString { get; set; }

            protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers)
            {
                if (response == null || response.Content == null)
                {
                    return new ObjectResponseResult<T>(default(T), string.Empty);
                }

                if (ReadResponseAsString)
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    try
                    {
                        var typedBody = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                        return new ObjectResponseResult<T>(typedBody, responseText);
                    }
                    catch (Newtonsoft.Json.JsonException exception)
                    {
                        var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                        throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                    }
                }
                else
                {
                    try
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        using (var streamReader = new System.IO.StreamReader(responseStream))
                        using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                        {
                            var serializer = Newtonsoft.Json.JsonSerializer.Create(JsonSerializerSettings);
                            var typedBody = serializer.Deserialize<T>(jsonTextReader);
                            return new ObjectResponseResult<T>(typedBody, string.Empty);
                        }
                    }
                    catch (Newtonsoft.Json.JsonException exception)
                    {
                        var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                        throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                    }
                }
            }

            private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
            {
                if (value is System.Enum)
                {
                    string name = System.Enum.GetName(value.GetType(), value);
                    if (name != null)
                    {
                        var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                        if (field != null)
                        {
                            var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                                as System.Runtime.Serialization.EnumMemberAttribute;
                            if (attribute != null)
                            {
                                return attribute.Value != null ? attribute.Value : name;
                            }
                        }
                    }
                }
                else if (value is bool)
                {
                    return System.Convert.ToString(value, cultureInfo).ToLowerInvariant();
                }
                else if (value is byte[])
                {
                    return System.Convert.ToBase64String((byte[])value);
                }
                else if (value != null && value.GetType().IsArray)
                {
                    var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                    return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
                }

                return System.Convert.ToString(value, cultureInfo);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.2.3.0 (NJsonSchema v10.1.5.0 (Newtonsoft.Json v12.0.0.0))")]
        public partial class ApiUdalostClient
        {
            private string _baseUrl = "http://localhost:8080";
            private System.Net.Http.HttpClient _httpClient;
            private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

            public ApiUdalostClient(System.Net.Http.HttpClient httpClient)
            {
                _httpClient = httpClient;
                _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
            }

            private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            }

            public string BaseUrl
            {
                get { return _baseUrl; }
                set { _baseUrl = value; }
            }

            protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

            partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);
            partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
            partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
            partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task<string> GetListAsync()
            {
                return GetListAsync(System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task<string> GetListAsync(System.Threading.CancellationToken cancellationToken)
            {
                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiUdalost/GetList");

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        request_.Method = new System.Net.Http.HttpMethod("GET");
                        request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                var objectResponse_ = await ReadObjectResponseAsync<string>(response_, headers_).ConfigureAwait(false);
                                return objectResponse_.Object;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }

                            return default(string);
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task AddAsync(UdalostModel msg)
            {
                return AddAsync(msg, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task AddAsync(UdalostModel msg, System.Threading.CancellationToken cancellationToken)
            {
                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiUdalost/Add");

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(msg, _settings.Value));
                        content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                        request_.Content = content_;
                        request_.Method = new System.Net.Http.HttpMethod("POST");

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                return;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task DeleteAsync(int id)
            {
                return DeleteAsync(id, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task DeleteAsync(int id, System.Threading.CancellationToken cancellationToken)
            {
                if (id == null)
                    throw new System.ArgumentNullException("id");

                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiUdalost/Remove/{id}");
                urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        request_.Method = new System.Net.Http.HttpMethod("DELETE");

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                return;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            /// <exception cref="ApiException">A server side error occurred.</exception>
            public System.Threading.Tasks.Task UpdateAsync(UdalostModel msg)
            {
                return UpdateAsync(msg, System.Threading.CancellationToken.None);
            }

            /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
            /// <exception cref="ApiException">A server side error occurred.</exception>
            public async System.Threading.Tasks.Task UpdateAsync(UdalostModel msg, System.Threading.CancellationToken cancellationToken)
            {
                var urlBuilder_ = new System.Text.StringBuilder();
                urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/ApiUdalost/Update");

                var client_ = _httpClient;
                try
                {
                    using (var request_ = new System.Net.Http.HttpRequestMessage())
                    {
                        var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(msg, _settings.Value));
                        content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                        request_.Content = content_;
                        request_.Method = new System.Net.Http.HttpMethod("POST");

                        PrepareRequest(client_, request_, urlBuilder_);
                        var url_ = urlBuilder_.ToString();
                        request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                        PrepareRequest(client_, request_, url_);

                        var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                        try
                        {
                            var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                            if (response_.Content != null && response_.Content.Headers != null)
                            {
                                foreach (var item_ in response_.Content.Headers)
                                    headers_[item_.Key] = item_.Value;
                            }

                            ProcessResponse(client_, response_);

                            var status_ = ((int)response_.StatusCode).ToString();
                            if (status_ == "200")
                            {
                                return;
                            }
                            else
                            if (status_ != "200" && status_ != "204")
                            {
                                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                            }
                        }
                        finally
                        {
                            if (response_ != null)
                                response_.Dispose();
                        }
                    }
                }
                finally
                {
                }
            }

            protected struct ObjectResponseResult<T>
            {
                public ObjectResponseResult(T responseObject, string responseText)
                {
                    this.Object = responseObject;
                    this.Text = responseText;
                }

                public T Object { get; }

                public string Text { get; }
            }

            public bool ReadResponseAsString { get; set; }

            protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers)
            {
                if (response == null || response.Content == null)
                {
                    return new ObjectResponseResult<T>(default(T), string.Empty);
                }

                if (ReadResponseAsString)
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    try
                    {
                        var typedBody = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                        return new ObjectResponseResult<T>(typedBody, responseText);
                    }
                    catch (Newtonsoft.Json.JsonException exception)
                    {
                        var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                        throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                    }
                }
                else
                {
                    try
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        using (var streamReader = new System.IO.StreamReader(responseStream))
                        using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                        {
                            var serializer = Newtonsoft.Json.JsonSerializer.Create(JsonSerializerSettings);
                            var typedBody = serializer.Deserialize<T>(jsonTextReader);
                            return new ObjectResponseResult<T>(typedBody, string.Empty);
                        }
                    }
                    catch (Newtonsoft.Json.JsonException exception)
                    {
                        var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                        throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                    }
                }
            }

            private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
            {
                if (value is System.Enum)
                {
                    string name = System.Enum.GetName(value.GetType(), value);
                    if (name != null)
                    {
                        var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                        if (field != null)
                        {
                            var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                                as System.Runtime.Serialization.EnumMemberAttribute;
                            if (attribute != null)
                            {
                                return attribute.Value != null ? attribute.Value : name;
                            }
                        }
                    }
                }
                else if (value is bool)
                {
                    return System.Convert.ToString(value, cultureInfo).ToLowerInvariant();
                }
                else if (value is byte[])
                {
                    return System.Convert.ToBase64String((byte[])value);
                }
                else if (value != null && value.GetType().IsArray)
                {
                    var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                    return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
                }

                return System.Convert.ToString(value, cultureInfo);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v12.0.0.0)")]
        public partial class DochazkaModel
        {
            [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
            public int Id { get; set; }

            [Newtonsoft.Json.JsonProperty("datum", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.DateTimeOffset Datum { get; set; }

            [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
            public int UzivatelId { get; set; }

            [Newtonsoft.Json.JsonProperty("uzivatelCeleJmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string UzivatelCeleJmeno { get; set; }

            [Newtonsoft.Json.JsonProperty("prichod", Required = Newtonsoft.Json.Required.Always)]
            public bool Prichod { get; set; }

            [Newtonsoft.Json.JsonProperty("cteckaId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string CteckaId { get; set; }


        }

        [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v12.0.0.0)")]
        public partial class UdalostModel
        {
            [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
            public int Id { get; set; }

            [Newtonsoft.Json.JsonProperty("udalostTypId", Required = Newtonsoft.Json.Required.Always)]
            public int UdalostTypId { get; set; }

            [Newtonsoft.Json.JsonProperty("popis", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Popis { get; set; }

            [Newtonsoft.Json.JsonProperty("datumOd", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.DateTimeOffset DatumOd { get; set; }

            [Newtonsoft.Json.JsonProperty("datumDo", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.DateTimeOffset DatumDo { get; set; }

            [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
            public int UzivatelId { get; set; }

            [Newtonsoft.Json.JsonProperty("uzivatelCeleJmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string UzivatelCeleJmeno { get; set; }


        }

        [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.2.3.0 (NJsonSchema v10.1.5.0 (Newtonsoft.Json v12.0.0.0))")]
        public partial class FileResponse : System.IDisposable
        {
            private System.IDisposable _client;
            private System.IDisposable _response;

            public int StatusCode { get; private set; }

            public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

            public System.IO.Stream Stream { get; private set; }

            public bool IsPartial
            {
                get { return StatusCode == 206; }
            }

            public FileResponse(int statusCode, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.IO.Stream stream, System.IDisposable client, System.IDisposable response)
            {
                StatusCode = statusCode;
                Headers = headers;
                Stream = stream;
                _client = client;
                _response = response;
            }

            public void Dispose()
            {
                if (Stream != null)
                    Stream.Dispose();
                if (_response != null)
                    _response.Dispose();
                if (_client != null)
                    _client.Dispose();
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.2.3.0 (NJsonSchema v10.1.5.0 (Newtonsoft.Json v12.0.0.0))")]
        public partial class ApiException : System.Exception
        {
            public int StatusCode { get; private set; }

            public string Response { get; private set; }

            public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

            public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
                : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
            {
                StatusCode = statusCode;
                Response = response;
                Headers = headers;
            }

            public override string ToString()
            {
                return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.2.3.0 (NJsonSchema v10.1.5.0 (Newtonsoft.Json v12.0.0.0))")]
        public partial class ApiException<TResult> : ApiException
        {
            public TResult Result { get; private set; }

            public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
                : base(message, statusCode, response, headers, innerException)
            {
                Result = result;
            }
        }

    }

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore 472
#pragma warning restore 114
#pragma warning restore 108

