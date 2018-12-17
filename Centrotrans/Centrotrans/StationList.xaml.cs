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
            public Item(dynamic line, string header)
            {
                this.line = line;
                this.header = header;
            }
            public dynamic line { get; set; }
            public string header { get; set; }
        }


        public ObservableCollection<Item> Items { get; set; }

        int stationBegin;
        int stationEnd;
        DateTime date;

        dynamic stationData;
    public StationList(int stationBegin, int stationEnd, DateTime date)
        {
            InitializeComponent();

            this.stationBegin = stationBegin;
            this.stationEnd = stationEnd;
            this.date = date;

            getStationData();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = (ListView)sender;
            var item = (Item)list.SelectedItem;

            var begIndex = indexOfStationInTimeTable(stationBegin, item.line.timeTables[0]);
            var endIndex = indexOfStationInTimeTable(stationEnd, item.line.timeTables[0]);



            await DisplayAlert(item.line.partner.name,
                "Vrijeme polaska: " + item.line.timeTables[0].departTime.AddSeconds(item.line.timeTables[0].entries[begIndex].timeSeconds).ToString("HH:mm") + "\n\n" +
                "Vrijeme dolaska: " + item.line.timeTables[0].departTime.AddSeconds(item.line.timeTables[0].entries[endIndex].timeSeconds).ToString("HH:mm") + "\n\n" +
                "Cijena karte: " + item.line.linePrice.ToString() + " " + item.line.currency + "\n\n"

                , "OK");


            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private int indexOfStationInTimeTable(int stationId, dynamic timeTable)
        {
            for (int i = 0; i < timeTable.entries.Count; i++)
            {
                if (timeTable.entries[i].station.stationId == stationId)
                {
                    return i;
                }
            }
            return 0;
        }

        private async void getStationData()
        {
            stationData = await StationAPI.GetLinesDrivingThroughStations.post(stationBegin, stationEnd, date);


            Items = new ObservableCollection<Item> { };

            foreach (var i in stationData)
            {
                Console.WriteLine(i.timeTables[0].departTime.ToString() + "  " + i.timeTables[0].entries[0].station.name.ToString() + new String(' ', 20 - (int)i.timeTables[0].entries[0].station.name.ToString().Length) + i.linePrice.ToString() + " " + i.currency.ToString());
                Items.Add(new Item(i, i.timeTables[0].departTime.ToString() + "  " + i.timeTables[0].entries[0].station.name.ToString() + " - " + i.linePrice.ToString() + " " + i.currency.ToString()));
            }

            MyListView.ItemsSource = Items;
        }
    }
}
