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
        private Boolean lastCommandWasSearch = false;
        private string lastSearchText = "";
        public string currentlyBrowsing { get; set; } = "books";
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
        public void Search(string searchtext, int cnt)
        {
            lastCommandWasSearch = true;
            if (cnt != lastClicked)
            {
                initPage();
            }
            lastClicked = cnt;
            switch (cnt)
            {
                case 0:
                    SearchBooks(searchtext);
                    break;
                case 1:
                    SearchHouses(searchtext);
                    break;
                case 2:
                    SearchCharacters(searchtext);
                    break;
                default:
                    break;
            }
            lastSearchText = searchtext;
        }
        
        public void ListPreviews(int cnt)
        {
            lastCommandWasSearch = false;
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
            if (lastCommandWasSearch)
            {
                Search(lastSearchText, lastClicked);
            }
            else
            {
                ListPreviews(lastClicked);
            }
        }
        private async void SearchCharacters(string searchtext)
        {
            var chsInstance = CharacterService.Instance;
            var previewitems = await chsInstance.GetCharactersPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
            var previewcultures = await chsInstance.GetCharactersPreviewAsyncFromCulture(searchtext, page);
            AppendToListitems(previewcultures);
            var previewborn = await chsInstance.GetCharactersPreviewAsyncFromBirth(searchtext, page);
            AppendToListitems(previewborn);
            var previewdied = await chsInstance.GetCharactersPreviewAsyncFromDeath(searchtext, page);
            AppendToListitems(previewdied);
            if (searchtext.ToLower() == "true" || searchtext.ToLower() == "false")
            {
                var previewIsAlive = await chsInstance.GetCharactersPreviewAsyncFromIsAlive(searchtext, page);
                AppendToListitems(previewIsAlive);
            }
            if (searchtext.ToLower() == "male" || searchtext.ToLower() == "female")
            {
                var previewgenders = await chsInstance.GetCharactersPreviewAsyncFromGender(searchtext, page);
                AppendToListitems(previewgenders);
            }
            if (searchtext.ToLower() == "alive")
            {
                var previewIsAlive = await chsInstance.GetCharactersPreviewAsyncFromIsAlive("true", page);
                AppendToListitems(previewIsAlive);
            }
            if (searchtext.ToLower() == "dead")
            {
                var previewIsAlive = await chsInstance.GetCharactersPreviewAsyncFromIsAlive("false", page);
                AppendToListitems(previewIsAlive);
            }



        }
        private async void SearchBooks(string searchtext)
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
        }

        private async void SearchHouses(string searchtext)
        {
            var hsInstance = HouseService.Instance;
            var previewitems = await hsInstance.GetHousesPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
            var previewregions = await hsInstance.GetHousesPreviewAsyncFromRegion(searchtext, page);
            AppendToListitems(previewregions);
            var previewwords = await hsInstance.GetHousesPreviewAsyncFromWords(searchtext, page);
            AppendToListitems(previewwords);
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
                if(p.name == "")
                {
                    p.name = "Unnamed";
                }
                listitems.Add(p);
            }
        }

        private void AppendToListitems(IReadOnlyCollection<BaseHelper> previewitems)
        {
            foreach (var p in previewitems)
            {
                if (p.name == "")
                {
                    p.name = "Unnamed";
                }
                listitems.Add(p);
            }
        }

        public async Task<IEnumerable<string>> GetEverything(int mode)
        {
            var res = new List<string>();
            int pagelocal = 1; //Paging starts from 1
            switch (mode)
            {
                case 0:
                    var bookpreviewitems = await BookService.Instance.GetBooksPreviewAsync(pagelocal);
                    while (bookpreviewitems.Count > 0)
                    {
                        foreach(var i in bookpreviewitems)
                        {
                            if (i.name == "")
                            {
                                res.Add("Unnamed");
                            }
                            else
                            {
                                res.Add(i.name);
                            }
                        }
                        pagelocal++;
                        bookpreviewitems = await BookService.Instance.GetBooksPreviewAsync(pagelocal);
                    }
                    break;
                case 1:
                    var housepreviewitems = await HouseService.Instance.GetHousesPreviewAsync(pagelocal);
                    while (housepreviewitems.Count > 0)
                    {
                        foreach (var i in housepreviewitems)
                        {
                            if (i.name == "")
                            {
                                res.Add("Unnamed");
                            }
                            else
                            {
                                res.Add(i.name);
                            }
                        }
                        pagelocal++;
                        housepreviewitems = await HouseService.Instance.GetHousesPreviewAsync(pagelocal);
                    }
                    break;
                case 2:
                    var characterpreviewitems = await CharacterService.Instance.GetCharactersPreviewAsync(pagelocal);
                    while (characterpreviewitems.Count > 0)
                    {
                        foreach (var i in characterpreviewitems)
                        {
                            if (i.name == "")
                            {
                                res.Add("Unnamed");
                            }
                            else
                            {
                                res.Add(i.name);
                            }
                        }
                        pagelocal++;
                        characterpreviewitems = await CharacterService.Instance.GetCharactersPreviewAsync(pagelocal);
                    }
                    break;
                default:
                    break;
            }


            return res;
        }


    }
}
