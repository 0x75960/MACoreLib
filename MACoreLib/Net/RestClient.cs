using MACoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MACoreLib.Net
{
    /// <summary>
    /// exception thrown when failed to request
    /// </summary>
    public class RequestFailedException : MACoreException { }

    /// <summary>
    /// exception thrown when failed to decode response
    /// </summary>
    public class DecodeFailedException : MACoreException { }

    /// <summary>
    /// simple REST API Client ***Untested***
    /// </summary>
    public class RestClient
    {

        private HttpClient cli;

        /// <summary>
        /// RestClient for API Server
        /// </summary>
        /// <param name="api_root">baseaddress of service</param>
        public RestClient(string api_root)
        {
            this.cli = new HttpClient();
            this.cli.BaseAddress = new Uri(api_root);
            this.cli.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Get request to api
        /// </summary>
        /// <typeparam name="T">Type of response. must be seriaizable</typeparam>
        /// <param name="api">api to request</param>
        /// <exception cref="MACoreLib.Net.RequestFailedException"></exception>
        /// <exception cref="MACoreLib.Net.DecodeFailedException"></exception>
        /// <returns>Object of response</returns>
        public async Task<T> Get<T>(string api) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            Task<System.IO.Stream> result;

            try
            {
                result = this.cli.GetStreamAsync(api);
            }
            catch
            {
                throw new RequestFailedException();
            }

            try
            {
                return serializer.ReadObject(await result) as T;
            }
            catch
            {
                throw new DecodeFailedException();
            }

        }

        /// <summary>
        /// Get request to api with query parameters
        /// </summary>
        /// <typeparam name="T">Type of response. must be serializable</typeparam>
        /// <param name="api">api to request</param>
        /// <param name="param">query parameters</param>
        /// <exception cref="MACoreLib.Net.RequestFailedException"></exception>
        /// <exception cref="MACoreLib.Net.DecodeFailedException"></exception>
        /// <returns>Object of response</returns>
        public async Task<T> Get<T>(string api, Dictionary<string, string> param) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            var p = (from KeyValuePair<string, string> pair in param select pair.Key + "=" + pair.Value).ToArray<string>();
            var uri =  api + ("?" + string.Join("&", p));


            Task<System.IO.Stream> result;

            try
            {
                result = this.cli.GetStreamAsync(uri);
            }
            catch
            {
                throw new RequestFailedException();
            }

            try
            {
                return serializer.ReadObject(await result) as T;
            }
            catch
            {
                throw new DecodeFailedException();
            }
        }

        /// <summary>
        /// Post request to api with query parameters
        /// </summary>
        /// <typeparam name="T">Type of response. must be serializable</typeparam>
        /// <param name="api">api to request</param>
        /// <param name="param">query parameters</param>
        /// <exception cref="MACoreLib.Net.RequestFailedException"></exception>
        /// <exception cref="MACoreLib.Net.DecodeFailedException"></exception>
        /// <returns>Object of response</returns>
        public async Task<T> Post<T>(string api, Dictionary<string, string> param) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            var content = new FormUrlEncodedContent(param);

            HttpResponseMessage result;

            try
            {
                result = this.cli.PostAsync(api, content).Result;
            }
            catch
            {
                throw new RequestFailedException();
            }

            try
            {
                return serializer.ReadObject(await result.Content.ReadAsStreamAsync()) as T;
            }
            catch
            {
                throw new DecodeFailedException();
            }

        }

        /// <summary>
        /// Put request to api with query parameters
        /// </summary>
        /// <typeparam name="T">Type of response. must be serializable</typeparam>
        /// <param name="api">api to request</param>
        /// <param name="param">query parameters</param>
        /// <exception cref="MACoreLib.Net.RequestFailedException"></exception>
        /// <exception cref="MACoreLib.Net.DecodeFailedException"></exception>
        /// <returns>Object of response</returns>
        public async Task<T> Put<T>(string api, Dictionary<string, string> param) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            var content = new FormUrlEncodedContent(param);

            HttpResponseMessage result;

            try
            {
                result = this.cli.PutAsync(api, content).Result;
            }
            catch
            {
                throw new RequestFailedException();
            }

            try
            {
                return serializer.ReadObject(await result.Content.ReadAsStreamAsync()) as T;
            }
            catch
            {
                throw new DecodeFailedException();
            }

        }

    }
}