using Infraestructure.Messaging.Proccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messageging.Process.Functions
{
    public class SendInfoToApiError
    {
        public async Task SendErrorAsync(Error error)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:24469/api/error";

                string json = JsonSerializer.Serialize(error);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error insertado correctamente.");
                }
                else
                {
                    Console.WriteLine($"Error al insertar: {response.StatusCode}");
                }
            }
        }
    }
}
