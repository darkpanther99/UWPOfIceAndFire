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
    class CharacterService : BaseService
    {
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
        
        public async Task<Character> GetCharacterAsync(int id)
        {
            return await GetCharacterAsyncFromFullUrl(new Uri(baseUrl, $"characters/{id}"), depth: 0);
        }
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsync(int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?page={page}&pageSize=30"));
        }

        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?name={name}&pageSize=30"));
        }

        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromGender(string gender, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?gender={gender}&page={page}&pageSize=30"));
        }

        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromCulture(string culture, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?culture={culture}&page={page}&pageSize=30"));
        }
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromBirth(string birth, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?born={birth}&page={page}&pageSize=30"));
        }
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromDeath(string death, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?died={death}&page={page}&pageSize=30"));
        }
        public async Task<List<CharacterHelper>> GetCharactersPreviewAsyncFromIsAlive(string isalive, int page)
        {
            return await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?isAlive={isalive}&page={page}&pageSize=30"));
        }

        public async Task<List<Character>> GetCharactersAsyncFromName(string name)
        {
            var results = await GetAsync<List<CharacterHelper>>(new Uri(baseUrl, $"characters?name={name}"));
            var resultslist = new List<Character>();
            
            foreach(var res in results)
            {
                resultslist.Add(await GetCharacterAsyncFromFullUrl(new Uri(res.url), depth:0));
            }

            return resultslist;
        }

        

        public async Task<Character> GetCharacterAsyncFromFullUrl(Uri uri, int depth)
        {
            if (depth > 1)
            {
                return null;
            }
            HouseService houseService = HouseService.Instance;
            BookService bookService = BookService.Instance;
            var characterhelper = await GetAsync<CharacterHelper>(uri);
            var allegiances = new List<House>();
            var books = new List<Book>();
            var povBooks = new List<Book>();
            foreach (var all in characterhelper.allegiances)
            {
                House h = await houseService.GetHouseAsyncFromFullUrl(new Uri(all), depth + 1);
                allegiances.Add(h);
            }
            foreach (var book in characterhelper.books)
            {
                Book b = await bookService.GetBookAsyncFromFullUrl(new Uri(book), depth + 1);
                books.Add(b);
            }
            foreach (var povbook in characterhelper.povBooks)
            {
                Book pb = await bookService.GetBookAsyncFromFullUrl(new Uri(povbook), depth + 1);
                books.Add(pb);
            }
            Debug.WriteLine(characterhelper.father);
            Debug.WriteLine("asd");
            

            return new Character()
            {
                url = characterhelper.url,
                name = characterhelper.name,
                gender = characterhelper.gender,
                culture = characterhelper.culture,
                born = characterhelper.born,
                died = characterhelper.died,
                titles = characterhelper.titles,
                aliases = characterhelper.aliases,
                father = characterhelper.father != "" ? await GetCharacterAsyncFromFullUrl(new Uri(characterhelper.father), depth + 1) : null,
                mother = characterhelper.mother != "" ?  await GetCharacterAsyncFromFullUrl(new Uri(characterhelper.mother), depth + 1) : null,
                spouse = characterhelper.spouse != "" ?  await GetCharacterAsyncFromFullUrl(new Uri(characterhelper.spouse), depth + 1) : null,
                allegiances = allegiances,
                books = books,
                povBooks = povBooks,
                tvSeries = characterhelper.tvSeries,
                playedBy = characterhelper.playedBy
            };
        }
    }
}
