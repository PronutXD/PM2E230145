using System.Net;

namespace PM2E230145;

public partial class MainPage : ContentPage
{
    private int intentosFallidos = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void MostrarContrasena_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        txtContrasena.IsPassword = !e.Value;
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        string correo = txtCorreo.Text?.Trim() ?? "";
        string clave = txtContrasena.Text ?? "";

        if (correo == "" || clave == "")
        {
            lblError.Text = "Ingresa tu correo y contraseña.";
            lblError.IsVisible = true;
            return;
        }

        btnLogin.IsEnabled = false;
        indicadorCarga.IsVisible = true;
        indicadorCarga.IsRunning = true;
        lblError.IsVisible = false;

        try
        {
            HttpStatusCode resultado = await ApiService.LoginAsync(correo, clave);

            if (resultado == HttpStatusCode.OK)
            {
                intentosFallidos = 0;
                await Navigation.PushAsync(new Profile());
            }
            else if (resultado == HttpStatusCode.Unauthorized)
            {
                intentosFallidos++;
                lblError.Text = "Correo o contraseña incorrectos.";
                lblError.IsVisible = true;

                if (intentosFallidos >= 3)
                {
                    btnLogin.IsEnabled = false;
                    await DisplayAlert("Acceso bloqueado", "Alcanzaste los tres intentos permitidos.", "Aceptar");
                    return;
                }
            }
            else
            {
                lblError.Text = "La API respondió con un error: " + resultado;
                lblError.IsVisible = true;
            }
        }
        catch (HttpRequestException)
        {
            lblError.Text = "No se pudo conectar con la API. Comprueba que esté ejecutándose.";
            lblError.IsVisible = true;
        }
        catch (TaskCanceledException)
        {
            lblError.Text = "La conexión tardó demasiado.";
            lblError.IsVisible = true;
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.IsVisible = true;
        }
        finally
        {
            indicadorCarga.IsRunning = false;
            indicadorCarga.IsVisible = false;

            if (intentosFallidos < 3)
                btnLogin.IsEnabled = true;
        }
    }
}
