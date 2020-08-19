using Newtonsoft.Json;
using RakutenVoucherDownload.AppService;
using RakutenVoucherDownload.Comunication;
using RakutenVoucherDownload.Infra.CrossCutting.IoC;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RakutenVoucherDownload.Services.Rakuten
{
    public class BaixarXMLVouchers
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string UrlToken { get; private set; }
        public string Authorization { get; private set; }
        public string Scoped { get; private set; }
        private Token Token { get; set; }

        private DateTime TokenDate { get; set; }

        public BaixarXMLVouchers(Configuracoes configuracoes)
        {
            Username = configuracoes.Username;
            Password = configuracoes.Password;
            UrlToken = configuracoes.UrlToken;
            Scoped = configuracoes.Scoped;
            Authorization = configuracoes.Authorization;
        }

        public bool Executar()
        {
            try
            {
                var processar = ProcessarBaixaXML();

                return processar;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Token GetAccessTokenAsync()
        {
            try
            {
                Token token = new Token();

                if (Token == null)
                {
                    var client = new RestClient(this.UrlToken);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "text/plain");
                    request.AddHeader("authorization", this.Authorization);
                    IRestResponse response = client.Execute(request);

                    TokenDate = DateTime.Now;
                    token = JsonConvert.DeserializeObject<Token>(response.Content);
                }

                return token;
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível obter o token! {ex.Message}");
            }

        }

        private static string EncodeToBase64(string value)
        {
            var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public bool ProcessarBaixaXML()
        {
            Token = this.GetAccessTokenAsync();

            if (Token != null)
            {

            }

            return true;
        }
    }

}
