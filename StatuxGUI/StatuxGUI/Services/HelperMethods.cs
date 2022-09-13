using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace StatuxGUI.Services
{
    public static class HelperMethods
    {
        private static string IPAddress = AppSettingsManager.Settings["main_ip"] + ':' + AppSettingsManager.Settings["main_port"];
        private static string BaseAddress = "https://" + IPAddress + "/api/main/";
        public static HttpClient CreateHttpClient()
        {
            HttpClient client;
#if DEBUG
            client = new HttpClient(GetInsecureHandler());
#else
            client = new HttpClient();
#endif
            client.Timeout = TimeSpan.FromSeconds(5);
            client.BaseAddress = new Uri(BaseAddress);

            //TODO: DNS checkup

            return client;
        }

        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation
            };
            return handler;
        }

        private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            Debug.WriteLine("Certificate: ", certificate.Issuer.ToString());

            if (certificate.Issuer.ToString().Contains("CN=Filip"))
                return true;
            return errors == SslPolicyErrors.None;
        }
    }
}
