using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services.HelperModels;

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

        public async Task<Book> GetBookAsyncFromFullUrl(Uri uri, int depth)
        {
            if (depth > 1)
            {
                return null;
            }
            CharacterService characterService = CharacterService.Instance;
            var bookhelper = await GetAsync<BookHelper>(uri);
            var characters = new List<Character>();
            var povCharacters = new List<Character>();

            foreach (var bookcharacter in bookhelper.characters)
            {
                    Character c = await characterService.GetCharacterAsyncFromFullUrl(new Uri(bookcharacter), depth + 1);
                    characters.Add(c);
            }
            foreach (var bookcharacter in bookhelper.povCharacters)
            {
                    Character pc = await characterService.GetCharacterAsyncFromFullUrl(new Uri(bookcharacter), depth + 1);
                    povCharacters.Add(pc);
            }


            return new Book()
            {
                url = bookhelper.url,
                name = bookhelper.name,
                isbn = bookhelper.isbn,
                authors = bookhelper.authors,
                numberOfPages = bookhelper.numberOfPages,
                publisher = bookhelper.publisher,
                country = bookhelper.country,
                mediaType = bookhelper.mediaType,
                released = bookhelper.released,
                characters = characters,
                povCharacters = povCharacters
            };
        }

    }
}
