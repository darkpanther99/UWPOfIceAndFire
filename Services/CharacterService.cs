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
    /// Singleton Service, which forwards Character queries to the API.
    /// </summary>
    class CharacterService : BaseService
    {
        #region Singleton things
        private CharacterService() { }

        private static CharacterService instance = null;
        public static CharacterService Instance {
            get {
                if (instance == null){
                    instance = new CharacterService();
                }
                return instance;
            } 
        }
        #endregion

        /// <summary>
        /// Stores how many characters can be on the same page.
        /// </summary>
        private int pageSize = 30;

        /// <summary>
        /// Returns the character with the correct ID.
        /// </summary>
        public async Task<Character> GetCharacterAsync(int id)
        {
            return await GetCharacterAsyncFromFullUrl(new Uri(baseUrl, $"characters/{id}"), depth: 0);
        }

        /// <summary>
        /// Returns a preview object from characters on the specified page.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsync(int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from characters with the specified name.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?name={name}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from characters with the specified gender, from the specified page.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromGender(string gender, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?gender={gender}&page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from characters with the specified culture, from the specified page.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromCulture(string culture, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?culture={culture}&page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from characters with the specified birth date, from the specified page.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromBirth(string birth, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?born={birth}&page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from characters with the specified birth date, from the specified page.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromDeath(string death, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?died={death}&page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from characters with the specified state of being alive or not, from the specified page.
        /// </summary>
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromIsAlive(string isalive, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?isAlive={isalive}&page={page}&pageSize={pageSize}"));
        }

        /*public async Task<List<Character>> GetCharactersAsyncFromName(string name)
        {
            var results = await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?name={name}"));
            var resultslist = new List<Character>();
            
            foreach(var res in results)
            {
                resultslist.Add(await GetCharacterAsyncFromFullUrl(new Uri(res.url), depth:0));
            }

            return resultslist;
        }*/


        /// <summary>
        /// Returns a character object from the specified URI.
        /// This is a recursive function, because the JSON responses from the API are pointing to each other by URI-s.
        /// Gets all details of a character from the API, and for all the nested URI-s, it calls the right service.
        /// When it has constructed the full character object, returns it.
        /// </summary>
        public async Task<Character> GetCharacterAsyncFromFullUrl(Uri uri, int depth)
        {
            if (depth > 1)
            {
                return null;
            }

            HouseService houseService = HouseService.Instance;
            BookService bookService = BookService.Instance;

            var characterhelper = await GetAsync<CharacterHelper>(uri);

            var character = new Character()
            {
                url = characterhelper.url,
                name = characterhelper.name,
                gender = characterhelper.gender,
                culture = characterhelper.culture,
                born = characterhelper.born,
                died = characterhelper.died,
                titles = new ObservableCollection<string>(),
                aliases = new ObservableCollection<string>(),
                father = characterhelper.father != "" ? await GetCharacterAsyncFromFullUrl(new Uri(characterhelper.father), depth + 1) : null,
                mother = characterhelper.mother != "" ? await GetCharacterAsyncFromFullUrl(new Uri(characterhelper.mother), depth + 1) : null,
                spouse = characterhelper.spouse != "" ? await GetCharacterAsyncFromFullUrl(new Uri(characterhelper.spouse), depth + 1) : null,
                allegiances = new ObservableCollection<House>(),
                books = new ObservableCollection<Book>(),
                povBooks = new ObservableCollection<Book>(),
                tvSeries = new ObservableCollection<string>(),
                playedBy = new ObservableCollection<string>()
            };

            foreach (var all in characterhelper.allegiances)
            {
                House h = await houseService.GetHouseAsyncFromFullUrl(new Uri(all), depth + 1);
                character.allegiances.Add(h);
            }
            foreach (var book in characterhelper.books)
            {
                Book b = await bookService.GetBookAsyncFromFullUrl(new Uri(book), depth + 1);
                character.books.Add(b);
            }
            foreach (var povbook in characterhelper.povBooks)
            {
                Book pb = await bookService.GetBookAsyncFromFullUrl(new Uri(povbook), depth + 1);
                character.books.Add(pb);
            }
            foreach (var title in characterhelper.titles)
            {
                character.titles.Add(title);
            }
            foreach (var alias in characterhelper.aliases)
            {
                character.aliases.Add(alias);
            }
            foreach (var tv in characterhelper.tvSeries)
            {
                character.tvSeries.Add(tv);
            }
            foreach (var actor in characterhelper.playedBy)
            {
                character.playedBy.Add(actor);
            }


            return character;
            
        }
    }
}
