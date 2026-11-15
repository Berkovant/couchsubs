// SPDX-License-Identifier: AGPL-3.0-only
using System.Windows;
using System.Windows.Controls;
using CouchSubs.Shell;

namespace CouchSubs.App.Views.Steps;

public partial class XboxLoginStep : UserControl
{
    private bool working;
    public XboxLoginStep() => InitializeComponent();
    private AppController Controller => (AppController)DataContext;

    private async void OnSignIn(object sender, RoutedEventArgs e)
    {
        if (working) return;
        working = true;
        SignIn.IsEnabled = false;
        Error.Text = "";
        Status.Text = "Microsoft giriş sayfası açılıyor…";
        try
        {
            var challenge = await Controller.BeginXboxLoginAsync();
            Code.Text = challenge.Code;
            Instruction.Text = "Tarayıcıdaki Microsoft sayfasına bu kodu gir:";
            Status.Text = "Girişin tamamlanması bekleniyor…";
            await Controller.CompleteXboxLoginAsync();
            Status.Text = "Microsoft girişi tamamlandı. Devam edebilirsin.";
        }
        catch (Exception error)
        {
            Error.Text = error.Message;
            Status.Text = "Giriş tamamlanamadı. Yeniden deneyebilirsin.";
        }
        finally
        {
            working = false;
            SignIn.IsEnabled = true;
        }
    }
}
