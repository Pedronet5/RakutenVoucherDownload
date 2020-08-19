using Microsoft.Extensions.Configuration;
using System.IO;

namespace RakutenVoucherDownload.Infra.CrossCutting.IoC
{
    public class Configuracoes
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Scoped { get; set; }
        public string UrlToken { get; set; }
        public string Authorization { get; set; }

        public Configuracoes()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


            UrlToken = config.GetSection("TokenRakuten:UrlToken").Value;
            Authorization = config.GetSection("TokenRakuten:Authorization").Value;
            Username = config.GetSection("TokenRakuten:Username").Value;
            Password = config.GetSection("TokenRakuten:Password").Value;
            Scoped = config.GetSection("TokenRakuten:Scoped").Value;


        }
    }


}
