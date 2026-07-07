namespace backend.Models
{
    public class PessoaTotalDto
    {
        //Classe para gerir e realizar a transferência de objeto, sendo utilizada para guardar o valor de
        //despesa e receita, além do saldo líquido final de cada pessoa.
        
        public string Nome { get; set; } = string.Empty; //Nome da pessoa.
        public decimal Receita { get; set; } //Valor de receita da pessoa.
        public decimal Despesa { get; set; } //Valor de despesa da pessoa.
        public decimal SaldoLiquido { get; set; }//Valor da Receita - Despesa.
    }
}