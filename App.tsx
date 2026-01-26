
import React, { useState, useEffect } from 'react';
import Layout from './components/Layout';
import Dashboard from './components/Dashboard';
import AccountsManager from './components/AccountsManager';
import CardsManager from './components/CardsManager';
import AccountTransactionsManager from './components/AccountTransactionsManager';
import FixedBillsManager from './components/FixedBillsManager';
import Login from './components/Login';
import { BankAccount, CreditCard, Purchase, AccountTransaction, FixedBill } from './types';

const App: React.FC = () => {
  const [activeTab, setActiveTab] = useState('dashboard');
  
  // Auth state
  const [user, setUser] = useState<string | null>(() => {
    return localStorage.getItem('user');
  });

  const [accounts, setAccounts] = useState<BankAccount[]>(() => {
    const saved = localStorage.getItem('accounts');
    return saved ? JSON.parse(saved) : [
      { id: '1', name: 'Reserva Emergência', bankName: 'Nubank', balance: 5000 },
      { id: '2', name: 'Conta Salário', bankName: 'Itaú', balance: 2500 }
    ];
  });

  const [cards, setCards] = useState<CreditCard[]>(() => {
    const saved = localStorage.getItem('cards');
    return saved ? JSON.parse(saved) : [
      { id: 'c1', name: 'Visa Infinite', dueDay: 15, closingDay: 5 }
    ];
  });

  const [purchases, setPurchases] = useState<Purchase[]>(() => {
    const saved = localStorage.getItem('purchases');
    return saved ? JSON.parse(saved) : [];
  });

  const [accountTransactions, setAccountTransactions] = useState<AccountTransaction[]>(() => {
    const saved = localStorage.getItem('accountTransactions');
    return saved ? JSON.parse(saved) : [];
  });

  const [fixedBills, setFixedBills] = useState<FixedBill[]>(() => {
    const saved = localStorage.getItem('fixedBills');
    return saved ? JSON.parse(saved) : [];
  });

  useEffect(() => { localStorage.setItem('accounts', JSON.stringify(accounts)); }, [accounts]);
  useEffect(() => { localStorage.setItem('cards', JSON.stringify(cards)); }, [cards]);
  useEffect(() => { localStorage.setItem('purchases', JSON.stringify(purchases)); }, [purchases]);
  useEffect(() => { localStorage.setItem('accountTransactions', JSON.stringify(accountTransactions)); }, [accountTransactions]);
  useEffect(() => { localStorage.setItem('fixedBills', JSON.stringify(fixedBills)); }, [fixedBills]);

  const handleLogin = (email: string) => {
    localStorage.setItem('user', email);
    setUser(email);
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    setUser(null);
  };

  const handleAddAccountTransaction = (transaction: AccountTransaction) => {
    setAccountTransactions(prev => [...prev, transaction]);
    setAccounts(prevAccounts => 
      prevAccounts.map(acc => 
        acc.id === transaction.accountId ? { ...acc, balance: acc.balance - transaction.amount } : acc
      )
    );
  };

  const handleDeleteAccountTransaction = (id: string) => {
    const transaction = accountTransactions.find(t => t.id === id);
    if (!transaction) return;
    setAccounts(prevAccounts => 
      prevAccounts.map(acc => 
        acc.id === transaction.accountId ? { ...acc, balance: acc.balance + transaction.amount } : acc
      )
    );
    setAccountTransactions(prev => prev.filter(t => t.id !== id));
  };

  if (!user) {
    return <Login onLogin={handleLogin} />;
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
