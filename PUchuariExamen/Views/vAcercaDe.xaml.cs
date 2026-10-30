namespace PUchuariExamen.Views;

public partial class vAcercaDe : ContentPage
{
    public vAcercaDe()
    {
        InitializeComponent();
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
