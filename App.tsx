
import React, { useState, useEffect } from 'react';
import Layout from './components/Layout';
import Dashboard from './components/Dashboard';
import AccountsManager from './components/AccountsManager';
import CardsManager from './components/CardsManager';
import AccountTransactionsManager from './components/AccountTransactionsManager';
import FixedBillsManager from './components/FixedBillsManager';
import Login from './components/Login';
import Register from './components/Register';
import ForgotPassword from './components/ForgotPassword';
import { NotificationProvider } from './components/NotificationSystem';
import { ContaBancaria, CartaoCredito, Compra, TransacaoConta, ContaFixa } from './types';
import { 
  contasBancariasApi, 
  cartoesCreditoApi, 
  comprasApi, 
  transacoesContaApi, 
  contasFixasApi 
} from './services/apiService';

const App: React.FC = () => {
  const [activeTab, setActiveTab] = useState('dashboard');
  const [loading, setLoading] = useState(true);
  const [authView, setAuthView] = useState<'login' | 'register' | 'forgot'>('login');
  
  // Auth state
  const [user, setUser] = useState<any | null>(() => {
    try {
      const storedUser = localStorage.getItem('user');
      return storedUser ? JSON.parse(storedUser) : null;
    } catch (error) {
      console.error('Erro ao carregar usuário do localStorage:', error);
      localStorage.removeItem('user');
      return null;
    }
  });

  const [accounts, setAccounts] = useState<ContaBancaria[]>([]);
  const [cards, setCards] = useState<CartaoCredito[]>([]);
  const [purchases, setPurchases] = useState<Compra[]>([]);
  const [accountTransactions, setAccountTransactions] = useState<TransacaoConta[]>([]);
  const [fixedBills, setFixedBills] = useState<ContaFixa[]>([]);

  // Carregar dados da API
  const loadAllData = async () => {
    try {
      setLoading(true);
      const [accountsData, cardsData, purchasesData, transactionsData, billsData] = await Promise.all([
        contasBancariasApi.getAll(),
        cartoesCreditoApi.getAll(),
        comprasApi.getAll(),
        transacoesContaApi.getAll(),
        contasFixasApi.getAll(),
      ]);
      
      setAccounts(accountsData);
      setCards(cardsData);
      setPurchases(purchasesData);
      setAccountTransactions(transactionsData);
      setFixedBills(billsData);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      alert('Erro ao conectar com o servidor. Verifique se o backend está rodando.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (user) {
      loadAllData();
    }
  }, [user]);

  const handleAddAccountTransaction = async (transaction: TransacaoConta) => {
    try {
      const created = await transacoesContaApi.create(transaction);
      setAccountTransactions(prev => [...prev, created]);
      
      // Atualizar saldo localmente
      const account = accounts.find(acc => acc.id === transaction.contaBancariaId);
      if (account) {
        const updatedBalance = account.saldo - transaction.valor;
        await contasBancariasApi.update(account.id, { saldo: updatedBalance });
        setAccounts(prevAccounts => 
          prevAccounts.map(acc => 
            acc.id === transaction.contaBancariaId ? { ...acc, saldo: updatedBalance } : acc
          )
        );
      }
    } catch (error) {
      console.error('Erro ao adicionar transação:', error);
      alert('Erro ao adicionar transação');
    }
  };

  const handleDeleteAccountTransaction = async (id: string) => {
    try {
      const transaction = accountTransactions.find(t => t.id === id);
      if (!transaction) return;
      
      await transacoesContaApi.delete(id);
      
      // Restaurar saldo
      const account = accounts.find(acc => acc.id === transaction.contaBancariaId);
      if (account) {
        const updatedBalance = account.saldo + transaction.valor;
        await contasBancariasApi.update(account.id, { saldo: updatedBalance });
        setAccounts(prevAccounts => 
          prevAccounts.map(acc => 
            acc.id === transaction.contaBancariaId ? { ...acc, saldo: updatedBalance } : acc
          )
        );
      }
      
      setAccountTransactions(prev => prev.filter(t => t.id !== id));
    } catch (error) {
      console.error('Erro ao deletar transação:', error);
      alert('Erro ao deletar transação');
    }
  };

  if (loading && user) {
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
        <div className="text-center">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
          <p className="mt-4 text-slate-600">Carregando...</p>
        </div>
      </div>
    );
  }

  const renderContent = () => {
    switch (activeTab) {
      case 'dashboard':
        return <Dashboard accounts={accounts} cards={cards} purchases={purchases} fixedBills={fixedBills} />;
      case 'accounts':
        return <AccountsManager accounts={accounts} setAccounts={setAccounts} />;
      case 'cards':
        return <CardsManager cards={cards} setCards={setCards} purchases={purchases} setPurchases={setPurchases} />;
      case 'transactions':
        return (
          <AccountTransactionsManager 
            accounts={accounts}
            transactions={accountTransactions}
            onAddTransaction={handleAddAccountTransaction}
            onDeleteTransaction={handleDeleteAccountTransaction}
          />
        );
      case 'fixed-bills':
        return <FixedBillsManager bills={fixedBills} setBills={setFixedBills} />;
      default:
        return <Dashboard accounts={accounts} cards={cards} purchases={purchases} fixedBills={fixedBills} />;
    }
  };

  return (
    <NotificationProvider>
      {!user ? (
        authView === 'login' ? (
          <Login 
            onLogin={(userData: any) => {
              setUser(userData);
              setAuthView('login');
            }}
            onRegisterClick={() => {
              console.log('🟢 Mudando para registro');
              setAuthView('register');
            }}
            onForgotPasswordClick={() => {
              console.log('🟢 Mudando para esqueci senha');
              setAuthView('forgot');
            }}
          />
        ) : authView === 'register' ? (
          <Register 
            onRegisterSuccess={(userData: any) => {
              setUser(userData);
              setAuthView('login');
            }}
            onBackToLogin={() => setAuthView('login')}
          />
        ) : (
          <ForgotPassword 
            onBackToLogin={() => setAuthView('login')}
          />
        )
      ) : (
        <Layout activeTab={activeTab} setActiveTab={setActiveTab} user={user} onLogout={() => {
          localStorage.removeItem('user');
          setUser(null);
          setAuthView('login');
        }}>
          {renderContent()}
        </Layout>
      )}
    </NotificationProvider>
  );
};

export default App;
