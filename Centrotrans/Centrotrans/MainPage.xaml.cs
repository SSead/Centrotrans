using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xfx;
using Xamarin.Forms.Xaml;

namespace Centrotrans
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            Console.WriteLine("\n#######");
            new NavigationPage(new StationList(1, 9, DateTime.Today));

        }
        
    }   
}