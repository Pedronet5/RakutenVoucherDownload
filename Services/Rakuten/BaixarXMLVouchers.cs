using Newtonsoft.Json;
using RakutenVoucherDownload.Comunication;
using RakutenVoucherDownload.Infra.CrossCutting.IoC;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Serialization.Xml;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RakutenVoucherDownload.Services.Rakuten
{
    public class BaixarXMLVouchers
    {
        public string UrlToken { get; private set; }
        public string UrlCoupon { get; private set; }
        public string CaminhoXML { get; private set; }
        public string Authorization { get; private set; }
        private Token Token { get; set; }

        public BaixarXMLVouchers(Configuracoes configuracoes)
        {
            UrlToken = configuracoes.UrlToken;
            UrlCoupon = configuracoes.UrlCoupon;
            CaminhoXML = configuracoes.CaminhoXML;
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

                    token = JsonConvert.DeserializeObject<Token>(response.Content);
                }

                return token;
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível obter o token! {ex.Message}");
            }

        }

        public bool ProcessarBaixaXML()
        {
            Token = this.GetAccessTokenAsync();

            if (Token != null)
            {
                var client = new RestClient(this.UrlCoupon);
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + Token.AccessToken);
                IRestResponse response = client.Execute(request);

                var caminho = this.CaminhoXML + "\\Voucher.xml";

                bool exists = Directory.Exists(this.CaminhoXML);

                if (!exists)
                    Directory.CreateDirectory(this.CaminhoXML);

                File.WriteAllBytes(caminho, Encoding.ASCII.GetBytes(response.Content));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);
                doc.Save(caminho);

            }

            return true;
        }
    }

}
