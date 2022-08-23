using BoletoBradescoPGE.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace BoletoBradescoPGE.Repository
{
    public interface IBoletoRepository
    {
        public Task<DadosCandidato> PesquisarCandidato(string idCandidato, string token, string evento);
        public void AtualizaBoletoCandidato(string idBoleto, string CaminhoBoleto, string evento);
    }

    public class BoletoRepository : IBoletoRepository
    {
        readonly IConfiguration _configuration;

        public BoletoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DadosCandidato> PesquisarCandidato(string idCandidato, string token, string evento)
        {
            try
            {

                using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value.Replace("EVENTO", evento));

                var p = new DynamicParameters();
                p.Add("@inscricao", idCandidato);
                p.Add("@Token", token);

                var dadosRetorno = await connection.QueryFirstOrDefaultAsync<DadosCandidato>(
                    "Inscricao.uspConsultarDadosBoleto",
                    p,
                    commandType: CommandType.StoredProcedure
                    );

                return dadosRetorno;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
                
            }

        }

        public async void AtualizaBoletoCandidato(string idBoleto, string CaminhoBoleto, string evento)
        {
            try
            {

            using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value.Replace("EVENTO", evento));

            var p = new DynamicParameters();
            p.Add("@idBoleto", Convert.ToInt64(idBoleto));
            p.Add("@link", CaminhoBoleto);

            await connection.QueryAsync(
                "Inscricao.uspAtualizarDadosBoleto",
                p,
                commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
                
            }

        }
    }
}
