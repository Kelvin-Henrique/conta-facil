# 🔥 Guia de Configuração do Firebase

## 📝 Passos para configurar o Firebase Authentication

### 1. Criar Projeto no Firebase

1. Acesse: **https://console.firebase.google.com/**
2. Clique em **"Adicionar projeto"** (ou "Add project")
3. Digite o nome: **ContaFacil** (ou qualquer nome)
4. Desabilite o Google Analytics (opcional)
5. Clique em **"Criar projeto"**

### 2. Ativar Authentication

1. No menu lateral, clique em **"Authentication"**
2. Clique em **"Get Started"** (Começar)
3. Na aba **"Sign-in method"**, clique em **"Email/Password"**
4. **Ative** a primeira opção (Email/Password)
5. Clique em **"Salvar"**

### 3. Registrar o App Web

1. No menu lateral, clique no ícone de **engrenagem** ⚙️ → **Project settings**
2. Role até a seção **"Your apps"**
3. Clique no ícone **</>** (Web)
4. Digite um nome para o app: **ContaFacil Web**
5. **NÃO** marque "Firebase Hosting"
6. Clique em **"Register app"**

### 4. Copiar as Credenciais

Você verá um código parecido com este:

```javascript
const firebaseConfig = {
  apiKey: "AIzaSyA...",
  authDomain: "contafacil-12345.firebaseapp.com",
  projectId: "contafacil-12345",
  storageBucket: "contafacil-12345.appspot.com",
  messagingSenderId: "123456789012",
  appId: "1:123456789012:web:abc123def456"
};
```

### 5. Configurar no Projeto

1. Abra o arquivo: **`config/firebase.ts`**
2. Substitua as credenciais de exemplo pelas suas credenciais
3. Salve o arquivo

**Exemplo:**

```typescript
const firebaseConfig = {
  apiKey: "AIzaSyA...", // SUA API KEY AQUI
  authDomain: "contafacil-12345.firebaseapp.com",
  projectId: "contafacil-12345",
  storageBucket: "contafacil-12345.appspot.com",
  messagingSenderId: "123456789012",
  appId: "1:123456789012:web:abc123def456"
};
```

### 6. Configurar o Banco de Dados (PostgreSQL)

Antes de rodar o backend, você precisa de um banco PostgreSQL. Opções:

#### Opção A: Usar serviço em nuvem
- Render.com (grátis)
- Supabase (grátis)
- ElephantSQL (grátis)
- Railway (grátis com limite)

#### Opção B: Instalar localmente
1. Baixe PostgreSQL: https://www.postgresql.org/download/
2. Instale com usuário `postgres` e senha de sua escolha
3. Crie um banco chamado `conta_facil`

Depois, edite o arquivo **`Backend/ContaFacil.API/appsettings.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=72.61.58.169;Port=5433;Database=conta_facil;Username=conta;Password=Conta@2026"
  }
}
```

### 7. Executar Migration no Backend

```bash
cd Backend/ContaFacil.API
dotnet ef migrations add AddUsersTable
dotnet ef database update
```

### 8. Instalar Dependências e Rodar

**Frontend:**
```bash
npm install
npm run dev
```

**Backend:**
```bash
cd Backend/ContaFacil.API
dotnet run
```

## ✅ Funcionalidades Implementadas

- ✅ **Cadastro de usuário** com Firebase Auth
- ✅ **Login** com email e senha
- ✅ **Redefinição de senha** via email
- ✅ **Tabela de usuários** no backend
- ✅ **Sincronização** entre Firebase e backend
- ✅ **Rastreamento de último login**

## 🔐 Segurança

- Senhas são gerenciadas pelo Firebase (criptografadas)
- Backend sincroniza apenas dados de perfil
- Firebase envia emails de redefinição automaticamente
- Tokens de autenticação gerenciados pelo Firebase

## 📱 Endpoints da API

- `POST /api/users` - Criar usuário
- `GET /api/users/{id}` - Buscar usuário por ID
- `GET /api/users/firebase/{uid}` - Buscar por Firebase UID
- `GET /api/users/email/{email}` - Buscar por email
- `PUT /api/users/{id}` - Atualizar usuário
- `POST /api/users/login/{firebaseUid}` - Registrar último login
- `DELETE /api/users/{id}` - Deletar usuário

## 🎉 Pronto!

Agora você tem um sistema completo de autenticação com:
- Cadastro
- Login
- Redefinição de senha
- Gerenciamento de usuários

Se tiver dúvidas, consulte a documentação oficial do Firebase:
https://firebase.google.com/docs/auth
