using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services.HelperModels;

namespace TXC54G_HF.Services
{
    class HouseService : BaseService
    {
        private HouseService() { }
        private static HouseService instance = null;
        public static HouseService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HouseService();
                }
                return instance;
            }
        }
        
        public async Task<House> GetHouseAsync(int id)
        {
            return await GetHouseAsyncFromFullUrl(new Uri(baseUrl, $"houses/{id}"), depth: 0);
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsync(int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?page={page}"));
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?name={name}"));
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromRegion(string region, int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?region={region}&page={page}"));
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromWords(string words, int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?words={words}&page={page}"));
        }

        public async Task<House> GetHouseAsyncFromFullUrl(Uri uri, int depth)
        {
            if (depth > 1)
            {
                return null;
            }
            CharacterService characterService = CharacterService.Instance;
            var househelper = await GetAsync<HouseHelper>(uri);
            var cadetBranches = new List<House>();
            var swornMembers = new List<Character>();
            foreach (var cadetbranch in househelper.cadetBranches)
            {
                House h = await GetHouseAsyncFromFullUrl(new Uri(cadetbranch), depth + 1);
                cadetBranches.Add(h);
            }
            foreach (var swornmember in househelper.swornMembers)
            {
                Character c = await characterService.GetCharacterAsyncFromFullUrl(new Uri(swornmember), depth + 1);
                swornMembers.Add(c);
            }

            return new House()
            {
                url = househelper.url,
                name = househelper.name,
                region = househelper.region,
                coatOfArms = househelper.coatOfArms,
                words = househelper.words,
                titles = househelper.titles,
                seats = househelper.seats,
                currentLord = househelper.currentLord != "" ? await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.currentLord), depth + 1) : null,
                heir = househelper.heir != "" ? await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.heir), depth + 1) : null,
                overlord = househelper.overlord != "" ? await GetHouseAsyncFromFullUrl(new Uri(househelper.overlord), depth + 1) : null,
                founded = househelper.founded,
                founder = househelper.founder != "" ? await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.founder), depth + 1) : null,
                diedOut = househelper.diedOut,
                ancestralWeapons = househelper.ancestralWeapons,
                cadetBranches = cadetBranches,
                swornMembers = swornMembers
            };
        }
    }
}
