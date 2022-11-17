
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RestClient
{
	public class RestClient
	{
		#region Singelton
		private static IHttpContextAccessor httpContextAccessor;
		private static HttpClientHandler httpClientHandler;
		private static RestClient instance = null;
		private static object locker = new object();
		public static RestClient GetInstance(IHttpContextAccessor context)
		{
			if (instance == null)
			{
				lock (locker)
				{
					instance = new RestClient();
					httpClientHandler = new HttpClientHandler();
					httpContextAccessor = context;
					//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
					httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
				}
			}
			return instance;
		}
		#endregion RestClient

		#region GetAsync
		public async Task<string> GetAsync(string requestUri)
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					using (var response = await httpClient.GetAsync(requestUri))
					{
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> GetAsync(string requestUri, bool setAuthorization)
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					if (setAuthorization)
					{
						SetAcceptHeader(httpClient);
						SetAuthorization(httpClient);
					}
					
					using (var response = await httpClient.GetAsync(requestUri))
					{
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> GetAsync(string requestUri, bool setAuthorization, Dictionary<string, string> additionalHeaders)
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					SetAcceptHeader(httpClient);
					if (setAuthorization)
					{
						SetAuthorization(httpClient);
					}
					AddHeaders(httpClient, additionalHeaders);

					using (var response = await httpClient.GetAsync(requestUri))
					{
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		#endregion

		#region PostAsync
		public async Task<string> PostAsync<T>(string requestUri, T request) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					using (var response = await httpClient.PostAsync(requestUri, content))
					{
						result = await response.Content.ReadAsStringAsync();
						//if (response.StatusCode != HttpStatusCode.OK) throw new Exception(result);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PostAsync<T>(string requestUri, T request, bool setAuthorization) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					if (setAuthorization)
					{
						SetAcceptHeader(httpClient);
						SetAuthorization(httpClient);
					}
					using (var response = await httpClient.PostAsync(requestUri, content))
					{
						result = await response.Content.ReadAsStringAsync();
						//if (response.StatusCode != HttpStatusCode.OK) throw new Exception(result);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PostAsync<T>(string requestUri, T request, bool setAuthorization, Dictionary<string, string> additionalHeaders) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					SetAcceptHeader(httpClient);
					if (setAuthorization)
					{
						SetAuthorization(httpClient);
					}
					AddHeaders(httpClient, additionalHeaders);
					using (var response = await httpClient.PostAsync(requestUri, content))
					{
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK) throw new Exception(result);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}

		public async Task<string> PostAsync(string requestUri, List<KeyValuePair<string, string>> request)
		{
			string result = "";
			try
			{
				using (httpClientHandler = new HttpClientHandler())
				{
					using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
					{
						var content = new FormUrlEncodedContent(request);
						using (var response = await httpClient.PostAsync(requestUri, content))
						{
							result = await response.Content.ReadAsStringAsync();
							if (response.StatusCode != HttpStatusCode.OK) throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PostAsync(string requestUri, List<KeyValuePair<string, string>> request, bool setAuthorization)
		{
			string result = "";
			try
			{
				using (httpClientHandler = new HttpClientHandler())
				{
					using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
					{
						var content = new FormUrlEncodedContent(request);
						if (setAuthorization)
						{
							SetAcceptHeader(httpClient);
							SetAuthorization(httpClient);
						}
						using (var response = await httpClient.PostAsync(requestUri, content))
						{
							result = await response.Content.ReadAsStringAsync();
							if (response.StatusCode != HttpStatusCode.OK) throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PostAsync(string requestUri, List<KeyValuePair<string, string>> request, bool setAuthorization, Dictionary<string, string> additionalHeaders)
		{
			string result = "";
			try
			{
				using (httpClientHandler = new HttpClientHandler())
				{
					using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
					{
						var content = new FormUrlEncodedContent(request);
						SetAcceptHeader(httpClient);
						if (setAuthorization)
						{
							SetAuthorization(httpClient);
						}
						AddHeaders(httpClient, additionalHeaders);
						using (var response = await httpClient.PostAsync(requestUri, content))
						{
							result = await response.Content.ReadAsStringAsync();
							if (response.StatusCode != HttpStatusCode.OK) throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		#endregion

		#region PutAsync
		public async Task<string> PutAsync<T>(string requestUri, T request) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

					using (var response = await httpClient.PutAsync(requestUri, content))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PutAsync<T>(string requestUri, T request, bool setAuthorization) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					if (setAuthorization)
					{
						SetAcceptHeader(httpClient);
						SetAuthorization(httpClient);
					}
					using (var response = await httpClient.PutAsync(requestUri, content))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PutAsync<T>(string requestUri, T request, bool setAuthorization, Dictionary<string, string> additionalHeaders) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					SetAcceptHeader(httpClient);
					if (setAuthorization)
					{
						SetAuthorization(httpClient);
					}
					AddHeaders(httpClient, additionalHeaders);
					using (var response = await httpClient.PutAsync(requestUri, content))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		#endregion

		#region DeleteAsync
		public async Task<string> DeleteAsync(string requestUri)
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					using (var response = await httpClient.DeleteAsync(requestUri))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> DeleteAsync(string requestUri, bool setAuthorization)
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					if (setAuthorization)
					{
						SetAcceptHeader(httpClient);
						SetAuthorization(httpClient);
					}
					using (var response = await httpClient.DeleteAsync(requestUri))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> DeleteAsync(string requestUri, bool setAuthorization, Dictionary<string, string> additionalHeaders)
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					SetAcceptHeader(httpClient);
					if (setAuthorization)
					{
						SetAuthorization(httpClient);
					}
					AddHeaders(httpClient, additionalHeaders);
					using (var response = await httpClient.DeleteAsync(requestUri))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		#endregion

		#region PatchAsync
		public async Task<string> PatchAsync<T>(string requestUri, T request) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					using (var response = await httpClient.PatchAsync(requestUri, content))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PatchAsync<T>(string requestUri, T request, bool setAuthorization) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					if (setAuthorization)
					{
						SetAcceptHeader(httpClient);
						SetAuthorization(httpClient);
					}
					using (var response = await httpClient.PatchAsync(requestUri, content))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PatchAsync<T>(string requestUri, T request,bool setAuthorization, Dictionary<string, string> additionalHeaders) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					SetAcceptHeader(httpClient);
					if (setAuthorization)
					{
						SetAuthorization(httpClient);
					}
					AddHeaders(httpClient, additionalHeaders);
					using (var response = await httpClient.PatchAsync(requestUri, content))
					{
						//response.EnsureSuccessStatusCode();
						result = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new Exception(result);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		#endregion

		#region PostStreamAsync
		public async Task<string> PostStreamAsync<T>(string requestUri, T request) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					using (var response = await httpClient.PostAsync(requestUri, content))
					{
						using (var ms = (MemoryStream)await response.Content.ReadAsStreamAsync())
						{
							var sw = new StreamWriter(ms);
							var sr = new StreamReader(ms);
							ms.Position = 0;

							result = sr.ReadToEnd();
						}

						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new IntegrationException("Exception rest client");
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PostStreamAsync<T>(string requestUri, T request, bool setAuthorization) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					if (setAuthorization)
					{
						SetAcceptHeader(httpClient);
						SetAuthorization(httpClient);
					}
					using (var response = await httpClient.PostAsync(requestUri, content))
					{
						using (var ms = (MemoryStream)await response.Content.ReadAsStreamAsync())
						{
							var sw = new StreamWriter(ms);
							var sr = new StreamReader(ms);
							ms.Position = 0;

							result = sr.ReadToEnd();
						}

						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new IntegrationException("Exception rest client");
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		public async Task<string> PostStreamAsync<T>(string requestUri, T request, bool setAuthorization, Dictionary<string, string> additionalHeaders) where T : class
		{
			string result = "";
			try
			{
				using (HttpClient httpClient = new HttpClient(httpClientHandler, false))
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					SetAcceptHeader(httpClient);
					if (setAuthorization)
					{
						SetAuthorization(httpClient);
					}
					AddHeaders(httpClient, additionalHeaders);
					using (var response = await httpClient.PostAsync(requestUri, content))
					{
						using (var ms = (MemoryStream)await response.Content.ReadAsStreamAsync())
						{
							var sw = new StreamWriter(ms);
							var sr = new StreamReader(ms);
							ms.Position = 0;

							result = sr.ReadToEnd();
						}

						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new IntegrationException("Exception rest client");
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}
		#endregion

		#region Header
		private void SetAcceptHeader(HttpClient httpClient)
		{
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
		}
		private void SetAuthorization(HttpClient httpClient)
		{
			var bearerToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(bearerToken) == false)
			{
				httpClient.DefaultRequestHeaders.Add("Authorization", bearerToken.ToString());
			}
		}
		private void AddHeaders(HttpClient httpClient, Dictionary<string, string> additionalHeaders)
		{
			if (additionalHeaders == null)
				return;

			foreach (KeyValuePair<string, string> current in additionalHeaders)
			{
				httpClient.DefaultRequestHeaders.Add(current.Key, current.Value);
			}
		}
		#endregion
	}
}
