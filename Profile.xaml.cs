namespace PM2E230145;

public partial class Profile : ContentPage
{
    public Profile()
    {
        InitializeComponent();
        CargarPerfil();
    }

    private async void CargarPerfil()
    {
        try
        {
            Perfil perfil = await ApiService.ObtenerPerfilAsync();

            lblNombre.Text = perfil.Nombre;
            switchDisponibilidad.IsToggled = perfil.Disponible;
            sliderExperiencia.Value = Math.Clamp(perfil.AniosExperiencia, 0, 15);
            ActualizarNivel(sliderExperiencia.Value);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cargar el perfil: " + ex.Message, "Aceptar");
        }
    }

    private void SwitchDisponibilidad_Toggled(object sender, ToggledEventArgs e)
    {
        // El valor se envía cuando se presiona el botón Guardar.
    }

    private void SliderExperiencia_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ActualizarNivel(e.NewValue);
    }

    private void ActualizarNivel(double experiencia)
    {
        int anios = (int)Math.Round(experiencia);
        lblExperiencia.Text = anios + " años de experiencia";

        if (anios <= 2)
        {
            lblNivel.Text = "Nivel: Inicial";
            borderPerfil.BackgroundColor = Color.FromArgb("#DCFCE7");
        }
        else if (anios <= 7)
        {
            lblNivel.Text = "Nivel: Intermedio";
            borderPerfil.BackgroundColor = Color.FromArgb("#E0F2FE");
        }
        else
        {
            lblNivel.Text = "Nivel: Avanzado";
            borderPerfil.BackgroundColor = Color.FromArgb("#FEF3C7");
        }
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        btnGuardar.IsEnabled = false;

        try
        {
            Perfil perfil = new Perfil
            {
                Nombre = lblNombre.Text ?? "",
                Disponible = switchDisponibilidad.IsToggled,
                AniosExperiencia = (int)Math.Round(sliderExperiencia.Value)
            };

            await ApiService.GuardarPerfilAsync(perfil);
            await DisplayAlert("Aviso", "La solicitud fue enviada a la API.", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron enviar los cambios: " + ex.Message, "Aceptar");
        }
        finally
        {
            btnGuardar.IsEnabled = true;
        }
    }

    private async void BtnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        ApiService.CerrarSesion();
        await Navigation.PopToRootAsync();
    }
}
