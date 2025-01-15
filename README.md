# FinancialServices

# PublicBonds API

## Descrição
A API **PublicBonds** oferece serviços financeiros relacionados a títulos públicos, incluindo consulta de preços diários, importação de dados históricos, precificação de títulos públicos e informações sobre os diferentes tipos de títulos disponíveis. A API segue o padrão OpenAPI 3.0.1.

---

## Pré-requisitos

- **.NET SDK 6.0 ou superior**
- Ferramenta para testar APIs REST, como [Postman](https://www.postman.com/) ou o próprio swagger gerado na execução da aplicação.

---

## Configuração do Projeto

1. **Clone o repositório:**

2. **Navegue até a pasta do projeto:**
   ```bash
   cd PublicBonds
   ```

3. **Restaure as dependências do projeto:**
   ```bash
   dotnet restore
   ```

4. **Configure os arquivos de configuração:**
   - O arquivo de configurações necessário para a execução do projeto **não é fornecido pelo repositório Git**. Ele depende de configurações locais específicas que estão configuradas apenas no servidor do administrador do sistema.
   - Entre em contato com o administrador para obter as configurações adequadas.

5. **Execute o projeto:**
   ```bash
   dotnet run
   ```

6. **Acesse a documentação interativa (Swagger):**
   Abra o navegador e acesse: `http://localhost:<porta>/swagger`

   a porta é definida no arquivo launchsettings.json

---

## Endpoints Disponíveis

### **1. Importação de Preços Diários**
- **URL:** `/DailyPrices/Import`
- **Método:** `POST`
- **Descrição:** Importa os preços históricos de títulos públicos com base nos filtros fornecidos.
- **Corpo da Requisição:**
  ```json
  {
    "bondName": "string",
    "year": 2024
  }
  ```
- **Resposta (200):**
  ```json
  {
    "success": true,
    "message": "Importação concluída com sucesso."
  }
  ```

### **2. Consulta de Preços Históricos**
- **URL:** `/DailyPrices/HistoricalPrices`
- **Método:** `GET`
- **Descrição:** Recupera os preços históricos de títulos com base nos parâmetros de consulta.
- **Parâmetros de Consulta:**
  - `BondName` (string, opcional)
  - `MaturityDate` (string, formato ISO8601, opcional)
  - `StartYear` (integer, opcional)
  - `EndYear` (integer, opcional)
- **Resposta (200):**
  ```json
  {
    "success": true,
    "data": [
      {
        "date": "2024-01-01T00:00:00Z",
        "morningBuyRate": 9.5,
        "morningSellRate": 9.6,
        "morningBuyPrice": 102.5,
        "morningSellPrice": 101.8
      }
    ]
  }
  ```

### **3. Listagem de Tipos de Títulos Disponíveis**
- **URL:** `/Informational/AvailableBondTypes`
- **Método:** `GET`
- **Descrição:** Retorna uma lista de todos os tipos de títulos disponíveis.
- **Resposta (200):**
  ```json
  {
    "success": true,
    "data": [
      {
        "bondTypeName": "NTN-B",
        "category": "Prefixado",
        "annualCouponPercentage": 5.2
      }
    ]
  }
  ```

### **4. Consulta de Títulos Disponíveis**
- **URL:** `/Informational/AvailableBonds`
- **Método:** `GET`
- **Descrição:** Lista todos os títulos disponíveis com base no tipo e status de maturação.
- **Parâmetros de Consulta:**
  - `BondTypeName` (string, opcional)
  - `IncludeMaturedBonds` (boolean, opcional)
- **Resposta (200):**
  ```json
  {
    "success": true,
    "data": [
      {
        "bondName": "Tesouro Prefixado 2026",
        "maturityDate": "2026-01-01T00:00:00Z",
        "bondType": {
          "bondTypeName": "Prefixado",
          "category": "Renda Fixa"
        }
      }
    ]
  }
  ```

### **5. Consulta de Preços**
- **URL:** `/Pricing`
- **Método:** `GET`
- **Descrição:** Calcula o preço de compra e venda de títulos públicos com base nos parâmetros fornecidos.
- **Parâmetros de Consulta:**
  - `BondName` (string, opcional)
  - `BondMaturityDate` (string, formato ISO8601, opcional)
  - `PurchaseDate` (string, formato ISO8601, opcional)
  - `Rate` (number, opcional)
  - `Quantity` (number, opcional)
- **Resposta (200):**
  ```json
  {
    "success": true,
    "data": [
      {
        "presentValue": 1000.5,
        "duration": 2.5,
        "cashFlowPayments": []
      }
    ]
  }
  ```

### **6. Consulta de VNA (Valor Nominal Atualizado)**
- **URL:** `/Vna`
- **Método:** `GET`
- **Descrição:** Recupera o VNA para um índice específico em um intervalo de datas.
- **Parâmetros de Consulta:**
  - `StartDate` (string, formato ISO8601, obrigatório)
  - `EndDate` (string, formato ISO8601, obrigatório)
  - `Indexer` (string, opcional)
- **Resposta (200):**
  ```json
  {
    "success": true,
    "data": [
      {
        "index": "IPCA",
        "nominalValue": 1050.75,
        "referenceDate": "2024-01-01T00:00:00Z"
      }
    ]
  }
  ```

---

## Observações

- Todas as respostas seguem o padrão de envelope com `success`, `message` e `data`.
- Utilize os filtros fornecidos nos parâmetros para otimizar as consultas.

---

## Contato
Para dúvidas ou suporte, entre em contato com o administrador do projeto.
