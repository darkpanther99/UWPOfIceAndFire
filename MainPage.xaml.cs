using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TXC54G_HF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private BookService bookservice = BookService.Instance;
        private CharacterService characterService = CharacterService.Instance;
        private List<Book> books = new List<Book>();
        private List<Character> characters = new List<Character>();
        public Character character { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            character = new Character()
            { name = "Jani" };
            DataContext = this;
            //book = GetBook(1).Result;
            Test();


        }

        private async void Test()
        {
            //books.Add(await GetBook(1));
            //characters.Add(await GetCharacter(2));
            //Debug.WriteLine(characters[0].name);
            //character = characters[0];
            //var temp = await GetCharacter(2);
            var temp = await characterService.GetCharactersAsyncFromName("Jon Snow");
            Debug.WriteLine(temp[0].name);
            character.name = temp[0].name;
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
