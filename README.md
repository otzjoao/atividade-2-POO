# Atividade 2 POO
# 📚 Sistema de Gerenciamento de Biblioteca Universitária

<p align="center">
  <strong>API REST desenvolvida em C# com ASP.NET Core</strong>
  <br/>
  Projeto acadêmico de Programação Orientada a Objetos (POO)
</p>

---

## 📖 Sobre o projeto

O **Sistema de Gerenciamento de Biblioteca Universitária** é uma API REST desenvolvida com C# e ASP.NET Core, com o objetivo de gerenciar materiais de uma biblioteca, como livros e revistas.

O projeto foi desenvolvido para aplicar, na prática, os principais conceitos de Programação Orientada a Objetos, além de padrões de projeto e boas práticas de desenvolvimento de APIs.

A aplicação permite cadastrar, consultar, atualizar e remover materiais, além de disponibilizar funcionalidades específicas, como descrição dos materiais e regras de empréstimo.

## 🚀 Tecnologias utilizadas

- **C#** — Linguagem de programação.
- **.NET 10** — Plataforma de desenvolvimento.
- **ASP.NET Core Web API** — Construção dos endpoints REST.
- **Swagger / OpenAPI** — Documentação e testes da API.
- **Git e GitHub** — Versionamento e colaboração em equipe.

## 🧠 Conceitos de POO aplicados

O projeto utiliza os principais pilares e recursos da Programação Orientada a Objetos:

| Conceito | Aplicação no projeto |
|---|---|
| Abstração | Classe abstrata `MaterialBiblioteca` |
| Herança | Classes `Livro` e `Revista` |
| Polimorfismo | Comportamentos específicos de cada material |
| Encapsulamento | Organização e proteção dos dados das classes |
| Interfaces | Contrato `IEmprestimo` |
| Sobrescrita | Implementação de comportamentos nas subclasses |

### 📚 Estrutura dos materiais

- **MaterialBiblioteca:** classe base abstrata para os materiais da biblioteca.
- **Livro:** representa um livro e implementa a interface `IEmprestimo`.
- **Revista:** representa uma revista, com suas características e regras específicas.
- **IEmprestimo:** define o contrato para materiais que possuem funcionalidade de empréstimo.

### ⏱️ Regras de empréstimo

- Livros: prazo de 14 dias.
- Revistas: prazo de 7 dias.

A API utiliza a interface `IEmprestimo` para identificar os materiais que permitem a operação de empréstimo.

## 🏗️ Arquitetura do projeto

O projeto utiliza o padrão Repository para separar as regras de acesso e manipulação dos materiais da camada responsável pelas requisições HTTP.

```text
MinhaBiblioteca/
│
├── Controllers/
│   └── MateriaisController.cs
│
├── models/
│   ├── IEmprestimo.cs
│   ├── Livro.cs
│   ├── MaterialBiblioteca.cs
│   └── Revista.cs
│
├── repositories/
│   ├── IMaterialRepository.cs
│   └── MaterialRepository.cs
│
├── Program.cs
├── .gitignore
└── README.md
```

### 🔧 Repository Pattern

A aplicação utiliza:

- `IMaterialRepository`: interface que define as operações do repositório.
- `MaterialRepository`: implementação responsável pelo armazenamento e gerenciamento dos materiais.

Os dados são armazenados em memória utilizando `Dictionary<int, MaterialBiblioteca>`, com identificadores únicos para cada material.

A injeção de dependência é configurada no `Program.cs`, utilizando o ciclo de vida Singleton.

> **Observação:** como o armazenamento é em memória, os dados cadastrados durante a execução não são persistidos após reiniciar a aplicação.

## 🌐 Endpoints da API

A API disponibiliza os seguintes endpoints:

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/Materiais` | Lista todos os materiais |
| GET | `/api/Materiais/{id}` | Busca material por ID |
| POST | `/api/Materiais/livro` | Cadastra um livro |
| POST | `/api/Materiais/revista` | Cadastra uma revista |
| PUT | `/api/Materiais/livro/{id}` | Atualiza um livro |
| PUT | `/api/Materiais/revista/{id}` | Atualiza uma revista |
| DELETE | `/api/Materiais/{id}` | Remove um material |
| GET | `/api/Materiais/{id}/descricao` | Obtém a descrição de um material |
| POST | `/api/Materiais/{id}/emprestar` | Solicita empréstimo de um material |

## 🧪 Testando a API

A aplicação possui integração com Swagger, permitindo visualizar, executar e testar os endpoints diretamente pelo navegador.

Após iniciar o projeto, acesse:

```text
http://localhost:5000/swagger
```

A porta pode variar conforme a configuração de execução exibida no terminal.

### Exemplo de consulta

Requisição:

```http
GET /api/Materiais
```

Exemplo de resposta:

```json
[
  {
    "id": 1,
    "titulo": "Clean Code",
    "anoPublicacao": 2008,
    "tipo": "Livro"
  },
  {
    "id": 2,
    "titulo": "Tecnologia e Inovação",
    "anoPublicacao": 2026,
    "tipo": "Revista"
  }
]
```

*Os dados acima representam os materiais iniciais cadastrados no repositório.*

## ⚙️ Como executar o projeto

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Git](https://git-scm.com/)
- Editor de código de sua preferência (VS Code recomendado).

### 1. Clonar o repositório

```bash
git clone https://github.com/otzjoao/atividade-2-POO.git
```

### 2. Entrar na pasta do projeto

```bash
cd atividade-2-POO
```

Entre na pasta que contém o arquivo `.csproj`, caso o projeto esteja em um subdiretório.

### 3. Restaurar as dependências

```bash
dotnet restore
```

### 4. Executar a aplicação

```bash
dotnet run
```

### 5. Acessar o Swagger

Abra no navegador o endereço informado pelo terminal, acrescentando `/swagger`.

## 👥 Desenvolvimento em equipe

Projeto desenvolvido de forma colaborativa, utilizando Git e GitHub para controle de versão, organização das branches e integração das implementações.

**Integrantes:**

- Gustavo
- Vinicius
- Ryan
- João Ortiz

## 🎯 Objetivos acadêmicos

- Aplicar os conceitos fundamentais de Programação Orientada a Objetos.
- Desenvolver uma API REST utilizando ASP.NET Core.
- Trabalhar com interfaces, classes abstratas, herança e polimorfismo.
- Implementar o padrão Repository.
- Utilizar injeção de dependência.
- Praticar versionamento de código e desenvolvimento colaborativo.

---

<p align="center">
  Desenvolvido com 💻 e ☕ para a disciplina de Programação Orientada a Objetos.
</p>