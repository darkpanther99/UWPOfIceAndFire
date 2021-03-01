using System;
using System.Collections.Generic;
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
        private BookService bookservice = BookService.Instance;
        private CharacterService characterService = CharacterService.Instance;
        private List<Book> books = new List<Book>();
        private List<Character> characters = new List<Character>();
        public Character character { get; set; } = new Character() { name = "jani" };

        public async void Test(string searchstr)
        {
            var temp = await characterService.GetCharactersAsyncFromName(searchstr);
            Debug.WriteLine(temp[0].name);
            character = temp[0];
        }

        private async Task<Character> GetCharacter(int id)
        {
            return await characterService.GetCharacterAsync(id);
        }

        private async Task<Book> GetBook(int id)
        {
            return await bookservice.GetBookAsync(id);
        }
    }
}
