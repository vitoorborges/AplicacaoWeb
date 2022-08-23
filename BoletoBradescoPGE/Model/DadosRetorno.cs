using System;

namespace BoletoBradescoPGE.Model
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PedidoRetorno
    {
        public string numero { get; set; }
        public int valor { get; set; }
        public string descricao { get; set; }
    }

    public class BoletoRetorno
    {
        public int valor_titulo { get; set; }
        public string data_geracao { get; set; }
        public string data_atualizacao { get; set; }
        public string linha_digitavel { get; set; }
        public string linha_digitavel_formatada { get; set; }
        public string token { get; set; }
        public string url_acesso { get; set; }
    }

    public class Status
    {
        public int codigo { get; set; }
        public string mensagem { get; set; }
    }

    public class DadosRetorno
    {
        public string merchant_id { get; set; }
        public string meio_pagamento { get; set; }
        public PedidoRetorno pedido { get; set; }
        public BoletoRetorno boleto { get; set; }
        public Status status { get; set; }
    }




}
