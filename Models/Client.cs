using System.Numerics;

namespace APIPoliza.Models
{
    public class Client
    {
        public int? clI_Id { get; set; }
        public string? clI_Nombre {get; set;}
        public string? clI_Apellido1 {get; set;}
        public string? clI_Apellido2 {get; set;}
        public long? clI_NIF {get; set;}
        public DateTime cli_FechaExp {get; set;}
        public string? cli_Tipodoc {get; set;}
        public string? cli_EstadoCivil {get; set;}
        public int? clI_Sexo {get; set;}
        public string? clI_Genero {get; set;}
        public DateTime clI_Nacimiento {get; set;}
        public double? cli_Edad {get; set;}
        public int? vcO_ID {get; set;}
        public string? vcO_LiteralOpcion {get; set;}
        public int? tpC_Id {get; set;}
        public string? tpC_Nombre {get; set;}
        public int? dir_id {get; set;}
    }
}