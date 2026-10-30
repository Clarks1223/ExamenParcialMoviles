using Microsoft.Extensions.DependencyInjection;
using PUchuariExamen.Views;

namespace PUchuariExamen
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new vLogin()));
        }
    }
}