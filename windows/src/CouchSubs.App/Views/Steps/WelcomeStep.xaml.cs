// SPDX-License-Identifier: AGPL-3.0-only

using System.Windows.Controls;
using System.Windows;
using CouchSubs.Core;
using CouchSubs.Shell;

namespace CouchSubs.App.Views.Steps;

public partial class WelcomeStep : UserControl
{
    public WelcomeStep()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            Ps5.IsChecked = !Controller.IsXbox;
            Xbox.IsChecked = Controller.IsXbox;
            Ps5.IsEnabled = Xbox.IsEnabled = !Controller.ConsoleKindLocked;
            ShowRequirements();
        };
    }

    private AppController Controller => (AppController)DataContext;

    private void OnSelected(object sender, RoutedEventArgs e)
    {
        if (!IsLoaded) return;
        Controller.SelectConsoleKind(Xbox.IsChecked == true ? GameConsoleKind.Xbox : GameConsoleKind.PlayStation);
        ShowRequirements();
    }

    private void ShowRequirements()
    {
        if (Controller.IsXbox)
        {
            Layout.Message = "Xbox desteği beta aşamasındadır. CouchSubs oyun görüntüsündeki altyazıları okur ve çeviriyi TV'de ya da bilgisayarda gösterir. Oynamaya kendi kumandanızla devam edersiniz.";
            Requirements.Text = "Gerekenler: aynı ağdaki ev Xbox konsolunuz, Microsoft hesabınız ve altyazının görüneceği TV ya da bilgisayar. Xbox'ta uzaktan oynatma açık olmalıdır.";
            return;
        }
        // PS5 seçiliyken metinler önceki karşılama ekranıyla aynı: ortak katalogdan, her dilde.
        var text = Localizer.Current;
        Layout.Message = text["CouchSubs reads the English subtitles of your PS5 games and shows a Turkish translation on your TV, on top of the game. You keep playing on the TV with your own controller."];
        Requirements.Text = text["You will need:"] + "\n• " + text["A PS5 on the same network as this computer"]
            + "\n• " + text["An LG TV with the CouchSubs TV app"] + "\n• " + text["A free second PlayStation Network account"];
    }
}
