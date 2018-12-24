using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;

namespace Centrotrans
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StationPopup : PopupPage
	{
        public class Item
        {
            public Item(dynamic line, int entryIndex, int timeTableIndex)
            {
                this.line = line;
                this.entryIndex = entryIndex;


                stationName = line.timeTables[timeTableIndex].entries[entryIndex].station.name.ToString();
                time = DateTime.Parse(line.timeTables[timeTableIndex].departTime.ToString())
                    .AddSeconds(3600 + (int)line.timeTables[timeTableIndex].entries[entryIndex].timeSeconds)
                    .ToString("HH:mm");
                
                if ((int)line.subLineStationFrom == (int)line.timeTables[timeTableIndex].entries[entryIndex].station.stationId ||
                    (int)line.subLineStationTo == (int)line.timeTables[timeTableIndex].entries[entryIndex].station.stationId)
                {
                    imageSource = ImageSource.FromResource("Centrotrans.images.pin.png");
                } else
                {
                    imageSource = ImageSource.FromResource("Centrotrans.images.dot.png");
                }
            }

            public dynamic line { get; set; }
            public int entryIndex { get; set; }
            public string stationName { get; set; }
            public string time { get; set; }
            public ImageSource imageSource { get; set; }
        }

        public ObservableCollection<Item> Items { get; set; }
        public StationPopup (dynamic data, int timeTabelIndex)
		{
			InitializeComponent();

            Items = new ObservableCollection<Item> { };
            for (int i = 0; i < data.timeTables[timeTabelIndex].entries.Count; i++)
            {
                Items.Add(new Item(data, i, timeTabelIndex));
            }

            MyList.ItemsSource = Items;
            
            arrowImg.Source = ImageSource.FromResource("Centrotrans.images.arrow.png");

            firstStation.Text = findStationName(data, (int)data.subLineStationFrom);
            lastStation.Text = findStationName(data, (int)data.subLineStationTo);

            
            departTime.Text = DateTime.Parse(data.timeTables[timeTabelIndex].departTime.ToString())
                .AddSeconds(3600 + (int)data.timeTables[timeTabelIndex].entries[stationIndexInTimeTable(data.timeTables[timeTabelIndex], (int)data.subLineStationFrom)].timeSeconds)
                .ToString("HH:mm");

            arrivalTime.Text = DateTime.Parse(data.timeTables[timeTabelIndex].departTime.ToString())
                  .AddSeconds(3600 + (int)data.timeTables[timeTabelIndex].entries[stationIndexInTimeTable(data.timeTables[timeTabelIndex], (int)data.subLineStationTo)].timeSeconds)
                  .ToString("HH:mm");

            partner.Text += data.partner.name.ToString();

            price.Text = data.linePrice.ToString() + " " + data.currency.ToString();

            var tgr = new TapGestureRecognizer();
            tgr.Tapped += async (sender, args) =>
            {
                PopupNavigation.PopAsync(true);
            };

            close.GestureRecognizers.Add(tgr);
        }

        private string findStationName(dynamic data, int stationId)
        {
            foreach (var i in data.timeTables[0].entries)
            {
                if ((int)i.station.stationId == stationId) return i.station.name.ToString();
            }

            return "";
        }

        private int stationIndexInTimeTable(dynamic data, int stationId)
        {
            for (int i = 0; i < data.entries.Count; i++)
            {
                if ((int)data.entries[i].station.stationId == stationId) return i;
            }

            return 0;
        }
	}
}