using Microsoft.Extensions.Configuration;
using System.IO;

namespace RakutenVoucherDownload.Infra.CrossCutting.IoC
{
    public class Configuracoes
    {
        public string UrlToken { get; set; }
        public string UrlCoupon { get; set; }
        public string CaminhoXML { get; set; }
        public string Authorization { get; set; }

        public Configuracoes()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


            UrlToken = config.GetSection("TokenRakuten:UrlToken").Value;
            Authorization = config.GetSection("TokenRakuten:Authorization").Value;
            UrlCoupon = config.GetSection("TokenRakuten:UrlCoupon").Value;
            CaminhoXML = config.GetSection("TokenRakuten:CaminhoXML").Value;


        }
    }


}
