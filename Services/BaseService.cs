using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Services
{
    /// <summary>
    /// Base class of the Services.
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// The Base URL of the API is stored here.
        /// </summary>
        protected readonly Uri baseUrl = new Uri("https://www.anapioficeandfire.com/api/");

        /// <summary>
        /// Gets a specified entity from the API, using the uri parameter, then returns it deserialized into a C# object.
        /// </summary>
        protected async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

    }
}
