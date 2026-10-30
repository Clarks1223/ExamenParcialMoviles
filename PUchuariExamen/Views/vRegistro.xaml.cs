using System.Globalization;

namespace PUchuariExamen.Views;

public partial class vRegistro : ContentPage
{
    private readonly string usuario;
    private decimal? pagoMensual;

    public vRegistro(string usuario)
    {
        InitializeComponent();

        this.usuario = usuario;
        lblUsuarioConectado.Text = $"Usuario conectado: {usuario}";

        pkPlan.ItemsSource = ReglasNegocio.Planes;
        LimpiarFormulario();
    }

    /// <summary>Deja el formulario en blanco (usado al iniciar y por "Nuevo registro").</summary>
    public void LimpiarFormulario()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtEdad.Text = string.Empty;
        dpFechaInicio.MinimumDate = DateTime.Today;   // sin fechas pasadas
        dpFechaInicio.Date = DateTime.Today;
        pkPlan.SelectedIndex = -1;
        swCasillero.IsToggled = false;
        txtMontoInicial.Text = string.Empty;
        txtPagoMensual.Text = string.Empty;
        pagoMensual = null;
    }

    // Si cambia el monto inicial o el casillero, la cuota calculada deja de ser válida
    private void OnCasilleroToggled(object sender, ToggledEventArgs e) => InvalidarPagoMensual();

    private void OnMontoInicialTextChanged(object sender, TextChangedEventArgs e) => InvalidarPagoMensual();

    private void InvalidarPagoMensual()
    {
        pagoMensual = null;
        txtPagoMensual.Text = string.Empty;
    }

    private async void OnCalcularClicked(object sender, EventArgs e)
    {
        decimal? montoInicial = await ObtenerMontoInicialValidoAsync();
        if (montoInicial is null)
        {
            return;
        }

        pagoMensual = ReglasNegocio.CalcularPagoMensual(montoInicial.Value, swCasillero.IsToggled);
        txtPagoMensual.Text = pagoMensual.Value.ToString("F2", CultureInfo.InvariantCulture);
    }

    private async void OnVerResumenClicked(object sender, EventArgs e)
    {
        string nombre = txtNombre.Text?.Trim() ?? string.Empty;
        string apellido = txtApellido.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(nombre))
        {
            await DisplayAlert("Aviso", "Ingrese el nombre", "OK");
            return;
        }
        if (string.IsNullOrEmpty(apellido))
        {
            await DisplayAlert("Aviso", "Ingrese el apellido", "OK");
            return;
        }

        int? edad = await ObtenerEdadValidaAsync();
        if (edad is null)
        {
            return;
        }

        if (pkPlan.SelectedIndex < 0)
        {
            await DisplayAlert("Aviso", "Seleccione un plan", "OK");
            return;
        }

        decimal? montoInicial = await ObtenerMontoInicialValidoAsync();
        if (montoInicial is null)
        {
            return;
        }

        if (pagoMensual is null)
        {
            await DisplayAlert("Aviso", "Primero presione 'Calcular Pago mensual'", "OK");
            return;
        }

        var registro = new RegistroGimnasio
        {
            Nombre = nombre,
            Apellido = apellido,
            Edad = edad.Value,
            FechaInicio = dpFechaInicio.Date,
            Plan = ReglasNegocio.Planes[pkPlan.SelectedIndex],
            Casillero = swCasillero.IsToggled,
            MontoInicial = montoInicial.Value,
            PagoMensual = pagoMensual.Value
        };

        await Navigation.PushAsync(new vResumen(usuario, registro));
    }

    private async Task<int?> ObtenerEdadValidaAsync()
    {
        string texto = txtEdad.Text?.Trim() ?? string.Empty;

        // Solo dígitos (sin signo, punto ni coma)
        if (!int.TryParse(texto, NumberStyles.None, CultureInfo.InvariantCulture, out int edad))
        {
            await DisplayAlert("Aviso", "La edad debe contener solo números", "OK");
            return null;
        }
        if (edad < ReglasNegocio.EdadMinima)
        {
            await DisplayAlert("Aviso", $"La edad debe ser mayor o igual a {ReglasNegocio.EdadMinima} años", "OK");
            return null;
        }
        return edad;
    }

    private async Task<decimal?> ObtenerMontoInicialValidoAsync()
    {
        // Acepta decimales con punto o coma
        string texto = (txtMontoInicial.Text?.Trim() ?? string.Empty).Replace(',', '.');

        if (!decimal.TryParse(texto, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal monto))
        {
            await DisplayAlert("Aviso", "Ingrese un monto inicial numérico", "OK");
            return null;
        }
        if (!ReglasNegocio.MontoInicialValido(monto))
        {
            await DisplayAlert("Aviso",
                $"El monto inicial debe estar entre {ReglasNegocio.MontoInicialMinimo:F0} y {ReglasNegocio.MontoInicialMaximo:F0} " +
                "(10% y 50% del costo anual de 1200)", "OK");
            return null;
        }
        return monto;
    }
}
}
