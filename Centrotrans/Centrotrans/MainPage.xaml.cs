using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Centrotrans
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        Image image;
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            image = new Image
            {
                Source = ImageSource.FromResource("Centrotrans.images.logo.png")
            };

            this.Content = image;

            animation();
            //new NavigationPage(new StationList(1, 9, DateTime.Today));

        }

        private async void animation()
        {
            var inputPage = new InputPage();

            await image.FadeTo(0, 0);
            await image.FadeTo(1, 2000);

            await Navigation.PushAsync(inputPage);
        }
        
    }   
}