using RakutenVoucherDownload.Enums;
using RakutenVoucherDownload.Infra.CrossCutting.IoC;
using RakutenVoucherDownload.Services.Rakuten;
using System;
using System.Runtime.InteropServices;

namespace RakutenVoucherDownload
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            #region Obter parâmetros
            string[] linhaComando = Environment.GetCommandLineArgs();
            string[] parametros = linhaComando[1].Split("|");
            string nomeServico = parametros[0];
            #endregion

            #region Executar serviço
            EServicos eServico = (EServicos)Enum.Parse(typeof(EServicos), nomeServico);

            var configuracoes = new Configuracoes();

            BaixarXMLVouchers baixarXMLVouchers = new BaixarXMLVouchers(configuracoes);

            switch (eServico)
            {
                case EServicos.Nenhum:
                    break;
                case EServicos.BaixarXMLVoucher:
                    baixarXMLVouchers.Executar();
                    break;
                default:
                    break;
            }
            #endregion
        }
    }
}
