
import React, { useState, useEffect } from 'react';
import Layout from './components/Layout';
import Dashboard from './components/Dashboard';
import AccountsManager from './components/AccountsManager';
import CardsManager from './components/CardsManager';
import AccountTransactionsManager from './components/AccountTransactionsManager';
import FixedBillsManager from './components/FixedBillsManager';
import Login from './components/Login';
import { BankAccount, CreditCard, Purchase, AccountTransaction, FixedBill } from './types';
import { 
  bankAccountsApi, 
  creditCardsApi, 
  purchasesApi, 
  accountTransactionsApi, 
  fixedBillsApi 
} from './services/apiService';

const App: React.FC = () => {
  const [activeTab, setActiveTab] = useState('dashboard');
  const [loading, setLoading] = useState(true);
  
  // Auth state
  const [user, setUser] = useState<string | null>(() => {
    return localStorage.getItem('user');
  });

  const [accounts, setAccounts] = useState<BankAccount[]>([]);
  const [cards, setCards] = useState<CreditCard[]>([]);
  const [purchases, setPurchases] = useState<Purchase[]>([]);
  const [accountTransactions, setAccountTransactions] = useState<AccountTransaction[]>([]);
  const [fixedBills, setFixedBills] = useState<FixedBill[]>([]);

  // Carregar dados da API
  useEffect(() => {
    if (user) {
      loadAllData();
    }
  }, [user]);

  const loadAllData = async () => {
    try {
      setLoading(true);
      const [accountsData, cardsData, purchasesData, transactionsData, billsData] = await Promise.all([
        bankAccountsApi.getAll(),
        creditCardsApi.getAll(),
        purchasesApi.getAll(),
        accountTransactionsApi.getAll(),
        fixedBillsApi.getAll(),
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

  const handleLogin = (email: string) => {
    localStorage.setItem('user', email);
    setUser(email);
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    setUser(null);
  };

  const handleAddAccountTransaction = async (transaction: AccountTransaction) => {
    try {
      const created = await accountTransactionsApi.create(transaction);
      setAccountTransactions(prev => [...prev, created]);
      
      // Atualizar saldo localmente
      const account = accounts.find(acc => acc.id === transaction.accountId);
      if (account) {
        const updatedBalance = account.balance - transaction.amount;
        await bankAccountsApi.update(account.id, { balance: updatedBalance });
        setAccounts(prevAccounts => 
          prevAccounts.map(acc => 
            acc.id === transaction.accountId ? { ...acc, balance: updatedBalance } : acc
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
      
      await accountTransactionsApi.delete(id);
      
      // Restaurar saldo
      const account = accounts.find(acc => acc.id === transaction.accountId);
      if (account) {
        const updatedBalance = account.balance + transaction.amount;
        await bankAccountsApi.update(account.id, { balance: updatedBalance });
        setAccounts(prevAccounts => 
          prevAccounts.map(acc => 
            acc.id === transaction.accountId ? { ...acc, balance: updatedBalance } : acc
          )
        );
      }
      
      setAccountTransactions(prev => prev.filter(t => t.id !== id));
    } catch (error) {
      console.error('Erro ao deletar transação:', error);
      alert('Erro ao deletar transação');
    }
  };

  if (!user) {
    return <Login onLogin={handleLogin} />;
  }

  if (loading) {
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
    <Layout activeTab={activeTab} setActiveTab={setActiveTab} user={user} onLogout={handleLogout}>
      {renderContent()}
    </Layout>
  );
};

export default App;
