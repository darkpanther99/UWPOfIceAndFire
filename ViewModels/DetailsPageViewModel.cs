using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services;

namespace TXC54G_HF.ViewModels
{
    class DetailsPageViewModel
    {
        public Character character { get; set; } = new Character();
        public ObservableCollection<string> titles { get; set; }  = new ObservableCollection<string>();
        public ObservableCollection<string> aliases { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<House> allegiances { get; set; } = new ObservableCollection<House>();
        public ObservableCollection<Book> books { get; set; } = new ObservableCollection<Book>();
        public ObservableCollection<Book> povBooks { get; set; } = new ObservableCollection<Book>();
        public ObservableCollection<string> tvSeries { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> playedBy { get; set; } = new ObservableCollection<string>();
        public House house { get; set; } = new House() { name = "haz" };
        public Book book { get; set; } = new Book() { name = "konyv" };

        public async void ShowDetails(string searchstr)
        {
            if (searchstr.Contains("characters"))
            {
                var charact = await GetCharacter(searchstr);
                character.url = charact.url;
                character.name = charact.name;
                character.gender = charact.gender;
                character.culture = charact.culture;
                character.born = charact.born;
                character.died = charact.died;
                //character.titles = charact.titles;
                titles.Clear();
                foreach (var title in charact.titles)
                {
                    titles.Add(title);
                }
                //character.aliases = charact.aliases;
                aliases.Clear();
                foreach (var alias in charact.aliases)
                {
                    aliases.Add(alias);
                }
                character.father = charact.father;
                character.mother = charact.mother;
                character.spouse = charact.spouse;
                foreach (var allegiance in charact.allegiances)
                {
                    allegiances.Add(allegiance);
                }
                foreach (var book in charact.books)
                {
                    books.Add(book);
                }
                foreach (var book in charact.povBooks)
                {
                    povBooks.Add(book);
                }
                foreach (var tv in charact.tvSeries)
                {
                    tvSeries.Add(tv);
                }
                foreach (var actor in charact.playedBy)
                {
                    playedBy.Add(actor);
                }
            }
            else if (searchstr.Contains("houses"))
            {
                var hous = await GetHouse(searchstr);
                house.url = hous.url;
                house.name = hous.name;
                house.region = hous.region;
                house.coatOfArms = hous.coatOfArms;
                house.words = hous.words;
                house.currentLord = hous.currentLord;
                house.heir = hous.heir;
                house.overlord = hous.overlord;
                house.founded = hous.founded;
                house.founder = hous.founder;
                house.diedOut = hous.diedOut;
                // TODO: titles, seats, ancestral weapons, cadetbranches, swornmembers
            }
            else if (searchstr.Contains("books"))
            {
                var bookhelper = await GetBook(searchstr);
                //Debug.WriteLine(book.name);
                book.name = bookhelper.name;
            }
            else
            {
                throw new Exception("Se nem karakter, se nem ház, se nem könyv? Akkor mi?");
            }
        }

        private async Task<Character> GetCharacter(string str)
        {
            return await CharacterService.Instance.GetCharacterAsyncFromFullUrl(new Uri(str), depth:0);
        }

        private async Task<Book> GetBook(string str)
        {
            return await BookService.Instance.GetBookAsyncFromFullUrl(new Uri(str), depth: 0);
        }

        private async Task<House> GetHouse(string str)
        {
            return await HouseService.Instance.GetHouseAsyncFromFullUrl(new Uri(str), depth: 0);
        }
    }
}
