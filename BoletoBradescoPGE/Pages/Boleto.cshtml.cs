using BoletoBradescoPGE.Model;
using BoletoBradescoPGE.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace BoletoBradescoPGE.Pages
{
    public class BoletoModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBoletoRepository _boletoRepository;
        private readonly IConfiguration _configuration;
        [BindProperty(SupportsGet = true)]
        [Required]
        public string IdCandidato { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public string Token { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public string Evento { get; set; }

        public string Ambiente()
        {

            if (_configuration.GetSection("Ambiente").Value == "H")
            {
                return _configuration.GetSection("UrlAcesso").GetSection("UrlHomologacao").Value.ToString();
            }
            return _configuration.GetSection("UrlAcesso").GetSection("UrlProducao").Value.ToString();
        }

        public BoletoModel(ILogger<IndexModel> logger, IBoletoRepository boletoRepository, IConfiguration configuration)
        {
            _logger = logger;
            _boletoRepository = boletoRepository;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Log.Information(Evento + " Requisição feita com os dados " + IdCandidato + " Token: " + Token);
                var dadosCandidato = await _boletoRepository.PesquisarCandidato(IdCandidato, Token, Evento);

                if (dadosCandidato.CaminhoBoleto != null)
                {
                    Log.Information(Evento + "Boleto encontrado para o candidato: " + IdCandidato + " Token: " + Token);
                    return Redirect(dadosCandidato.CaminhoBoleto);
                }
                if (dadosCandidato != null)
                {
                    Log.Information(Evento + "Dados encontrados para o candidato: " + IdCandidato + " Token: " + Token + " e gerando o boleto");


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Ambiente());
                    request.Timeout = -1;
                    request.Method = "POST";
                    request.Headers.Add("Authorization", _configuration.GetSection("Authorization").Value);
                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Content-Type", "application/json");
                    request.Headers.Add("Cookie", "JSESSIONID=0000DLwgDVacPruoVP5oRSkyoCS:-1");
                    DadosEntrada body = new DadosEntrada()
                    {

                        merchant_id = dadosCandidato.Merchant_ID,
                        meio_pagamento = dadosCandidato.MeioPagamento,
                        pedido = new Pedido()
                        {
                            numero = dadosCandidato.NossoNumero,
                            valor = Convert.ToInt32(dadosCandidato.Valor.Replace(".", "")),
                            descricao = dadosCandidato.Descricao
                        },
                        comprador = new Comprador()
                        {
                            nome = dadosCandidato.NomeCandidato,
                            documento = dadosCandidato.cpf,
                            endereco = new Endereco()
                            {
                                cep = dadosCandidato.CEP,
                                logradouro = dadosCandidato.Logradouro,
                                numero = dadosCandidato.Numero,
                                complemento = "",
                                bairro = dadosCandidato.Bairro,
                                cidade = dadosCandidato.Cidade,
                                uf = dadosCandidato.UFResidencia

                            }
                        },
                        boleto = new Boleto()
                        {
                            beneficiario = dadosCandidato.Beneficiario,
                            carteira = dadosCandidato.Carteira,
                            nosso_numero = dadosCandidato.NossoNumero,
                            numero_documento = dadosCandidato.NossoNumero,
                            data_emissao = dadosCandidato.data_emissao.ToString("yyyy-MM-dd"),
                            data_vencimento = dadosCandidato.data_vencimento.ToString("yyyy-MM-dd"),
                            valor_titulo = Convert.ToInt32(dadosCandidato.Valor.Replace(".", "")),
                            url_logotipo = "https://cdn.cebraspe.org.br/wp-content/uploads/2018/12/logo-cebraspe.png",
                            mensagem_cabecalho = dadosCandidato.MensagemCabecalho,
                            tipo_renderizacao = dadosCandidato.TipoRenderizacao,
                            instrucoes = new BoletoInstrucoes()
                            {
                                instrucao_linha_1 = "Inscrição: " + dadosCandidato.Inscricao,
                                instrucao_linha_2 = dadosCandidato.cargo
                            }


                        }

                    };
                    var result = "";
                    JsonConversao jsonconv = new JsonConversao();
                    var json = jsonconv.ConverteObjectParaJSon<DadosEntrada>(body);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (Stream strm = response.GetResponseStream())

                    {

                        result = new StreamReader(strm).ReadToEnd();

                    }

                    var dadosWebService = jsonconv.ConverteJSonParaObject<DadosRetorno>(result);

                    if (dadosWebService.status.codigo == 0)
                    {
                        Log.Information(Evento + "Boleto gerado com sucesso para o candidato: " + IdCandidato + " Token: " + Token);
                        _boletoRepository.AtualizaBoletoCandidato(dadosCandidato.NossoNumero, dadosWebService.boleto.url_acesso, Evento);
                        return Redirect(dadosWebService.boleto.url_acesso.ToString());

                    }
                    Log.Information(Evento + "Erro ao gerar o boleto para o candidato: " + IdCandidato + " Token: " + Token + " Erro:" + dadosWebService.status.codigo + " " + dadosWebService.status.mensagem);


                    return RedirectToPage("error");


                }
                Log.Information(Evento + "Não encontrado dados para o candidato: " + IdCandidato + " Token: " + Token);
                return RedirectToPage("error");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return RedirectToPage("error");

            }
        }

    }
}
