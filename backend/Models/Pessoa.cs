namespace backend.Models
{
    public class Pessoa
    {
        //Classe pessoa, com os dados solicitados: Id, Nome e Idade.
        public int Id { get; set; } //Identificador gerado automaticamente pelo banco de dados.
        public string Nome { get; set; } = string.Empty; //Nome da pessoa.
        public int Idade { get; set; } //Idade da pessoa, importante para a classe Transação.
    
    }
}