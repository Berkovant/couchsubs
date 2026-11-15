// SPDX-License-Identifier: AGPL-3.0-only

using System.Windows.Controls;
using System.Windows;
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

    private void ShowRequirements() => Requirements.Text = Controller.IsXbox
        ? "Gerekenler: aynı ağdaki ev Xbox konsolun, Microsoft hesabın ve altyazıyı göstereceğin TV veya bilgisayar. Xbox üzerinde uzaktan oynatma açık olmalı."
        : "Gerekenler: aynı ağdaki PS5, ücretsiz ikinci PlayStation Network hesabı ve altyazıyı göstereceğin TV veya bilgisayar.";
}
