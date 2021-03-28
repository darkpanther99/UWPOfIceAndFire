using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services.HelperModels;
using System.Collections.ObjectModel;

namespace TXC54G_HF.Services
{
    /// <summary>
    /// Singleton Service, which forwards Book queries to the API.
    /// </summary>
    class BookService : BaseService
    {
        #region Singleton things
        private BookService() { }
        private static BookService instance = null;
        public static BookService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookService();
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Stores how many books can be on the same page.
        /// </summary>
        private int pageSize = 30;

        private int start = 0;

        /// <summary>
        /// Stores how many characters can be on the same page(the list of characters in a book is pageable).
        /// </summary>
        private const int charactersonpage = 10;

        /// <summary>
        /// Returns the book with the correct ID.
        /// </summary>
        public async Task<Book> GetBookAsync(int id)
        {
            return await GetBookAsyncFromFullUrl(new Uri(baseUrl, $"books/{id}"), depth: 0);
        }

        /// <summary>
        /// Returns a preview object from books on the specified page.
        /// </summary>
        public async Task<List<BookHelper>> GetBooksPreviewAsync(int page)
        {
            return await GetAsync<List<BookHelper>>(new Uri(baseUrl, $"books?page={page}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a preview object from books with the specified name.
        /// </summary>
        public async Task<List<BookHelper>> GetBooksPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<BookHelper>>(new Uri(baseUrl, $"books?name={name}&pageSize={pageSize}"));
        }

        /// <summary>
        /// Returns a book object from the specified URI.
        /// This is a recursive function, because the JSON responses from the API are pointing to each other by URI-s.
        /// Gets all details of a book from the API, and for all the nested URI-s, it calls the right service.
        /// When it has constructed the full book object, returns it.
        /// </summary>
        public async Task<Book> GetBookAsyncFromFullUrl(Uri uri, int depth)
        {
            if (depth > 1)
            {
                return null;
            }

            CharacterService characterService = CharacterService.Instance;
            var bookhelper = await GetAsync<BookHelper>(uri);

            var book = new Book()
            {
                url = bookhelper.url,
                name = bookhelper.name,
                isbn = bookhelper.isbn,
                authors = new ObservableCollection<string>(),
                numberOfPages = bookhelper.numberOfPages,
                publisher = bookhelper.publisher,
                country = bookhelper.country,
                mediaType = bookhelper.mediaType,
                released = bookhelper.released,
                characters = new ObservableCollection<Character>(),
                povCharacters = new ObservableCollection<Character>()
            };

            foreach (var author in bookhelper.authors)
            {
                book.authors.Add(author);
            }
            start = 0;
            int end = start + charactersonpage;
            for (int i = start; i < end; ++i)
            {
                Character c = await characterService.GetCharacterAsyncFromFullUrl(new Uri(bookhelper.characters[i]), depth + 1);
                book.characters.Add(c);
            }
            foreach (var bookcharacter in bookhelper.povCharacters)
            {
                Character pc = await characterService.GetCharacterAsyncFromFullUrl(new Uri(bookcharacter), depth + 1);
                book.povCharacters.Add(pc);
            }

            return book;
        }

        /// <summary>
        /// Returns a List of the next charactersonpage number of characters.
        /// </summary>
        public async Task<List<Character>> NextPage(string searchstr)
        {
            //ezt lehet lehetne gyorsítani, kevesebb fölösleges dolgot lekérdezni
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
        /// Gets a bookhelper object from the API, and returns a List of the bookhelper's characters in the specified interval(charactersonpage number of characters, starting from the start field's value)
        /// </summary>
        private async Task<List<Character>> GetCharactersFromInterval(Uri uri)
        {
            var bookhelper = await GetAsync<BookHelper>(uri);
            var res = new List<Character>();
            int end = start + charactersonpage;
            if (end > bookhelper.characters.Count())
            {
                end = bookhelper.characters.Count();
            }
            for (int i = start; i < end; ++i)
            {
                Character c = await CharacterService.Instance.GetCharacterAsyncFromFullUrl(new Uri(bookhelper.characters[i]), depth: 1);
                res.Add(c);
            }
            return res;
        }

    }
}
