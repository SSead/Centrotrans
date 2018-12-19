using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xfx;


namespace Centrotrans
{

    public partial class InputPage : ContentPage
    {
        public Func<string, ICollection<string>, ICollection<string>> sorting { get; } = (text, values) => values
            .Where(x => x.ToLower().StartsWith(text.ToLower()))
            .OrderBy(x => x)
            .ToList();

        XfxComboBox stationBegin;
        XfxComboBox stationEnd;

        List<string> stationBeginStrings;
        List<string> stationEndStrings;

        dynamic stationBeginData;
        dynamic stationEndData;
        public InputPage()
        {
            InitializeComponent();
            initilizeView();

            getStationBeginData();

            stationBegin.ItemSelected += (sender, args) =>
            {


                getStationEndData((int)stationBeginData[stationBeginStrings.IndexOf(stationBegin.SelectedItem.ToString())].stationId.Value);
            };
        }
        private async void getStationBeginData()
        {
            stationBeginData = await StationAPI.GetStations.post();

            stationBeginStrings = new List<string>();

            foreach (var i in stationBeginData)
            {
                stationBeginStrings.Add(i.name.ToString());
            }

            stationBegin.ItemsSource = stationBeginStrings;
        }
        private async void getStationEndData(int beginStationId)
        {
            stationEndData = await StationAPI.GetPossibleDestinations.post(beginStationId);

            stationEndStrings = new List<string>();

            foreach (var i in stationEndData)
            {
                stationEndStrings.Add(i.name.ToString());
            }

            stationEnd.ItemsSource = stationEndStrings;
            stationEnd.IsEnabled = true;
        }

        private void initilizeView()
        {
            var h = DeviceDisplay.MainDisplayInfo.Height * 0.315;

            var logo = new Image
            {
                Source = ImageSource.FromResource("Centrotrans.images.logo.jpg"),
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = h / 3,
                Aspect = Aspect.AspectFit

            };

            stationBegin = new XfxComboBox
            {
                Placeholder = "Pocetna stanica",
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0, 10, 0),
                SortingAlgorithm = sorting
            };

            stationEnd = new XfxComboBox
            {
                Placeholder = "Krajnja stanica",
                VerticalOptions = LayoutOptions.Center,
                IsEnabled = false,
                Margin = new Thickness(10, 0, 10, 0),
                SortingAlgorithm = sorting
            };

            var datePicker = new DatePicker
            {
                Margin = new Thickness(10, 0, 10, 0),
                MinimumDate = DateTime.Now
            };

            var button = new Button
            {
                Text = "Lista Buseva",
                Margin = new Thickness(50, 0, 50, 0),
                BackgroundColor = Color.FromHex("#2196f3"),
                TextColor = Color.White,
                HeightRequest = 50

            };

            button.Pressed += async (sender, args) =>
            {
                if (stationBeginStrings.Contains(stationBegin.Text) && stationEndStrings.Contains(stationEnd.Text))
                {
                    //Application.Current.MainPage = new NavigationPage(new MainPage());

                    await Navigation.PushAsync(new StationList((int)stationBeginData[stationBeginStrings.IndexOf(stationBegin.Text)].stationId.Value,
                                                        (int)stationEndData[stationEndStrings.IndexOf(stationEnd.Text)].stationId.Value,
                                                        datePicker.Date));
                    /*
                    await (Application.Current as App).NavigationPage.PushAsync(new StationList((int)stationBeginData[stationBeginStrings.IndexOf(stationBegin.Text)].stationId.Value,
                                                        (int)stationEndData[stationEndStrings.IndexOf(stationEnd.Text)].stationId.Value,
                                                        datePicker.Date));*/
                }
            };

            var grid = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(30, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(11, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(11, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(11, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(7, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(9, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(6, GridUnitType.Star) }
                }
            };

            grid.Children.Add(logo, 0, 0);
            grid.Children.Add(stationBegin, 0, 1);
            grid.Children.Add(stationEnd, 0, 3);
            grid.Children.Add(datePicker, 0, 5);
            grid.Children.Add(button, 0, 7);

            this.Content = grid;
        }

    }
}