using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PedidosApp.Interfaces;

namespace PedidosApp.Services
{
    public class AccessService : IAccessService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccessService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendEmailRecover(string emailTo, string usuario)
        {
            try
            {
                var pedidosAppiClient = _httpClientFactory.CreateClient("PedidosAppiClient");
                pedidosAppiClient.DefaultRequestHeaders.Accept.Clear();
                pedidosAppiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers
                    .MediaTypeWithQualityHeaderValue("application/json"));

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(emailTo), "EmailTo");
                    formData.Add(new StringContent(usuario), "Usuario");

                    var response = await pedidosAppiClient.PostAsync("api/sendEmail/SendEmailRecover", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        return "Código enviado correctamente, por favor revise su correo.";
                    }
                    else
                    {
                        return "Error al enviar el código " + response.ReasonPhrase;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al enviar el código: " + ex.Message;
            }

            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> ValidateUser(string usuario, string codigoRecuperacion)
        {
            try
            {
                var pedidosAppiClient = _httpClientFactory.CreateClient("PedidosAppiClient");
                pedidosAppiClient.DefaultRequestHeaders.Accept.Clear();
                pedidosAppiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers
                    .MediaTypeWithQualityHeaderValue("application/json"));

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(usuario), "user");
                    formData.Add(new StringContent(codigoRecuperacion), "recoveryCode");

                    var response = await pedidosAppiClient.PostAsync("api/sendEmail/ValidateUser", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                        return result;
                    }
                    else
                    {
                        return new Dictionary<string, object> { { "success", false }, { "message", $"Error al validar el usuario." } };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "suceess", false }, { "message", $"Error: {ex.Message}" } };
            }
        }

    }
}
