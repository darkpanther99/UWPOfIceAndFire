using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;

namespace TXC54G_HF.Services
{
    class HouseService : BaseService
    {
        public async Task<House> GetHouseAsync(int id)
        {
            return await GetAsync<House>(new Uri(baseUrl, $"houses/{id}"));
        }
    }
}
