using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoletoBradescoPGE.Model
{

    public class Pedido
    {
        public string numero { get; set; }
        public int valor { get; set; }
        public string descricao { get; set; }
    }

    public class Endereco
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
    }

    public class Comprador
    {
        public string nome { get; set; }
        public string documento { get; set; }
        public Endereco endereco { get; set; }
    }

    public class Boleto
    {
        public string beneficiario { get; set; }
        public string carteira { get; set; }
        public string nosso_numero { get; set; }
        public string numero_documento { get; set; }
        public string data_emissao { get; set; }
        public string data_vencimento { get; set; }
        public int valor_titulo { get; set; }
        public string url_logotipo { get; set; }
        public string mensagem_cabecalho { get; set; }
        public string tipo_renderizacao { get; set; }
        public BoletoInstrucoes instrucoes { get; set; }
    }

    public class BoletoInstrucoes
    {
        public string instrucao_linha_1 { get; set; }
        public string instrucao_linha_2 { get; set; }

    }

    public class DadosEntrada
    {
        public string merchant_id { get; set; }
        public string meio_pagamento { get; set; }
        public Pedido pedido { get; set; }
        public Comprador comprador { get; set; }
        public Boleto boleto { get; set; }
       
    }

}
