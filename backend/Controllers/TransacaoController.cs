using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    //Gerenciamento de transações, adicionando e lendo as transações já criadas, sejam elas do tipo despesa ou receita.
    [ApiController]
    [Route("api/[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly AppDbContext _context; //Crianção de variável para manipulação do banco de dados.
        public TransacaoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        //Leitura das transações.
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoes()
        {
            return await _context.Transacoes.ToListAsync();
        }
        [HttpPost]
        //Adicionar uma transação.
        public async Task<ActionResult<Transacao>> PostTransacao(Transacao transacao)
        {
            var pessoa = await _context.Pessoas.FindAsync(transacao.PessoaId); //Procurar pessoa pelo Id
            if (pessoa == null)
            {
                return NotFound("Pessoa não existente"); //Se pessoa não existe, retorna esse erro.
            }
            if (pessoa.Idade < 18 && transacao.Tipo == "Receita") //Verificação se a pessoa tem 18 anos ou mais e o tipo de transferência que deseja registrar
            {
                return BadRequest("Menores de 18 anos podem cadastrar apenas despesas."); //Se tiver menos de 18 anos, não pode ser do tipo "Receita".
            }
            _context.Transacoes.Add(transacao); //Adiciona na fila.
            await _context.SaveChangesAsync(); //Salva as mudanças no banco de dados.
            return CreatedAtAction(nameof(GetTransacoes), new { id = transacao.id }, transacao); //Confirmação de sucesso.
        }
    }
}