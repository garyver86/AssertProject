using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Assert.Infrastructure.Utils
{
    public class JsonMgr
    {
        public static string ConvertToJson(object objeto)
        {
            var configuracion = new JsonSerializerSettings
            {
                // Ignorar propiedades con valores null
                NullValueHandling = NullValueHandling.Ignore,

                // Evitar referencias cíclicas
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Formatear la salida para que sea legible (opcional)
                Formatting = Formatting.Indented,

                // Usar camelCase para los nombres de las propiedades (opcional)
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // Serializar el objeto a JSON
            return JsonConvert.SerializeObject(objeto, configuracion);
        }
        public static string ConvertToJson(object objeto, int length)
        {
            var configuracion = new JsonSerializerSettings
            {
                // Ignorar propiedades con valores null
                NullValueHandling = NullValueHandling.Ignore,

                // Evitar referencias cíclicas
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Formatear la salida para que sea legible (opcional)
                Formatting = Formatting.Indented,

                // Usar camelCase para los nombres de las propiedades (opcional)
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // Serializar el objeto a JSON
            string result = JsonConvert.SerializeObject(objeto, configuracion);
            if (result.Length > length)
            {
                result = result.Substring(0, length);
            }
            return result;
        }
    }
}
