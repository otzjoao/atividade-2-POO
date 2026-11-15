# Sistema de Gerenciamento de Biblioteca Universitária

<p align="center">
  <strong>API REST desenvolvida em C# com ASP.NET Core</strong><br>
  Projeto acadêmico de Programação Orientada a Objetos (POO)
</p>

---

## Sobre o projeto

O **Sistema de Gerenciamento de Biblioteca Universitária** é uma API REST desenvolvida com C# e ASP.NET Core para gerenciar materiais de uma biblioteca, como livros e revistas.

O projeto aplica conceitos de Programação Orientada a Objetos, padrões de projeto e boas práticas de desenvolvimento de APIs.

A aplicação permite cadastrar, consultar, atualizar e remover materiais. Também disponibiliza funcionalidades para consultar a descrição de um material e solicitar seu empréstimo.

## Tecnologias utilizadas

- **C#** — linguagem de programação.
- **.NET 10** — plataforma de desenvolvimento.
- **ASP.NET Core Web API** — construção dos endpoints REST.
- **Swagger / OpenAPI** — documentação e testes da API.
- **Git e GitHub** — versionamento e colaboração em equipe.

## Conceitos de POO aplicados

| Conceito | Aplicação no projeto |
|---|---|
| Abstração | Classe abstrata `MaterialBiblioteca` |
| Herança | Classes `Livro` e `Revista` |
| Polimorfismo | Comportamentos específicos de cada material |
| Encapsulamento | Organização e proteção dos dados das classes |
| Interfaces | Contrato `IEmprestimo` |
| Sobrescrita | Implementação de comportamentos nas subclasses |

### Estrutura dos materiais

- **MaterialBiblioteca:** classe base abstrata para os materiais da biblioteca.
- **Livro:** representa um livro e implementa a interface `IEmprestimo`.
- **Revista:** representa uma revista, com características e regras específicas.
- **IEmprestimo:** define o contrato para materiais que possuem funcionalidade de empréstimo.

### Regras de empréstimo

- **Livros:** prazo de 14 dias.
- **Revistas:** prazo de 7 dias.

A API utiliza a interface `IEmprestimo` para identificar os materiais que permitem a operação de empréstimo.

## Arquitetura do projeto

O projeto utiliza o padrão Repository para separar o acesso e a manipulação dos materiais da camada responsável pelas requisições HTTP.

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

### Repository Pattern

A aplicação utiliza:

- `IMaterialRepository`: interface que define as operações do repositório.
- `MaterialRepository`: implementação responsável pelo armazenamento e gerenciamento dos materiais.

Os dados são armazenados em memória utilizando `Dictionary<int, MaterialBiblioteca>`, com identificadores únicos para cada material.

A injeção de dependência é configurada no `Program.cs`, utilizando o ciclo de vida Singleton.

> **Observação:** como o armazenamento é em memória, os dados cadastrados durante a execução não são persistidos após reiniciar a aplicação.

## Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/Materiais` | Lista materiais com filtros e paginação |
| GET | `/api/Materiais/{id}` | Busca material por ID |
| POST | `/api/Materiais/livro` | Cadastra um livro |
| POST | `/api/Materiais/revista` | Cadastra uma revista |
| PUT | `/api/Materiais/livro/{id}` | Atualiza um livro |
| PUT | `/api/Materiais/revista/{id}` | Atualiza uma revista |
| DELETE | `/api/Materiais/{id}` | Remove um material |
| GET | `/api/Materiais/{id}/descricao` | Obtém a descrição de um material |
| POST | `/api/Materiais/{id}/emprestar` | Solicita empréstimo de um material |

### Filtros e paginação

O endpoint `GET /api/Materiais` aceita os seguintes parâmetros:

| Parâmetro | Descrição | Padrão |
|---|---|---|
| `titulo` | Filtra pelo título ou parte dele | Sem filtro |
| `tipo` | Filtra por Livro ou Revista | Sem filtro |
| `ano` | Filtra pelo ano de publicação | Sem filtro |
| `pagina` | Número da página | `1` |
| `quantidade` | Registros por página (máximo de 100) | `10` |

**Exemplo de requisição:**

```http
GET /api/Materiais?titulo=Clean&tipo=Livro&ano=2008&pagina=1&quantidade=5
```

## Exemplos de cadastro

### Cadastro de livro

**Requisição:**

```http
POST /api/Materiais/livro
Content-Type: application/json
```

**Corpo JSON:**

```json
{
  "titulo": "Clean Code",
  "anoPublicacao": 2008,
  "autor": "Robert C. Martin",
  "paginas": 464,
  "isbn": "9780132350884",
  "editora": "Prentice Hall",
  "categoria": "Tecnologia"
}
```

Resposta esperada: `201 Created`.

### Cadastro de revista

**Requisição:**

```http
POST /api/Materiais/revista
Content-Type: application/json
```

**Corpo JSON:**

```json
{
  "titulo": "Revista de Tecnologia",
  "anoPublicacao": 2025,
  "edicao": 10,
  "mesPublicacao": 6,
  "issn": "1234-5678",
  "editora": "Editora Tech",
  "categoria": "Tecnologia"
}
```

Resposta esperada: `201 Created`.

## Respostas HTTP

A API utiliza os seguintes códigos HTTP para indicar o resultado das operações:

| Código | Significado |
|---|---|
| `200` | Operação realizada com sucesso |
| `201` | Recurso criado com sucesso |
| `204` | Operação concluída sem conteúdo de retorno |
| `400` | Dados inválidos ou operação não permitida |
| `404` | Material não encontrado |

As respostas de erro seguem o padrão **ProblemDetails**, facilitando a identificação e o tratamento de erros pelos clientes da API.

## Swagger / OpenAPI

A aplicação possui integração com Swagger / OpenAPI, permitindo visualizar, executar e testar os endpoints diretamente pelo navegador.

A documentação apresenta:

- Descrição das operações.
- Parâmetros de consulta e identificação.
- Códigos HTTP possíveis.
- Exemplos de requisição para cadastro de livros e revistas.
- Modelos utilizados nas requisições e respostas.

Após iniciar a aplicação, acesse `/swagger` no endereço local informado pelo terminal.

Exemplo:

```text
http://localhost:5000/swagger
```

> A porta pode variar conforme a configuração de execução exibida no terminal.

## Como executar o projeto

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

## Desenvolvimento em equipe

Projeto desenvolvido de forma colaborativa, utilizando Git e GitHub para controle de versão, organização das branches e integração das implementações.

**Integrantes:**

- Gustavo
- Vinicius
- Ryan
- João Ortiz

## Objetivos acadêmicos

- Aplicar os conceitos fundamentais de Programação Orientada a Objetos.
- Desenvolver uma API REST utilizando ASP.NET Core.
- Trabalhar com interfaces, classes abstratas, herança e polimorfismo.
- Implementar o padrão Repository.
- Utilizar injeção de dependência.
- Praticar versionamento de código e desenvolvimento colaborativo.

---

<p align="center">
  Desenvolvido para a disciplina de Programação Orientada a Objetos.
</p>
