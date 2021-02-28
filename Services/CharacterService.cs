using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;

namespace TXC54G_HF.Services
{
    class CharacterService : BaseService
    {
        public async Task<Character> GetCharacterAsync(int id)
        {
            return await GetAsync<Character>(new Uri(baseUrl, $"characters/{id}"));
        }

        public async Task<Character> GetCharacterAsyncFromFullUrl(string uri)
        {
            return await GetAsync<Character>(new Uri(uri));
        }
    }
}
