namespace backend.Models
{
    public class Transacao
    {
        //Classe para a Transação, com todos os atributos necessários para validá-la.
        public int id { get; set; } //Id gerado automaticamente pelo banco de dados, utilizado para identificação.
        public string Descricao { get; set; } = string.Empty; //Descrição da transação realizada.
        public decimal Valor { get; set; } //Valor da transação realizada.
        public string Tipo { get; set; } = string.Empty; //Tipo da transação, sendo receita ou despesa.
        public int PessoaId { get; set; } //Id de pessoa ligada à transação, para deleção em cascata e adição de transação ligada à uma pessoa.

    }
}