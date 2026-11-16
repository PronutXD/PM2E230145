using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PM2E230145;

internal static class ApiService
{
#if ANDROID
    private const string DireccionApi = "http://10.0.2.2:5080/";
#else
    private const string DireccionApi = "http://localhost:5080/";
#endif

    private static readonly HttpClient cliente = new HttpClient
    {
        BaseAddress = new Uri(DireccionApi),
        Timeout = TimeSpan.FromSeconds(10)
    };

    private static string token = "";

    public static async Task<HttpStatusCode> LoginAsync(string correo, string clave)
    {
        var datos = new { correo = correo, clave = clave };
        var respuesta = await cliente.PostAsJsonAsync("api/auth/login", datos);

        if (respuesta.IsSuccessStatusCode)
        {
            var resultado = await respuesta.Content.ReadFromJsonAsync<TokenRespuesta>();
            token = resultado?.Token ?? "";
        }

        return respuesta.StatusCode;
    }

    public static async Task<Perfil> ObtenerPerfilAsync()
    {
        var solicitud = new HttpRequestMessage(HttpMethod.Get, "api/perfil");
        solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var respuesta = await cliente.SendAsync(solicitud);
        respuesta.EnsureSuccessStatusCode();

        var perfil = await respuesta.Content.ReadFromJsonAsync<Perfil>();
        return perfil ?? new Perfil();
    }

    public static async Task GuardarPerfilAsync(Perfil perfil)
    {
        var solicitud = new HttpRequestMessage(HttpMethod.Put, "api/perfil");
        solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        solicitud.Content = JsonContent.Create(perfil);

        var respuesta = await cliente.SendAsync(solicitud);
        respuesta.EnsureSuccessStatusCode();
    }

    public static void CerrarSesion()
    {
        token = "";
    }
}

internal class TokenRespuesta
{
    public string Token { get; set; } = "";
}

internal class Perfil
{
    public string Nombre { get; set; } = "";
    public bool Disponible { get; set; }
    public int AniosExperiencia { get; set; }
}
