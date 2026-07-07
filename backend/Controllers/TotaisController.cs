using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    //Gerar relatório de todas as transações,
    //juntando transações de receita e despesa de todos os residentes da casa.
    [ApiController]
    [Route("api/[controller]")]
    public class TotaisController : ControllerBase
    {
        private readonly AppDbContext _context; //Criação de variável para manipulação do banco de dados.

        public TotaisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //Leitura geral de todas as transações para obtenção de relatório geral.
        public async Task<ActionResult<RelatorioGastos>> GetTotais()
        {
            var relatorio = new RelatorioGastos(); //Instanciando a classe RelatorioGastos.
            var pessoas = await _context.Pessoas.ToListAsync();
            
            foreach (var pessoa in pessoas) //Utilização de loop em foreach, 
                                            //para verificarmos de pessoa em pessoa dentro do banco de dados.
            {
                var transacoesDaPessoa = await _context.Transacoes //Criação da variável de identificação de transações das pessoas
                        .Where(t => t.PessoaId == pessoa.Id) //Que procura na tabela de transações o atributo PessoaId igual ao Id da pessoa que está pegando os dados no momento
                        .ToListAsync(); //Adiciona na Lista as Transações onde o Id da pessoa analisada é o mesmo do atributo PessoaId da transação.

                var totalReceita = transacoesDaPessoa //Variável de Total de receita, utilizando a lista de transações da pessoa, verificado anteriormente
                        .Where(t => t.Tipo == "Receita") //Acessando as transações com o Tipo == "Receita";
                        .Sum(t => t.Valor); //Somando o valor das transações de receita, gerando o total de receita dessa pessoa.

                var totalDespesa = transacoesDaPessoa //Variável de Total de despesa, utilizando a lista de transações da pessoa, verificado anteriormente
                        .Where(t => t.Tipo == "Despesa") //Acessando as transações com o Tipo == "Despesa";
                        .Sum(t => t.Valor); //Somando o valor das transações de despesa, gerando o total de despesa dessa pessoa.

                var saldoLiquido = totalReceita - totalDespesa; //Saldo líquido, gerado por receita - despesa.

                var GastoFinalPessoa = new PessoaTotal(); //Instanciando a classe PessoaTotal, criando um novo objeto.

                GastoFinalPessoa.Nome = pessoa.Nome; //Colocando o nome da pessoa no objeto, sendo o mesmo da pessoa analisada no looping;
                GastoFinalPessoa.Receita = totalReceita; //Colocando o total da receita no objeto;
                GastoFinalPessoa.Despesa = totalDespesa; //Colocando o total da despesa no objeto;
                GastoFinalPessoa.SaldoLiquido = saldoLiquido; //Colocando o saldo líquido no objeto.

                relatorio.PessoasTotais.Add(GastoFinalPessoa); //Adicionando na lista de Pessoas Totais.
            }
            relatorio.TotalGeralReceitas = relatorio.PessoasTotais.Sum(p => p.Receita); //Soma de total de receitas de todas as pessoas
            relatorio.TotalGeralDespesas = relatorio.PessoasTotais.Sum(p => p.Despesa); //Soma de total de despesas de todas as pessoas
            relatorio.TotalGeralSaldo = relatorio.TotalGeralReceitas - relatorio.TotalGeralDespesas; //Saldo líquido final de todas as pessoas, 
                                                                                                     // total de receitas - total de  despesas
                                                                                                     
            return Ok(relatorio); //Retorno de mensagem de sucesso.
        }

    }
}