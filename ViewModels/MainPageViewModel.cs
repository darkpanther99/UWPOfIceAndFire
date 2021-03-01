using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services;
using TXC54G_HF.Services.HelperModels;

namespace TXC54G_HF.ViewModels
{
    class MainPageViewModel
    {
        public CharacterHelper character { get; set; } = new CharacterHelper() { name = "jani" };
        public ObservableCollection<BaseHelper> listitems { get; set; } = new ObservableCollection<BaseHelper>();
        public async void Search(string searchtext)
        {
            //var previewcharacters = await CharacterService.Instance.GetCharacterPreviewAsync(searchtext);
            
            //character.name = previewcharacters[0].name;

        }
        public async void ListPreviews(int cnt)
        {
            listitems.Clear();
            switch (cnt)
            {
                case 0:
                    ListBooks();
                    break;
                case 1:
                    ListHouses();
                    break;
                case 2:
                    ListCharacters();
                    break;
                default:
                    break;
            }
        }
        private async void ListCharacters()
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsync();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }
        private async void ListHouses()
        {
            var previewitems = await HouseService.Instance.GetHousesPreviewAsync();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }
        private async void ListBooks()
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsync();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }


    }
}
