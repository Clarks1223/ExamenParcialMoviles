using System.Globalization;
using PUchuariExamen.Models;

namespace PUchuariExamen.Views;

public partial class vResumen : ContentPage
{
    public vResumen(string usuario, RegistroGimnasio registro)
    {
        InitializeComponent();

        lblUsuarioConectado.Text = $"Usuario conectado: {usuario}";

        lblNombre.Text = registro.Nombre;
        lblApellido.Text = registro.Apellido;
        lblEdad.Text = registro.Edad.ToString(CultureInfo.InvariantCulture);
        lblFechaInicio.Text = registro.FechaInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        lblPlan.Text = registro.Plan;
        lblCasillero.Text = registro.Casillero ? "Sí" : "No";
        lblMontoInicial.Text = FormatearMonto(registro.MontoInicial);
        lblPagoMensual.Text = FormatearMonto(registro.PagoMensual);
        lblPagoTotal.Text = FormatearMonto(registro.PagoTotal);
    }

    private static string FormatearMonto(decimal monto) =>
        $"$ {monto.ToString("F2", CultureInfo.InvariantCulture)}";

    private async void OnNuevoRegistroClicked(object sender, EventArgs e)
    {
        // Vuelve al mismo Registro (mantiene el usuario) con los campos en blanco
        var registro = Navigation.NavigationStack.OfType<vRegistro>().LastOrDefault();
        registro?.LimpiarFormulario();

        await Navigation.PopAsync();
    }

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Cerrar sesión", "¿Desea cerrar la sesión?", "Sí", "No");
        if (!confirmar)
        {
            return;
        }

        // Se descarta la pila de navegación: no se puede volver al Registro
        if (Window is not null)
        {
            Window.Page = new NavigationPage(new vLogin());
        }
    }
}
