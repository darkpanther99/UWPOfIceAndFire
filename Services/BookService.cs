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
    class BookService : BaseService
    {
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
        
        public async Task<Book> GetBookAsync(int id)
        {
            return await GetBookAsyncFromFullUrl(new Uri(baseUrl, $"books/{id}"), depth: 0);
        }
        public async Task<List<BookHelper>> GetBooksPreviewAsync(int page)
        {
            return await GetAsync<List<BookHelper>>(new Uri(baseUrl, $"books?page={page}&pageSize=30"));
        }
        public async Task<List<BookHelper>> GetBooksPreviewAsyncFromName(string name)
        {
            return await GetAsync<List<BookHelper>>(new Uri(baseUrl, $"books?name={name}&pageSize=30"));
        }

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
            int counter = 0;
            foreach (var bookcharacter in bookhelper.characters)
            {
                    Character c = await characterService.GetCharacterAsyncFromFullUrl(new Uri(bookcharacter), depth + 1);
                    book.characters.Add(c);
                counter++;
                if (counter > 10) break;
            }
            foreach (var bookcharacter in bookhelper.povCharacters)
            {
                    Character pc = await characterService.GetCharacterAsyncFromFullUrl(new Uri(bookcharacter), depth + 1);
                    book.povCharacters.Add(pc);
            }

            return book;
        }

    }
}
