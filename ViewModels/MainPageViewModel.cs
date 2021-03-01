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
        public void Search(string searchtext)
        {
            SearchCharacters(searchtext);
            //SearchBooks(searchtext);
            //SearchHouses(searchtext);
        }
        
        public void ListPreviews(int cnt)
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
        public void ListNewPageOfPreviews()
        {
            ListPreviews(lastClicked);
        }
        private async void SearchCharacters(string searchtext)
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
        }
        private async void SearchBooks(string searchtext)
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
        }
        private async void SearchHouses(string searchtext)
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
        }
        private async void ListCharacters()
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsync(page);
            RepopulateListitems(previewitems);
        }
        private async void ListHouses()
        {
            var previewitems = await HouseService.Instance.GetHousesPreviewAsync(page);
            RepopulateListitems(previewitems);
        }
        private async void ListBooks()
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsync(page);
            RepopulateListitems(previewitems);
        }

        private void RepopulateListitems(IReadOnlyCollection<BaseHelper> previewitems) //IReadOnlyCollection, mert nem írom át a paramétert és így elfogad Generikus típusnak leszármazottat is.
        {
            listitems.Clear();
            foreach (var p in previewitems)
            {
                listitems.Add(p);
            }
        }


    }
}
