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
    /// <summary>
    /// Singleton Service, which forwards House queries to the API.
    /// </summary>
    class HouseService : BaseService
    {
        #region Singleton things
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
        #endregion


        /// <summary>
        /// Stores how many houses can be on the same page.
        /// </summary>
        private int pageSize = 30;

        private int start = 0;

        /// <summary>
        /// Stores how many characters can be on the same page(the list of sworn member characters of a house is pageable).
        /// </summary>
        private const int charactersonpage = 10;

        /// <summary>
        /// Returns the House with the correct ID.
        /// </summary>
        public async Task<House> GetHouseAsync(int id)
        {
            return await GetHouseAsyncFromFullUrl(new Uri(baseUrl, $"houses/{id}"), depth: 0);
        }

        /// <summary>
        /// Returns a preview object from houses on the specified page.
        /// </summary>
        public async Task<List<HouseHelper>> GetHousesPreviewAsync(int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from houses with the specified name.
        /// </summary>
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?name={name}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from houses from the specified region, from the specified page.
        /// </summary>
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromRegion(string region, int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?region={region}&page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from houses which has the specified words, from the specified page.
        /// </summary>
        public async Task<List<HouseHelper>> GetHousesPreviewAsyncFromWords(string words, int page)
        {
            return await GetAsync<List<HouseHelper>>(new Uri(baseUrl, $"houses?words={words}&page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a house object from the specified URI.
        /// This is a recursive function, because the JSON responses from the API are pointing to each other by URI-s.
        /// Gets all details of a house from the API, and for all the nested URI-s, it calls the right service.
        /// When it has constructed the full house object, returns it.
        /// </summary>
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
            start = 0;
            int end = start + charactersonpage;
            if (end > househelper.swornMembers.Count())
            {
                end = househelper.swornMembers.Count();
            }
            for (int i = start; i < end; ++i)
            {
                Character c = await characterService.GetCharacterAsyncFromFullUrl(new Uri(househelper.swornMembers[i]), depth + 1);
                house.swornMembers.Add(c);
            }

            return house;
        }

        /// <summary>
        /// Returns a List of the next charactersonpage number of characters.
        /// </summary>
        public async Task<List<Character>> NextPage(string searchstr)
        {
            start += charactersonpage;
            return await GetCharactersFromInterval(new Uri(searchstr));
        }

        /// <summary>
        /// Returns a List of the previous charactersonpage number of characters.
        /// </summary>
        public async Task<List<Character>> PreviousPage(string searchstr)
        {
            start -= charactersonpage;
            if (start < 0)
            {
                start = 0;
            }
            return await GetCharactersFromInterval(new Uri(searchstr));
        }

        /// <summary>
        /// Gets a househelper object from the API, and returns a List of the househelper's swornMembers in the specified interval(charactersonpage number of characters, starting from the start field's value)
        /// </summary>
        private async Task<List<Character>> GetCharactersFromInterval(Uri uri)
        {
            var househelper = await GetAsync<HouseHelper>(uri);
            var res = new List<Character>();
            int end = start + charactersonpage;
            if (end > househelper.swornMembers.Count())
            {
                end = househelper.swornMembers.Count();
            }
            for (int i = start; i < end; ++i)
            {
                Character c = await CharacterService.Instance.GetCharacterAsyncFromFullUrl(new Uri(househelper.swornMembers[i]), depth: 1);
                res.Add(c);
            }
            return res;
        }
    }
}
