using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;

namespace TXC54G_HF.Services
{
    class BookService : BaseService
    {
        CharacterService characterService = new CharacterService();
        public async Task<Book> GetBookAsync(int id)
        {
            var book = await GetAsync<Book>(new Uri(baseUrl, $"books/{id}"));
            foreach (var b in book.characters)
            {
                try
                {
                    await characterService.GetCharacterAsyncFromFullUrl(b);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return book;


        }

        public async Task<Book> GetBookAsyncFromFullUrl(string uri)
        {
            var book = await GetAsync<Book>(new Uri(uri));
            foreach (var b in book.characters)
            {
                try
                {
                    await characterService.GetCharacterAsyncFromFullUrl(b);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return book;
        }

    }
}
