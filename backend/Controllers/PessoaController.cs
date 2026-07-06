using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    //Gerenciamento e controle das pessoas da residência, com leitura das pessoas, adição de pessoas e remoção.
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly AppDbContext _context; //Criação da variável para utilizar o banco de dados
        public PessoaController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        //Ler as pessoas presentes no banco de dados.
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            return await _context.Pessoas.ToListAsync();
        }
        [HttpPost]
        //Adicionar uma pessoa ao banco de dados.
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            if (string.IsNullOrWhiteSpace(pessoa.Nome)) //Teste para verificar se o nome não está nulo ou em espaços em branco, como "    "
            {
                return BadRequest("O nome da pessoa é obrigatório."); //Uso de função da classe herdada ControllerBase, para retornar um erro no Swagger.
            }
            _context.Pessoas.Add(pessoa); //Adicionar na fila para adição no banco de dados
            await _context.SaveChangesAsync(); //Adição no banco de dados e gerar id para a pessoa.
            return CreatedAtAction(nameof(GetPessoas), new { id = pessoa.Id }, pessoa); // Retorna o status informando que o registro foi gerado com sucesso.
        }
        [HttpDelete("{id}")]
        //Deleção de pessoa do banco de dados pelo ID dela.
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) //Verificação para ver se a pessoa realmente existe.
            {
                return NotFound("Pessoa nao encontrada."); //Retorno de erro para caso não exista.
            }
            _context.Pessoas.Remove(pessoa); //Remoção do banco de dados
            await _context.SaveChangesAsync(); //Salvar a mudança.
            return NoContent(); //retorno de sucesso na realização
        }
    }
}