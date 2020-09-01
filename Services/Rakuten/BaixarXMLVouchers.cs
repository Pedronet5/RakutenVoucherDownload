using Newtonsoft.Json;
using RakutenVoucherDownload.Comunication;
using RakutenVoucherDownload.Infra.CrossCutting.IoC;
using RestSharp;
using System;
using System.IO;
using System.Text;
using System.Xml;

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
            try
            {
                Token = this.GetAccessTokenAsync();

                if (Token != null)
                {
                    this.LimparDiretorio();

                    var numeroPaginas = GetRequestedVoucher(1);

                    if (numeroPaginas > 1)
                    {
                        for (int i = 1; i < numeroPaginas; i++)
                        {
                            this.GetRequestedVoucher(i + 1);
                        }
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception($"Não foi possível obter o token! {ex.Message}");
            }
        }

        public int GetRequestedVoucher(int numeroPagina)
        {
            try
            {
                var client = new RestClient(this.UrlCoupon + "?pagenumber=" + numeroPagina);
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + Token.AccessToken);
                IRestResponse response = client.Execute(request);

                string caminho;
                caminho = this.CaminhoXML + "\\Voucher" + numeroPagina + ".xml";

                bool exists = Directory.Exists(this.CaminhoXML);

                if (!exists)
                    Directory.CreateDirectory(this.CaminhoXML);

                File.WriteAllBytes(caminho, Encoding.ASCII.GetBytes(response.Content));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);
                doc.Save(caminho);

                if (response.Content.ToString().Contains("PageNumberRequested"))
                {
                    var posicao = response.Content.ToString().IndexOf("TotalPages>") + 11;
                    numeroPagina = Convert.ToInt32(response.Content.ToString().Substring(posicao, 1));
                }

                return numeroPagina;
            }
            catch(Exception ex)
            {
                throw new Exception($"Ocorreu um erro no request do voucher.xml! {ex.Message}");
            }
        }

        public void LimparDiretorio()
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(this.CaminhoXML);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar limpar o diretorio! {ex.Message}");
            }
        }


    }

}
