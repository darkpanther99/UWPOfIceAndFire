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
        public ObservableCollection<BaseHelper> listitems { get; set; } = new ObservableCollection<BaseHelper>();
        private int page = 1;
        private int lastClicked = -1;
        public async void Search(string searchtext)
        {
            //TODO SEARCH
        }
        public void initPage()
        {
            page = 1;
        }
        public void nextPage()
        {
            page++;
        }
        public void previousPage()
        {
            page--;
            if (page < 1)
            {
                initPage();
            }
        }
        public async void ListPreviews(int cnt)
        {
            if (cnt != lastClicked)
            {
                initPage();
            }
            lastClicked = cnt;
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
        public async void ListNewPageOfPreviews()
        {
            ListPreviews(lastClicked);
        }
        private async void ListCharacters()
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsync(page);
            listitems.Clear();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }
        private async void ListHouses()
        {
            var previewitems = await HouseService.Instance.GetHousesPreviewAsync(page);
            listitems.Clear();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }
        private async void ListBooks()
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsync(page);
            listitems.Clear();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }


    }
}
