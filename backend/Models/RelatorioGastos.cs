namespace backend.Models
{
    public class RelatorioGastos
    {
        //Classe para realizar o relatório final de todos as transações da família,
        //utilizando a classe PessoaTotal para criar uma lista com todos os residentes e,
        //no final, listar o total das transações.
        public List<PessoaTotalDto> PessoasTotais { get; set; } = new List<PessoaTotalDto>(); //Lista de todas as pessoas-objetos, com atributos da classe PessoaTotal,
                                                                                        //necessários para o relatório.
        public decimal TotalGeralReceitas { get; set; } //Total de receita gerada por todos os residentes.
        public decimal TotalGeralDespesas { get; set; } //Total de despesa gerada por todos os residentes.
        public decimal TotalGeralSaldo { get; set; } // Total de saldo líquiso, total de receita - total de despesas.
    }
}