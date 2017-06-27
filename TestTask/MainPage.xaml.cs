using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TestTask.DataService;

namespace TestTask
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnStartSearch_Click(object sender, RoutedEventArgs e)
        {
            //Calling web service
            DataClient webService = new DataClient();
            var response=await webService.GetDataAsync(tbSearchField.Text.Trim());
            List<Setting> resultList = response.ToList();
            //Adding items to ListView
            lvSetting.ItemsSource = resultList;
            await webService.CloseAsync();
        }

        private void btnStartSearch_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Calling web service and adding item to ListView on loading event
            DataClient webService = new DataClient();
            var response = await webService.GetLastResultAsync();
            List<Setting> resultList = response.ToList();
            lvSetting.ItemsSource = resultList;
            await webService.CloseAsync();
        }
    }
}
