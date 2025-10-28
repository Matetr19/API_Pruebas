using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.IO.Font;
using APIPoliza.Models;
using APIPoliza.Data;


namespace APIPoliza.Controllers
{
     /// <summary>
    /// Servicio que permite consultar información de MPM (Emulacion)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly APIPolizaContext _context;
        public ClientController(APIPolizaContext context) { _context = context; }


        /// <param name="numCedula">Número de cédula del cliente</param>
        /// <summary>
        /// Metodo que nos ayuda a obtener clientes
        /// </summary>
        /// <remarks>
        /// Obtiene la información de 1 Cliente con 1 Dirección Principal Relacionada.
        /// Se utiliza el campo Cédula de Ciudadanía.
        /// 
        /// **CLI_Sexo - Género Cliente**
        /// * 1 = Hombre  
        /// * 2 = Mujer  
        /// * Otro = Otro
        ///
        /// **VCO_ID - Vía de Comunicación**
        /// * 1 = No Autorizado  
        /// * 2 = Canales de gestión telefónica  
        /// * 3 = Canales telefónicos y mensajería  
        /// * 4 = Telefónicos, mensajería y correo electrónico  
        /// * 5 = Telefónicos, mensajería, correo electrónico y visita  
        ///
        /// **TPC_ID - Tipo Cliente**
        /// * 1 = Normal  
        /// * 3 = VIP
        /// </remarks>
        /// <response code="200">Retorna la información del cliente</response>
        /// <response code="404">Si no se encuentra el cliente</response>

        [HttpGet("{numCedula}")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient(long? numCedula)
        {
            var query = _context.vw_endpoint_cliente.AsQueryable();
            if (numCedula.HasValue)
                query = query.Where(c => c.clI_NIF == numCedula);

            var result = await query.ToListAsync();
            return Ok(result);
        }
        // =======================
        /***
        [HttpGet("pdf/{numCedula}/polizaPDF")]
        public async Task<IActionResult> GetClientPdfFromTemplate(int numCedula)
        {
            // 1. OBTENER LOS DATOS
            var clientData = await _context.vw_endpoint_cliente.Where(c => c.clI_NIF == numCedula).FirstOrDefaultAsync();
            
            if (clientData == null) 
                return NotFound($"Cliente con cédula {numCedula} no encontrado.");
            
            // 2. VERIFICAR PLANTILLA
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "template.pdf");
            if (!System.IO.File.Exists(templatePath)) 
                return StatusCode(500, "Error: Plantilla PDF no encontrada.");
            
            var stream = new MemoryStream();
            
            try
            {
                using (var templateStream = new MemoryStream(System.IO.File.ReadAllBytes(templatePath)))
                using (var reader = new PdfReader(templateStream))
                {
                    var writer = new PdfWriter(stream);
                    writer.SetCloseStream(false);
                    
                    using (var pdfDoc = new PdfDocument(reader, writer))
                    {
                        var form = PdfAcroForm.GetAcroForm(pdfDoc, true);


                        if (form == null)
                        {
                            Console.WriteLine("❌ El PDF no tiene campos de formulario");
                            return StatusCode(500, "El PDF no tiene campos de formulario");
                        }
                        var fields = form.GetFormFields();
                        // IMPRIME TODOS LOS CAMPOS QUE ENCUENTRA
                Console.WriteLine($"Total de campos: {fields.Count}");
                foreach (var field in fields)
                {
                    Console.WriteLine($"Campo encontrado: '{field.Key}'");
                }
                var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                // LLENA LOS CAMPOS
                if (fields.ContainsKey("Text1"))
                {
                    Console.WriteLine("✅ Llenando Text1");
                    fields["Text1"].SetValue($"{clientData.clI_Nombre} {clientData.clI_Apellido1}");
                }
                else
                {
                    Console.WriteLine("❌ Text1 NO existe");
                }
                
                if (fields.ContainsKey("Text2"))
                {
                    Console.WriteLine("✅ Llenando Text2");
                    fields["Text2"].SetValue(clientData.clI_NIF.ToString());
                }
                else
                {
                    Console.WriteLine("❌ Text2 NO existe");
                }
                
                if (fields.ContainsKey("Text3"))
                {
                    Console.WriteLine("✅ Llenando Text3");
                    fields["Text3"].SetValue(clientData.clI_Nacimiento.ToShortDateString());
                }
                else
                {
                    Console.WriteLine("❌ Text3 NO existe");
                }
                
                form.FlattenFields();
                    }
                }
                
                stream.Position = 0;
                return File(stream, "application/pdf", $"Reporte_Cliente_{numCedula}.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                stream.Dispose();
                throw;
            }
        }
    */    
    }
}