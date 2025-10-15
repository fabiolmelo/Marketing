using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Marketing.Domain.Extensoes
{
    public static class StringExtensao
    {
        public static string PriMaiuscula(this String texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }
        
        public static string RemoverCaracteresEspeciais(this string texto)
        {
            try
            {
                return Regex.Replace(texto, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
    }
}