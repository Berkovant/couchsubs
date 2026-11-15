// SPDX-License-Identifier: AGPL-3.0-only
using System.Windows;
using System.Windows.Controls;
using CouchSubs.Shell;

namespace CouchSubs.App.Views.Steps;

public partial class XboxConsoleStep : UserControl
{
    public XboxConsoleStep()
    {
        InitializeComponent();
        Loaded += async (_, _) => await SearchAsync();
    }

    private AppController Controller => (AppController)DataContext;
    private async void OnRefresh(object sender, RoutedEventArgs e) => await SearchAsync();

    private async Task SearchAsync()
    {
        Refresh.IsEnabled = false;
        Error.Text = "";
        Status.Text = "Xbox konsolları aranıyor…";
        Consoles.Children.Clear();
        try
        {
            var found = await Controller.DiscoverXboxConsolesAsync();
            Status.Text = found.Count == 0 ? "Bu Microsoft hesabına bağlı ev konsolu bulunamadı." : "Kullanacağın konsolu seç:";
            foreach (var console in found)
            {
                var option = new RadioButton
                {
                    Content = $"{console.Name} ({console.Model})" + (console.StreamingEnabled ? "" : " — uzaktan oynatma kapalı"),
                    GroupName = "xbox", IsEnabled = console.StreamingEnabled,
                    IsChecked = Controller.SelectedXboxConsole?.Id == console.Id,
                    Margin = new Thickness(0, 0, 0, 12),
                };
                option.Checked += (_, _) => Controller.SelectXboxConsole(console);
                Consoles.Children.Add(option);
            }
        }
        catch (Exception error) { Error.Text = error.Message; Status.Text = "Konsollar getirilemedi."; }
        finally { Refresh.IsEnabled = true; }
    }
}
