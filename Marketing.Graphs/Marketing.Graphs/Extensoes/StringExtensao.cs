using System;

namespace Marketing.Domain.Extensoes
{
    public static class StringExtensao
    {
        public static string PriMaiuscula(this String texto){
            if (string.IsNullOrEmpty(texto))
                return texto;
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }
    }
}