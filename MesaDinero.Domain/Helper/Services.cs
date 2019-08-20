using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain
{
    public  class QuertiumPersona
    {
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
    }

    public class QuertiunEmpresa
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }

    }


    public static class QuertiumServices
    {
        private static string GetToken()
        {
            return ConfigurationManager.AppSettings["token_quertium"];
        }

        public static async Task<PersonaNatutalRequest> GetDatosPersonaNatural(string dni)
        {
            QuertiumPersona qPersona = new QuertiumPersona();
            PersonaNatutalRequest persona = new PersonaNatutalRequest();

            try
            {
                string token = GetToken();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var result = await client.GetAsync("http://quertium.com/api//v1/Reniec/dni/" + dni);
                    result.EnsureSuccessStatusCode();
                    var jsonString = await result.Content.ReadAsStringAsync();

                    qPersona = Newtonsoft.Json.JsonConvert.DeserializeObject<QuertiumPersona>(jsonString);
                }

                if(qPersona != null)
                {
                    persona.tipoDocumento = "DNI";
                    persona.nombres = string.Format("{0} {1}", qPersona.PrimerNombre, qPersona.SegundoNombre);
                    persona.apePaterno = qPersona.ApellidoPaterno;
                    persona.apeMaterno = qPersona.ApellidoMaterno;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return persona;
        }

        public static async Task<PersonaJuridicaReuest> getDatosPersonaJuridica(string ruc)
        {
            QuertiunEmpresa qEmpresa = new QuertiunEmpresa();
            PersonaJuridicaReuest empresa = new PersonaJuridicaReuest();

            try
            {
                string token = GetToken();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var result = await client.GetAsync("http://quertium.com/api//v1/sunat/ruc/" + ruc);
                    if(result.StatusCode ==  System.Net.HttpStatusCode.InternalServerError)                    {
                        empresa.nombre = qEmpresa.Nombre;
                        empresa.direccion = qEmpresa.Direccion;

                        return empresa;
                    }
                    result.EnsureSuccessStatusCode();
                    var jsonString = await result.Content.ReadAsStringAsync();

                    qEmpresa = Newtonsoft.Json.JsonConvert.DeserializeObject<QuertiunEmpresa>(jsonString);
                }

                if(qEmpresa != null)
                {
                    empresa.ruc = "RUC";
                    empresa.nombre = qEmpresa.Nombre;
                    empresa.direccion = qEmpresa.Direccion;
                }

            }
            catch (Exception ex)
            {
                   throw new Exception(ex.Message);             
            }

            return empresa;
        }

    }
}
