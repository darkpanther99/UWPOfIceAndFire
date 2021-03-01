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
        public Character character { get; set; } = new Character() { name = "karakter" };
        public House house { get; set; } = new House() { name = "haz" };
        public Book book { get; set; } = new Book() { name = "konyv" };

        public async void ShowDetails(string searchstr)
        {
            if (searchstr.Contains("characters"))
            {
                var charact = await GetCharacter(searchstr);
                character.name = charact.name;
                character.born = charact.born;
            }
            else if (searchstr.Contains("houses"))
            {
                house = await GetHouse(searchstr);
                Debug.WriteLine(house.name);
            }
            else if (searchstr.Contains("books"))
            {
                book = await GetBook(searchstr);
                Debug.WriteLine(book.name);
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
