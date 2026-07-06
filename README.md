# 📊 Sistema de Controle de Gastos Residenciais

Projeto Full-Stack desenvolvido como parte de um desafio técnico para gerenciamento, validação e consolidação de despesas e receitas de moradores de uma residência.

---

## 🛠️ Tecnologias Utilizadas

* **Back-end:** C# (.NET Core), Entity Framework Core, Banco de Dados SQLite.
* **Front-end:** React, TypeScript, gerenciamento de estados (`useState`), gatilhos (`useEffect`) e consumo de APIs assíncronas (`fetch`).
* **Interface e Estilização:** Visual moderno e responsivo via Water.css (Framework Classless).

---

## 🚀 Como Executar o Projeto Localmente

### 1. Pré-requisitos
Antes de começar, certifique-se de ter instalado em sua máquina:
* **.NET SDK** (versão 8.0 ou superior)
* **Node.js** (versão 18 ou superior)

### 2. Configurando e Rodando o Back-end (API C#)
Abra o seu terminal na pasta raiz do projeto e execute os seguintes comandos:
```bash
# Navegar até a pasta do servidor
cd backend

# Atualizar o histórico de migrations e criar o arquivo do banco de dados SQLite
dotnet ef database update

# Inicializar o servidor do Back-end
dotnet run

### 3. Configurando e Rodando o Front-end (React)
Abra um segundo terminal (mantendo o do back-end rodando) e execute:
# Navegar até a pasta da interface
cd frontend

# Instalar todas as dependências do projeto
npm install

# Inicializar o servidor de desenvolvimento do React
npm run dev
