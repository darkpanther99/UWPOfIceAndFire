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
        public House house { get; set; } = new House() {
            titles = new ObservableCollection<string>(),
            seats = new ObservableCollection<string>(),
            ancestralWeapons = new ObservableCollection<string>(),
            cadetBranches = new ObservableCollection<House>(),
            swornMembers = new ObservableCollection<Character>()
        };
        public Book book { get; set; } = new Book() { 
            authors = new ObservableCollection<string>(),
            characters = new ObservableCollection<Character>(),
            povCharacters = new ObservableCollection<Character>()
        };

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
                BuildBook(bookhelper);
            }
            else
            {
                throw new Exception("Se nem karakter, se nem ház, se nem könyv? Akkor mi?");
            }
        }

        public async Task<string> GetURIStringFromName(string name)
        {
            var urls = new List<string>();
            var books = await BookService.Instance.GetBooksPreviewAsyncFromName(name);
            var characters = await CharacterService.Instance.GetCharactersPreviewAsyncFromName(name);
            var houses = await HouseService.Instance.GetHousesPreviewAsyncFromName(name);

            foreach (var b in books)
            {
                urls.Add(b.url);
            }
            foreach (var c in characters)
            {
                urls.Add(c.url);
            }
            foreach (var h in houses)
            {
                urls.Add(h.url);
            }

            foreach (var u in urls)
            {
                return u;
            }

            return "";
        }


        private void BuildBook(Book b)
        {
            book.url = b.url;
            book.name = b.name;
            book.isbn = b.isbn;
            RepopulateObservableCollection(b.authors, book.authors);
            book.numberOfPages = b.numberOfPages;
            book.publisher = b.publisher;
            book.country = b.country;
            book.mediaType = b.mediaType;
            book.released = b.released;
            RepopulateObservableCollection(b.characters, book.characters);
            RepopulateObservableCollection(b.povCharacters, book.povCharacters);
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
            RepopulateObservableCollection(hous.titles, house.titles);
            RepopulateObservableCollection(hous.seats, house.seats);
            RepopulateObservableCollection(hous.ancestralWeapons, house.ancestralWeapons);
            RepopulateObservableCollection(hous.cadetBranches, house.cadetBranches);
            RepopulateObservableCollection(hous.swornMembers, house.swornMembers);
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
