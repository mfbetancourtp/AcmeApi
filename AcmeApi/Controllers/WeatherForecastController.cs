using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AcmeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        [HttpPost("enviarPedido")]
        public IActionResult EnviarPedido([FromBody] PedidoJson request)
        {

            var xmlRequest = ConvertJsonToXml(request);


            var xmlResponse = CallSoapService(xmlRequest);


            var jsonResponse = ConvertXmlToJson(xmlResponse);

            return Ok(jsonResponse);
        }

        private string ConvertJsonToXml(PedidoJson request)
        {
            var xml = new XElement("soapenv:Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "env", "http://WSDLs/EnvioPedidos/EnvioPedidosAcme"),
                new XElement("soapenv:Header"),
                new XElement("soapenv:Body",
                    new XElement("env:EnvioPedidoAcme",
                        new XElement("EnvioPedidoRequest",
                            new XElement("pedido", request.numPedido),
                            new XElement("Cantidad", request.cantidadPedido),
                            new XElement("EAN", request.codigoEAN),
                            new XElement("Producto", request.nombreProducto),
                            new XElement("Cedula", request.numDocumento),
                            new XElement("Direccion", request.direccion)
                        )
                    )
                )
            );
            return xml.ToString();
        }

        private string CallSoapService(string xmlRequest)
        {


            string xmlResponse = "";
            return xmlResponse;
        }

        private PedidoResponseJson ConvertXmlToJson(string xmlResponse)
        {
            var doc = XDocument.Parse(xmlResponse);
            var response = new PedidoResponseJson
            {
                codigoEnvio = doc.Descendants("Codigo").FirstOrDefault()?.Value,
                estado = doc.Descendants("Mensaje").FirstOrDefault()?.Value
            };
            return response;
        }
    }

    public class PedidoJson
    {
        public string? numPedido { get; set; }
        public string? cantidadPedido { get; set; }
        public string? codigoEAN { get; set; }
        public string? nombreProducto { get; set; }
        public string? numDocumento { get; set; }
        public string? direccion { get; set; }
    }

    public class PedidoResponseJson
    {
        public string? codigoEnvio { get; set; }
        public string? estado { get; set; }
    }
}
