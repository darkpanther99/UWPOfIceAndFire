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
    /// <summary>
    /// VM of the Main page, it handles the code behind's requests.
    /// </summary>
    class MainPageViewModel
    {
        /// <summary>
        /// An observable collection of the listed names.
        /// </summary>
        public ObservableCollection<BaseHelper> listitems { get; set; } = new ObservableCollection<BaseHelper>();

        /// <summary>
        /// Keeps track of the paging.
        /// </summary>
        /// <remarks>
        /// Paging starts from 1 in the api, so the default value is 1
        /// </remarks>
        private int page = 1;

        #region Paging methods
        /// <summary>
        /// Reinitializes the page.
        /// </summary>
        public void initPage()
        {
            page = 1;
        }
        /// <summary>
        /// Increments the page.
        /// </summary>
        public void nextPage()
        {
            page++;
        }
        /// <summary>
        /// Decrements the page, while not letting it go below 1.
        /// </summary>
        public void previousPage()
        {
            page--;
            if (page < 1)
            {
                initPage();
            }
        }
        #endregion

        /// <summary>
        /// Keeps track of the last clicked Mode.
        /// </summary>
        private Mode lastClicked = Mode.Undefined;

        /// <summary>
        /// Data about the last query, stores if it was full listing or search.
        /// </summary>
        private bool lastCommandWasSearch = false;

        /// <summary>
        /// Data about the last query's text.
        /// </summary>
        private string lastSearchText = "";

        /// <summary>
        /// Databound property of the shown logo image
        /// </summary>
        public ImageWrapper imageitem { get; set; }

        /// <summary>
        /// The constructor sets the value of the logo to the one selected.
        /// </summary>
        public MainPageViewModel()
        {
            SetImage();
        }

        /// <summary>
        /// Sets the image of the logo to the one saved to the LocalSettings.
        /// </summary>
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

        /// <summary>
        /// Calls the correct search method depending on which type of entity the user wants to search for.
        /// </summary>
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

        /// <summary>
        /// Calls the correct listing method depending on which type of entity the user wants to make a listing of.
        /// </summary>
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

        /// <summary>
        /// Switches to the next page and lists its contents.
        /// </summary>
        public async Task ListNextPageOfPreviews()
        {
            this.nextPage();
            await ListNewPageOfPreviews();
        }

        /// <summary>
        /// Switches to the previous page and lists its contents.
        /// </summary>
        public async Task ListPrevPageOfPreviews()
        {
            this.previousPage();
            await ListNewPageOfPreviews();
        }

        /// <summary>
        /// Repeats the last command.
        /// </summary>
        private async Task ListNewPageOfPreviews()
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

        /// <summary>
        /// Makes some input validation, then forwards the query to the CharacterService.
        /// Puts the return value into the Observable Listitems Collection.
        /// </summary>
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

        /// <summary>
        /// Forwards the query to the BookService.
        /// Puts the return value into the Observable Listitems Collection.
        /// </summary>
        private async Task SearchBooks(string searchtext)
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsyncFromName(searchtext);
            RepopulateListitems(previewitems);
        }

        /// <summary>
        /// Forwards the query to the HouseService.
        /// Puts the return value into the Observable Listitems Collection.
        /// </summary>
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

        /// <summary>
        /// Forwards the listing query to the CharacterService.
        /// Puts the return value into the Observable Listitems Collection.
        /// </summary>
        private async Task ListCharacters()
        {
            var previewitems = await CharacterService.Instance.GetCharactersPreviewAsync(page);
            RepopulateListitems(previewitems);
        }

        /// <summary>
        /// Forwards the listing query to the HouseService.
        /// Puts the return value into the Observable Listitems Collection.
        /// </summary>
        private async Task ListHouses()
        {
            var previewitems = await HouseService.Instance.GetHousesPreviewAsync(page);
            RepopulateListitems(previewitems);
        }

        /// <summary>
        /// Forwards the listing query to the BookService.
        /// Puts the return value into the Observable Listitems Collection.
        /// </summary>
        private async Task ListBooks()
        {
            var previewitems = await BookService.Instance.GetBooksPreviewAsync(page);
            RepopulateListitems(previewitems);
        }

        /// <summary>
        /// Clears the Listitems Collection and fills it from the parameter Collection.
        /// Entities without names get the name "Unnamed" instead of an empty string.
        /// </summary>
        private void RepopulateListitems(IReadOnlyCollection<BaseHelper> previewitems)
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

        /// <summary>
        /// Fills the Listitems Collection from the parameter Collection.
        /// Entities without names get the name "Unnamed" instead of an empty string.
        /// </summary>
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

        /// <summary>
        /// Opens a FilePicker, and writes the query result into it.
        /// </summary>
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
