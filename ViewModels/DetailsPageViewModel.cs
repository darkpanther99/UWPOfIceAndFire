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
        public Character character { get; set; } = new Character() {
            allegiances = new ObservableCollection<House>(), 
            books = new ObservableCollection<Book>(),
            povBooks = new ObservableCollection<Book>(),
            titles = new ObservableCollection<string>(),
            aliases = new ObservableCollection<string>(),
            tvSeries = new ObservableCollection<string>(),
            playedBy = new ObservableCollection<string>(),
        };
        public House house { get; set; } = new House() { name = "haz" };
        public Book book { get; set; } = new Book() { name = "konyv" };

        public async void ShowDetails(string searchstr)
        {
            if (searchstr.Contains("characters"))
            {
                var charact = await GetCharacter(searchstr);
                BuildCharacter(charact);
                
            }
            else if (searchstr.Contains("houses"))
            {
                var hous = await GetHouse(searchstr);
                BuildHouse(hous);
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

        private void BuildHouse(House hous)
        {
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

        private void BuildCharacter(Character charact)
        {
            character.url = charact.url;
            character.name = charact.name;
            character.gender = charact.gender;
            character.culture = charact.culture;
            character.born = charact.born;
            character.died = charact.died;
            RepopulateObservableCollection(charact.titles, character.titles);
            RepopulateObservableCollection(charact.aliases, character.aliases);
            character.father = charact.father;
            character.mother = charact.mother;
            character.spouse = charact.spouse;
            RepopulateObservableCollection(charact.allegiances, character.allegiances);
            RepopulateObservableCollection(charact.books, character.books);
            RepopulateObservableCollection(charact.povBooks, character.povBooks);
            RepopulateObservableCollection(charact.tvSeries, character.tvSeries);
            RepopulateObservableCollection(charact.playedBy, character.playedBy);
        }

        private void RepopulateObservableCollection<T>(ObservableCollection<T> source, ObservableCollection<T> destination)
        {
            destination.Clear();
            foreach (T element in source)
            {
                destination.Add(element);
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
