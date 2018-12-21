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
        
        List<string> stationBeginStrings;
        List<string> stationEndStrings;

        dynamic stationBeginData;
        dynamic stationEndData;
        public InputPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);

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
            arrowImg.Source = ImageSource.FromResource("Centrotrans.images.arrow.png");

            departureDate.MinimumDate = DateTime.Today;
            returnDate.MinimumDate = DateTime.Today.AddDays(-1);

            stationBegin.SortingAlgorithm = sorting;
            stationEnd.SortingAlgorithm = sorting;

            returnLbl.IsEnabled = false;
            returnDate.IsEnabled = false;

            button.Pressed += async (sender, args) =>
            {
                if (stationBeginStrings.Contains(stationBegin.Text) && stationEndStrings.Contains(stationEnd.Text))
                {
                    //Application.Current.MainPage = new NavigationPage(new MainPage());

                    await Navigation.PushAsync(new StationList((int)stationBeginData[stationBeginStrings.IndexOf(stationBegin.Text)].stationId.Value,
                                                        (int)stationEndData[stationEndStrings.IndexOf(stationEnd.Text)].stationId.Value,
                                                        departureDate.Date,
                                                        (returnSwitch.IsToggled ? returnDate.Date : DateTime.Today.AddDays(-1))));
                    /*
                    await (Application.Current as App).NavigationPage.PushAsync(new StationList((int)stationBeginData[stationBeginStrings.IndexOf(stationBegin.Text)].stationId.Value,
                                                        (int)stationEndData[stationEndStrings.IndexOf(stationEnd.Text)].stationId.Value,
                                                        datePicker.Date));*/
                }
            };

            returnSwitch.Toggled += async (sender, args) =>
            {
                returnLbl.IsEnabled = returnSwitch.IsToggled;
                returnDate.IsEnabled = returnSwitch.IsToggled;
            };

            departureDate.DateSelected += async (sender, args) =>
            {
                returnDate.MinimumDate = departureDate.Date;
            };
        }

    }
}