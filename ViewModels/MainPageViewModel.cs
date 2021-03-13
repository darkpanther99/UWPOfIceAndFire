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
using TXC54G_HF.ViewModels.Utilities;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

namespace TXC54G_HF.ViewModels
{
    class MainPageViewModel
    {
        public ObservableCollection<BaseHelper> listitems { get; set; } = new ObservableCollection<BaseHelper>();
        private int page = 1;
        private Mode lastClicked = Mode.Undefined;
        private bool lastCommandWasSearch = false;
        private string lastSearchText = "";
        public string currentlyBrowsing { get; set; } = "books";
        public ImageWrapper imageitem { get; set; }
        public MainPageViewModel()
        {
            SetImage();
        }

        private void SetImage()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string value = localSettings.Values["favouritehouse"] as string;
            if (value == null)
            {
                imageitem = new ImageWrapper() { Image = new BitmapImage(new Uri("ms-appx:///Assets/starklogo.png")) };
            }
            else
            {
                imageitem = new ImageWrapper() { Image = new BitmapImage(new Uri($"ms-appx:///Assets/{value}logo.png")) };
            }
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
        public async Task Search(string searchtext, Mode mode)
        {
            //If the search text is empty, I make a listing instead and return.
            if (searchtext.Length < 1)
            {
                await ListPreviews(mode);
                return;
            }
            lastCommandWasSearch = true;
            if (mode != lastClicked)
            {
                initPage();
            }
            lastClicked = mode;
            switch (mode)
            {
                case Mode.Book:
                    await SearchBooks(searchtext);
                    break;
                case Mode.House:
                    await SearchHouses(searchtext);
                    break;
                case Mode.Character:
                    await SearchCharacters(searchtext);
                    break;
                default:
                    break;
            }
            lastSearchText = searchtext;
        }
        
        public async Task ListPreviews(Mode mode)
        {
            lastCommandWasSearch = false;
            if (mode != lastClicked)
            {
                initPage();
            }
            lastClicked = mode;
            switch (mode)
            {
                case Mode.Book:
                    await ListBooks();
                    break;
                case Mode.House:
                    await ListHouses();
                    break;
                case Mode.Character:
                    await ListCharacters();
                    break;
                default:
                    break;
            }
        }
        public async Task ListNewPageOfPreviews()
        {
            if (lastCommandWasSearch)
            {
                await Search(lastSearchText, lastClicked);
            }
            else
            {
                await ListPreviews(lastClicked);
            }
        }
        private async Task SearchCharacters(string searchtext)
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
        private async Task SearchBooks(string searchtext)
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
        }

        private async Task SearchHouses(string searchtext)
        {
            var hsInstance = HouseService.Instance;
            var previewitems = await hsInstance.GetHousesPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
            var previewregions = await hsInstance.GetHousesPreviewAsyncFromRegion(searchtext, page);
            AppendToListitems(previewregions);
            var previewwords = await hsInstance.GetHousesPreviewAsyncFromWords(searchtext, page);
            AppendToListitems(previewwords);
        }

        private async Task ListCharacters()
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsync(page);
            RepopulateListitems(previewitems);
        }
        private async Task ListHouses()
        {
            var previewitems = await HouseService.Instance.GetHousesPreviewAsync(page);
            RepopulateListitems(previewitems);
        }
        private async Task ListBooks()
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

        public async Task SaveToFile(Mode mode)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".txt");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var toWrite = await GetEverything(mode);
                Debug.WriteLine("írás kezdődik!");
                //var lines = await FileIO.ReadLinesAsync(file);
                await FileIO.WriteLinesAsync(file, toWrite);
            }
            else
            {
            }
        }

        public async Task<IEnumerable<string>> GetEverything(Mode mode)
        {
            var res = new List<string>();
            int pagelocal = 1; //Paging starts from 1
            switch (mode)
            {
                case Mode.Book:
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
                case Mode.House:
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
                case Mode.Character:
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
