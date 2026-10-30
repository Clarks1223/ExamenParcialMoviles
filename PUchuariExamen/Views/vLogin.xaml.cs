namespace PUchuariExamen.Views;

public partial class vLogin : ContentPage
{
    private string[,] usuarios = new string[4, 2]
    {
            { "recepcion",  "gym2026"   },
            { "entrenador", "fit_01"    },
            { "uisrael",    "2026"      },
            { "gerente",    "admin1234" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(object sender, EventArgs e)
    {
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