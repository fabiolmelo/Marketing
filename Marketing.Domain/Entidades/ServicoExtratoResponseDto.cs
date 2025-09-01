using System.Net;

namespace Marketing.Domain.Entidades
{
    public class ServicoExtratoResponseDto
    {
        public ServicoExtratoResponseDto(HttpStatusCode httpStatusCode, string response)
        {
            this.httpStatusCode = httpStatusCode;
            Response = response;
        }
        public HttpStatusCode httpStatusCode { get; private set; }
        public bool IsSuccessStatusCode { get { return (int)httpStatusCode >= 200 && (int)httpStatusCode <= 299; }}
        public string Response { get; private set; }
    }
}