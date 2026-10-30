namespace PUchuariExamen.Views;

public partial class vLogin : ContentPage
{
    private readonly string[,] usuarios = new string[4, 2]
           {
            { "recepcion",  "gym2026"   },
            { "entrenador", "fit_01"    },
            { "uisrael",    "2026"      },
            { "gerente",    "admin123"  }
           };

    public vLogin()
    {
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
        string contrasena = txtContrasena.Text ?? string.Empty;

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
        {
            await DisplayAlert("Aviso", "Ingrese usuario y contraseña", "OK");
            return;
        }

        if (!CredencialesValidas(usuario, contrasena))
        {
            await DisplayAlert("Error", "Dato incorrecto", "OK");
            return;
        }

        txtContrasena.Text = string.Empty;
        await Navigation.PushAsync(new vRegistro(usuario));
    }

    private async void OnAcercaDeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }

    private bool CredencialesValidas(string usuario, string contrasena)
    {
        for (int i = 0; i < usuarios.GetLength(0); i++)
        {
            if (usuarios[i, 0] == usuario && usuarios[i, 1] == contrasena)
            {
                return true;
            }
        }
        return false;
    }
}
