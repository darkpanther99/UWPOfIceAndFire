using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services;
using TXC54G_HF.ViewModels.Utilities;
using Windows.UI.Xaml.Media.Imaging;

namespace TXC54G_HF.ViewModels
{
    /// <summary>
    /// VM of the Details page, it handles the code behind's requests.
    /// </summary>
    class DetailsPageViewModel
    {

        /// <summary>
        /// Databound Character entity, with initialized Observable Collections.
        /// </summary>
        public Character character { get; set; } = new Character() {
            allegiances = new ObservableCollection<House>(), 
            books = new ObservableCollection<Book>(),
            povBooks = new ObservableCollection<Book>(),
            titles = new ObservableCollection<string>(),
            aliases = new ObservableCollection<string>(),
            tvSeries = new ObservableCollection<string>(),
            playedBy = new ObservableCollection<string>(),
        };

        /// <summary>
        /// Databound House entity, with initialized Observable Collections.
        /// </summary>
        public House house { get; set; } = new House() {
            titles = new ObservableCollection<string>(),
            seats = new ObservableCollection<string>(),
            ancestralWeapons = new ObservableCollection<string>(),
            cadetBranches = new ObservableCollection<House>(),
            swornMembers = new ObservableCollection<Character>()
        };

        /// <summary>
        /// Databound Book entity, with initialized Observable Collections.
        /// </summary>
        public Book book { get; set; } = new Book() { 
            authors = new ObservableCollection<string>(),
            characters = new ObservableCollection<Character>(),
            povCharacters = new ObservableCollection<Character>()
        };

        /// <summary>
        /// Databound property of the shown logo image
        /// </summary>
        public ImageWrapper imageitem { get; set; }

        /// <summary>
        /// Keeps track of the last search.
        /// This is used to be able to handle paging of a shown list.
        /// </summary>
        private string lastsearch = "";

        /// <summary>
        /// In book mode, the characters list is pageable, in house mode, the sworn members list.
        /// If the Page is in the right mode, swaps the pageable list to the next page.
        /// </summary>
        public async Task NextPage(Mode mode)
        {
            if (mode == Mode.Book)
            {
                var newcharacters = await BookService.Instance.NextPage(lastsearch);
                RepopulateObservableCollection(newcharacters, book.characters);
            }
            else if (mode == Mode.House)
            {
                var newcharacters = await HouseService.Instance.NextPage(lastsearch);
                RepopulateObservableCollection(newcharacters, house.swornMembers);
            }
        }

        /// <summary>
        /// In book mode, the characters list is pageable, in house mode, the sworn members list.
        /// If the Page is in the right mode, swaps the pageable list to the previous page.
        /// </summary>
        public async Task PreviousPage(Mode mode)
        {
            if (mode == Mode.Book)
            {
                var newcharacters = await BookService.Instance.PreviousPage(lastsearch);
                RepopulateObservableCollection(newcharacters, book.characters);
            }
            else if (mode == Mode.House)
            {
                var newcharacters = await HouseService.Instance.PreviousPage(lastsearch);
                RepopulateObservableCollection(newcharacters, house.swornMembers);
            }
        }

        /// <summary>
        /// The constructor sets the value of the logo to the one selected.
        /// </summary>
        public DetailsPageViewModel()
        {
            SetImage();
        }

        /// <summary>
        /// Sets the image of the logo to the one saved to the LocalSettings.
        /// </summary>
        private void SetImage()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string value = localSettings.Values["favouritehouse"] as string;
            if (value == null)
            {
                imageitem = new ImageWrapper() { Image = new BitmapImage(new Uri("ms-appx:///Assets/starklogo.png")) };
            }
            else
            {
                imageitem = new ImageWrapper() { Image = new BitmapImage(new Uri($"ms-appx:///Assets/{value}logo.png")) };
            }
        }

        /// <summary>
        /// Calls the right service for the job, and puts the details of the service's query result into the right. databound property.
        /// </summary>
        public async Task ShowDetails(string searchstr)
        {
            if (searchstr.Contains("characters"))
            {
                var charact = await GetCharacter(searchstr);
                lastsearch = searchstr;
                BuildCharacter(charact);
            }
            else if (searchstr.Contains("houses"))
            {
                var hous = await GetHouse(searchstr);
                lastsearch = searchstr;
                BuildHouse(hous);
            }
            else if (searchstr.Contains("books"))
            {
                var bookhelper = await GetBook(searchstr);
                lastsearch = searchstr;
                BuildBook(bookhelper);
            }
            else
            {
                throw new Exception("Se nem karakter, se nem ház, se nem könyv? Akkor mi?");
            }
        }

        /// <summary>
        /// Returns the URI of a character from its name.
        /// </summary>
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

        /// <summary>
        /// Fills the properties of the databound book.
        /// </summary>
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

        /// <summary>
        /// Fills the properties of the databound house.
        /// </summary>
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


        /// <summary>
        /// Fills the properties of the databound character.
        /// </summary>
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


        /// <summary>
        /// Empties the destination collection, then copies the source's elements into it.
        /// </summary>
        private void RepopulateObservableCollection<T>(IReadOnlyCollection<T> source, ObservableCollection<T> destination)
        {
            destination.Clear();
            foreach (T element in source)
            {
                destination.Add(element);
            }
        }

        /// <summary>
        /// Sends a request for a Character to the API through the Service.
        /// </summary>
        private async Task<Character> GetCharacter(string str)
        {
            return await CharacterService.Instance.GetCharacterAsyncFromFullUrl(new Uri(str), depth:0);
        }

        /// <summary>
        /// Sends a request for a Book to the API through the Service.
        /// </summary>
        private async Task<Book> GetBook(string str)
        {
            return await BookService.Instance.GetBookAsyncFromFullUrl(new Uri(str), depth: 0);
        }

        /// <summary>
        /// Sends a request for a House to the API through the Service.
        /// </summary>
        private async Task<House> GetHouse(string str)
        {
            return await HouseService.Instance.GetHouseAsyncFromFullUrl(new Uri(str), depth: 0);
        }
    }
}
