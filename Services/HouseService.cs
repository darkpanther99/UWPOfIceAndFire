using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?page={page}&pageSize=30"));
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?name={name}&pageSize=30"));
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromRegion(string region, int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?region={region}&page={page}&pageSize=30"));
        }
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromWords(string words, int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?words={words}&page={page}&pageSize=30"));
        }

        public async Task<House> GetHouseAsyncFromFullUrl(Uri uri, int depth)
        {
            if (depth > 1)
            {
                return null;
            }

            CharacterService characterService = CharacterService.Instance;
            var househelper = await GetAsync<HouseHelper>(uri);

            var house = new House()
            {
                url = househelper.url,
                name = househelper.name,
                region = househelper.region,
                coatOfArms = househelper.coatOfArms,
                words = househelper.words,
                titles = new ObservableCollection<string>(),
                seats = new ObservableCollection<string>(),
                currentLord = househelper.currentLord != "" ? await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.currentLord), depth + 1) : null,
                heir = househelper.heir != "" ? await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.heir), depth + 1) : null,
                overlord = househelper.overlord != "" ? await GetHouseAsyncFromFullUrl(new Uri(househelper.overlord), depth + 1) : null,
                founded = househelper.founded,
                founder = househelper.founder != "" ? await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.founder), depth + 1) : null,
                diedOut = househelper.diedOut,
                ancestralWeapons = new ObservableCollection<string>(),
                cadetBranches = new ObservableCollection<House>(),
                swornMembers = new ObservableCollection<Character>()
            };

            foreach (var title in househelper.titles)
            {
                house.titles.Add(title);
            }
            foreach (var seat in househelper.seats)
            {
                house.seats.Add(seat);
            }
            foreach (var wpn in househelper.ancestralWeapons)
            {
                house.ancestralWeapons.Add(wpn);
            }
            foreach (var cadetbranch in househelper.cadetBranches)
            {
                House h = await GetHouseAsyncFromFullUrl(new Uri(cadetbranch), depth + 1);
                house.cadetBranches.Add(h);
            }
            foreach (var swornmember in househelper.swornMembers)
            {
                Character c = await characterService.GetCharacterAsyncFromFullUrl(new Uri(swornmember), depth + 1);
                house.swornMembers.Add(c);
            }

            return house;
        }
    }
}
