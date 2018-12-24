using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Centrotrans
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationList : ContentPage
    {
        public class Item
        {
            public Item(dynamic line, int firstStationIndex, int lastStationIndex, int timeTabelIndex)
            {
                this.line = line;
                this.timeTableIndex = timeTableIndex;

                this.firstStation = line.timeTables[timeTabelIndex].entries[firstStationIndex].station.name.ToString();
                this.lastStation = line.timeTables[timeTabelIndex].entries[lastStationIndex].station.name.ToString();

                this.departTime = DateTime.Parse(line.timeTables[timeTabelIndex].departTime.ToString()).AddSeconds(3600 + (int)line.timeTables[timeTabelIndex].entries[firstStationIndex].timeSeconds).ToString("HH:mm");
                this.arrivalTime = DateTime.Parse(line.timeTables[timeTabelIndex].departTime.ToString()).AddSeconds(3600 + (int)line.timeTables[timeTabelIndex].entries[lastStationIndex].timeSeconds).ToString("HH:mm");

                this.price = line.linePrice.ToString() + " " + line.currency.ToString();

                this.distance = ((int)line.timeTables[timeTabelIndex].entries[lastStationIndex].distanceToFirstStationKilometers - (int)line.timeTables[timeTabelIndex].entries[firstStationIndex].distanceToFirstStationKilometers).ToString() + " km";
            }
            
            public dynamic line { get; set; }
            public int timeTableIndex { get; set; }
            public string firstStation { get; set; }
            public string lastStation { get; set; }
            public string departTime { get; set; }
            public string arrivalTime { get; set; }
            public string price { get; set; }
            public string distance { get; set; }
        }


        public ObservableCollection<Item> Items { get; set; }

        int stationBegin;
        int stationEnd;
        DateTime departureDate;
        DateTime returnDate;

        dynamic stationData;
    public StationList(int stationBegin, int stationEnd, DateTime departureDate, DateTime returnDate)
        {
            InitializeComponent();

            this.stationBegin = stationBegin;
            this.stationEnd = stationEnd;
            this.departureDate = departureDate;
            this.returnDate = returnDate;

            getStationData();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = (ListView)sender;
            var item = (Item)list.SelectedItem;

            var begIndex = indexOfStationInTimeTable(stationBegin, item.line.timeTables[0]);
            var endIndex = indexOfStationInTimeTable(stationEnd, item.line.timeTables[0]);
            
            await PopupNavigation.Instance.PushAsync(new StationPopup(item.line, item.timeTableIndex), true);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        private int indexOfStationInTimeTable(int stationId, dynamic timeTable)
        {
            for (int i = 0; i < timeTable.entries.Count; i++)
            {
                if ((int)timeTable.entries[i].station.stationId == stationId)
                {
                    return i;
                }
            }
            return 0;
        }

        private async void getStationData()
        {
            stationData = await StationAPI.GetLinesDrivingThroughStations.post(stationBegin, stationEnd, departureDate, returnDate);


            Items = new ObservableCollection<Item> { };

            foreach (var i in stationData)
            {
                for (int j = 0; j < i.timeTables.Count; j++)
                {
                    Items.Add(new Item(i, indexOfStationInTimeTable(stationBegin, i.timeTables[j]), indexOfStationInTimeTable(stationEnd, i.timeTables[j]), j));
                }
            }

            MyListView.ItemsSource = Items;
        }
    }
}
