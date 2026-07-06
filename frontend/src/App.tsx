import { useState, useEffect } from "react";

function App() {
  // Estados para armazenar os dados do formulário de Pessoas
  const [nome, setNome] = useState(""); // Guarda o texto digitado no nome da pessoa
  const [idade, setIdade] = useState(""); // Guarda o número digitado na idade da pessoa
  const [listaPessoas, setListaPessoas] = useState<any[]>([]); // Armazena a lista de pessoas que vem do banco

  // Estados para armazenar os dados do formulário de Transações 
  const [descricao, setDescricao] = useState(""); // Guarda a descrição da transação
  const [valor, setValor] = useState(""); // Guarda o valor monetário da transação
  const [tipo, setTipo] = useState("Receita"); // Guarda se é Receita ou Despesa (padrão: Receita)
  const [pessoaSelecionadaId, setPessoaSelecionadaId] = useState(""); // Guarda o ID da pessoa vinculada à transação

  // Estado para armazenar o objeto de relatório geral vindo da API
  const [relatorio, setRelatorio] = useState<any>(null);

  // FUNÇÕES DE BUSCA (GET)

  // Busca a lista de moradores cadastrados
  async function buscarPessoas() {
    try {
      const resposta = await fetch("http://localhost:5195/api/Pessoa"); // Requisição GET para a rota de pessoas
      if (resposta.ok) {
        const dados = await resposta.json(); // Converte a resposta da API para o formato JSON
        setListaPessoas(dados); // Atualiza o estado com a lista de pessoas do banco
      }
    } catch (erro) {
      console.error("Erro ao buscar pessoas:", erro); // Exibe o erro no console do navegador caso falhe
    }
  }

  // Busca os cálculos matemáticos e totais gerais acumulados
  async function buscarRelatorio() {
    try {
      const resposta = await fetch("http://localhost:5195/api/Totais"); // Requisição GET para a rota de totais
      if (resposta.ok) {
        const dados = await resposta.json(); // Converte os dados do relatório para JSON
        setRelatorio(dados); // Armazena o relatório financeiro no estado
      }
    } catch (erro) {
      console.error("Erro ao buscar relatório:", erro);
    }
  }

  // Gatilho do React para rodar funções automaticamente assim que a página abre
  useEffect(() => {
    buscarPessoas(); // Carrega as pessoas na inicialização
    buscarRelatorio(); // Carrega o relatório financeiro na inicialização
  }, []); // Array vazio significa que roda apenas uma vez ao abrir o site

  // FUNÇÃO DE CADASTRO DE PESSOA (POST)
  async function lidarComEnvioPessoa() {
    const novaPessoa = { nome, idade: parseInt(idade) }; // Monta o objeto convertendo a idade para número inteiro
    try {
      const resposta = await fetch("http://localhost:5195/api/Pessoa", {
        method: "POST", // Define o método HTTP como POST para criação
        headers: { "Content-Type": "application/json" }, // Avisa a API que estamos enviando um JSON
        body: JSON.stringify(novaPessoa) // Converte o objeto JavaScript em texto puro JSON
      });
      if (resposta.ok) {
        alert("Pessoa cadastrada com sucesso!"); // Mensagem de sucesso na tela
        setNome(""); // Limpa o campo de texto do nome
        setIdade(""); // Limpa o campo de texto da idade
        buscarPessoas(); // Atualiza a lista de pessoas na tela
        buscarRelatorio(); // Atualiza o relatório financeiro
      } else {
        const textoErro = await resposta.text();
        alert(`Erro da API: ${textoErro}`); // Exibe o erro de validação retornado pelo C#
      }
    } catch (erro) {
      alert("Erro ao conectar ao Back-end.");
    }
  }

  // FUNÇÃO DE CADASTRO DE TRANSAÇÃO (POST)
  async function lidarComEnvioTransacao() {
    // Validação visual para impedir o envio sem selecionar um morador
    if (!pessoaSelecionadaId) {
      alert("Por favor, selecione uma pessoa responsável!");
      return;
    }

    // Monta o objeto de transação com os nomes idênticos aos atributos mapeados na API
    const novaTransacao = {
      descricao,
      valor: parseFloat(valor), // Converte o texto do valor para número decimal (float)
      tipo,
      pessoaId: parseInt(pessoaSelecionadaId) // Converte o ID selecionado para número inteiro
    };

    try {
      const resposta = await fetch("http://localhost:5195/api/Transacao", {
        method: "POST", // Método POST para criar a transação no banco
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(novaTransacao)
      });

      if (resposta.ok) {
        alert("Transação cadastrada com sucesso!");
        setDescricao(""); // Limpa o campo de descrição
        setValor(""); // Limpa o campo de valor
        buscarRelatorio(); // Atualiza a tabela do relatório com os novos valores na mesma hora
      } else {
        const textoErro = await resposta.text();
        alert(`Validação da API: ${textoErro}`); // Exibe o erro caso seja menor de 18 anos tentando receita
      }
    } catch (erro) {
      alert("Erro ao conectar ao Back-end.");
    }
  }

  // FUNÇÃO PARA DELETAR PESSOA (DELETE)
  async function deletarPessoa(id: number) {
    // Alerta de confirmação nativo do navegador para evitar cliques acidentais
    if (!confirm("Tem certeza que deseja excluir esta pessoa?")) {
      return; // Cancela a operação caso o usuário clique em "Cancelar"
    }

    try {
      // Envia o ID do morador direto na URL da rota
      const resposta = await fetch(`http://localhost:5195/api/Pessoa/${id}`, {
        method: "DELETE" // Método HTTP para remoção
      });

      if (resposta.ok) {
        alert("Pessoa excluída com sucesso!");
        buscarPessoas(); // Atualiza a tabela de moradores na tela
        buscarRelatorio(); // Atualiza os totais e saldos do relatório geral
      } else {
        alert("Erro ao excluir.");
      }
    } catch (erro) {
      alert("Erro ao conectar ao Back-end.");
    }
  }

  return (
    <div style={{ padding: "20px", fontFamily: "sans-serif", maxWidth: "800px", margin: "0 auto" }}>
      <h1>Controle de Gastos Residenciais</h1>
      <hr />

      {/* SEÇÃO: FORMULÁRIO DE CADASTRO DE PESSOAS */}
      <h2>Cadastrar Nova Pessoa</h2>
      <form onSubmit={(e) => e.preventDefault()}> {/* Impede a página de recarregar sozinha ao enviar */}
        <div>
          <label>Nome: </label>
          <input type="text" value={nome} onChange={(e) => setNome(e.target.value)} /> {/* Monitora e salva o texto no estado Nome */}
        </div>
        <br />
        <br />
        <div>
          <label>Idade: </label>
          <input type="number" value={idade} onChange={(e) => setIdade(e.target.value)} /> {/* Monitora e salva o número no estado Idade */}
        </div>
        <br />
        <br />
        <button type="button" onClick={lidarComEnvioPessoa}>Salvar Pessoa no Banco</button>
      </form>

      <hr />

      {/* SEÇÃO: FORMULÁRIO DE CADASTRO DE TRANSAÇÕES */}
      <h2>Cadastrar Nova Transação</h2>
      <form onSubmit={(e) => e.preventDefault()}>
        <div>
          <label>Quem realizou a movimentação? </label>
          <select value={pessoaSelecionadaId} onChange={(e) => setPessoaSelecionadaId(e.target.value)}>
            <option value="">-- Escolha uma Pessoa --</option>
            {/* Percorre dinamicamente a lista de pessoas para montar as opções do Select */}
            {listaPessoas.map((p) => (
              <option key={p.id} value={p.id}>{p.nome}</option>
            ))}
          </select>
        </div>
        <br />
        <div>
          <label>Descrição: </label>
          <input type="text" value={descricao} onChange={(e) => setDescricao(e.target.value)} />
        </div>
        <br />
        <div>
          <label>Valor (R$): </label>
          <input type="number" step="0.01" placeholder="Ex: 150.50" value={valor} onChange={(e) => setValor(e.target.value)} />
        </div>
        <br />
        <div>
          <label>Tipo: </label>
          <select value={tipo} onChange={(e) => setTipo(e.target.value)}>
            <option value="Receita">Receita (+)</option>
            <option value="Despesa">Despesa (-)</option>
          </select>
        </div>
        <br />
        <button type="button" onClick={lidarComEnvioTransacao}>Salvar Transação</button>
      </form>

      <hr />

      {/* SEÇÃO: TABELA DE VISUALIZAÇÃO E EXCLUSÃO DE MORADORES */}
      <h2>Pessoas Cadastradas</h2>
      <table border={1} cellPadding={10} style={{ width: "100%", textAlign: "left", borderCollapse: "collapse" }}>
        <thead>
          <tr style={{ backgroundColor: "#000020" }}>
            <th>ID</th>
            <th>Nome</th>
            <th>Idade</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {/* Percorre a lista de pessoas inserindo uma linha na tabela para cada morador */}
          {listaPessoas.map((p) => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.nome}</td>
              <td>{p.idade} anos</td>
              <td>
                {/* Botão vermelho que dispara a função passando o ID do morador da linha atual */}
                <button 
                  type="button" 
                  style={{ backgroundColor: "#ff4d4f", color: "white", border: "none", padding: "5px 10px", borderRadius: "3px", cursor: "pointer" }}
                  onClick={() => deletarPessoa(p.id)}
                >
                  Excluir
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <hr />

      {/* SEÇÃO: RELATÓRIO CONSOLIDADO DE GASTOS*/}
      <h2>📊 Relatório Consolidado de Gastos</h2>
      
      {/* Só desenha a tabela se o objeto relatorio já tiver sido carregado da API */}
      {relatorio ? (
        <div>
          <table border={1} cellPadding={10} style={{ width: "100%", textAlign: "left", borderCollapse: "collapse" }}>
            <thead>
              <tr style={{ backgroundColor: "#000020" }}>
                <th>Morador</th>
                <th>Total Receitas</th>
                <th>Total Despesas</th>
                <th>Saldo Líquido</th>
              </tr>
            </thead>
            <tbody>
              {/* Mapeia a lista interna 'pessoasTotais' calculada no Back-end */}
              {relatorio.pessoasTotais.map((pt: any, index: number) => (
                <tr key={index}>
                  <td><b>{pt.nome}</b></td> 
                  <td style={{ color: "green" }}>R$ {pt.receita.toFixed(2)}</td> {/* .toFixed(2) formata para 2 casas decimais */}
                  <td style={{ color: "red" }}>R$ {pt.despesa.toFixed(2)}</td>
                  {/* Estilização condicional: Fica verde se for positivo ou zero, e vermelho se for negativo */}
                  <td style={{ color: pt.saldoLiquido >= 0 ? "green" : "red" }}>
                    R$ {pt.saldoLiquido.toFixed(2)}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {/* Grandes totalizadores do sistema exibidos logo abaixo da tabela */}
          <div style={{ marginTop: "20px", padding: "15px", backgroundColor: "#000020", borderRadius: "5px", border: "1px solid #ddd" }}>
            <h3>Resumo Geral do Sistema:</h3>
            <p><b>Faturamento Total de Receitas:</b> <span style={{ color: "green" }}>R$ {relatorio.totalGeralReceitas.toFixed(2)}</span></p>
            <p><b>Acumulado Total de Despesas:</b> <span style={{ color: "red" }}>R$ {relatorio.totalGeralDespesas.toFixed(2)}</span></p>
            <hr />
            <h4>Saldo Final da residência: 
              <span style={{ color: relatorio.totalGeralSaldo >= 0 ? "green" : "red" }}> R$ {relatorio.totalGeralSaldo.toFixed(2)}</span>
            </h4>
          </div>
        </div>
      ) : (
        <p>Carregando dados do relatório...</p> // Exibido nos milissegundos enquanto a API não responde
      )}
    </div>
  );
}

export default App;