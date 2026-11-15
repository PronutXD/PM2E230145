namespace PM2E230145;

public partial class Profile : ContentPage
{
	public Profile()
	{
		InitializeComponent();
	}

	private void SwitchDisponibilidad_Toggled(object sender, ToggledEventArgs e)
    {
        
    }

	public void SliderExperiencia_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lblExperiencia.Text = $"{e.NewValue} Años de experiencia";

        if (e.NewValue <= 2)
        {
            lblNivel.Text = "Nivel: Inicial";
            lblNivel.TextColor = Color.FromArgb("#DCFCE7");
        }
        else if (e.NewValue >= 3 && e.NewValue <= 7)
        {
            lblNivel.Text = "Nivel: Intermedio";
            lblNivel.TextColor = Color.FromArgb("#E0F2FE");

        }
        else if (e.NewValue >= 8)
        {
            lblNivel.Text = "Nivel: Avanzado";
            lblNivel.TextColor = Color.FromArgb("#FEF3C7");
        }
     }

    public void BtnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }
}