using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoletoBradescoPGE.Model
{
    public class DadosCandidato
    {
        public string? Merchant_ID { get; set; }

        public string? MeioPagamento {get; set;}
        
        public string? Valor {get; set;}
        
        public string? Descricao {get; set;}
        
        public string? NomeCandidato {get; set;}
        
        public string? cpf {get; set;}
        
        public string? CEP {get; set;}
        
        public string? Logradouro {get; set;}
        
        public string? Numero {get; set;}
        
        public string? Bairro {get; set;}
        
        public string? Cidade {get; set;}
        
        public string? UFResidencia {get; set;}
        
        public DateTime data_vencimento {get; set;}
        
        public DateTime data_emissao {get; set;}
        
        public string? Inscricao {get; set;}
                     
        public string? cargo {get; set;}
                     
        public string? NossoNumero {get; set;}
                     
        public string? Beneficiario {get; set;}
                     
        public string? Carteira {get; set;}
                     
        public string? MensagemCabecalho {get; set;}
                     
        public string? TipoRenderizacao {get; set;}
        public string? CaminhoBoleto {get; set;}
    }
}
