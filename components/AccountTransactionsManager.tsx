
import React, { useState } from 'react';
import { BankAccount, AccountTransaction } from '../types';
import { ICONS, CATEGORIES } from '../constants';
import { formatCurrency } from '../utils/finance';

interface AccountTransactionsManagerProps {
  accounts: BankAccount[];
  transactions: AccountTransaction[];
  onAddTransaction: (t: AccountTransaction) => void;
  onDeleteTransaction: (id: string) => void;
}

const AccountTransactionsManager: React.FC<AccountTransactionsManagerProps> = ({ 
  accounts, 
  transactions, 
  onAddTransaction,
  onDeleteTransaction
}) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formData, setFormData] = useState({
    description: '',
    category: CATEGORIES[0],
    date: new Date().toISOString().split('T')[0],
    amount: '', // String to support formatted input
    accountId: accounts[0]?.id || ''
  });

  // Re-using the same masking logic from TransactionsManager
  const formatInputToCurrency = (value: string) => {
    let digits = value.replace(/\D/g, '');
    if (!digits || parseInt(digits) === 0) return "0,00";
    while (digits.length < 3) digits = "0" + digits;
    const integerPart = digits.slice(0, -2);
    const decimalPart = digits.slice(-2);
    const formattedInteger = parseInt(integerPart).toLocaleString('pt-BR');
    return `${formattedInteger},${decimalPart}`;
  };

  const parseCurrencyToNumber = (value: string): number => {
    const cleanValue = value.replace(/\./g, '').replace(',', '.');
    return parseFloat(cleanValue) || 0;
  };

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();
    const numAmount = parseCurrencyToNumber(formData.amount);
    if (!formData.description || !formData.accountId || numAmount <= 0) return;

    const newTransaction: AccountTransaction = {
      id: Math.random().toString(36).substr(2, 9),
      accountId: formData.accountId,
      description: formData.description,
      category: formData.category,
      date: formData.date,
      amount: numAmount
    };

    onAddTransaction(newTransaction);
    setIsModalOpen(false);
    setFormData({
      ...formData,
      description: '',
      amount: ''
    });
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h2 className="text-2xl font-bold text-slate-800">Lançamentos Avulsos</h2>
          <p className="text-sm text-slate-500">Despesas pagas via saldo bancário (Pix/Débito)</p>
        </div>
        <button
          onClick={() => setIsModalOpen(true)}
          disabled={accounts.length === 0}
          className="bg-indigo-600 text-white px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-indigo-700 transition-colors shadow-sm font-bold disabled:opacity-50"
        >
          <ICONS.Plus className="w-5 h-5" /> Novo Lançamento
        </button>
      </div>

      <div className="bg-white rounded-[2rem] shadow-sm border border-slate-100 overflow-hidden">
        <div className="p-6 border-b border-slate-50 flex justify-between items-center">
            <h4 className="font-bold text-slate-800">Histórico de Movimentação</h4>
            <span className="text-xs text-slate-400 font-bold uppercase">{transactions.length} registros</span>
        </div>
        
        {transactions.length === 0 ? (
          <div className="p-16 text-center text-slate-400">
            Nenhuma transação registrada nas contas bancárias.
          </div>
        ) : (
          <div className="divide-y divide-slate-50">
            {[...transactions].reverse().map((t) => {
              const acc = accounts.find(a => a.id === t.accountId);
              return (
                <div key={t.id} className="p-5 flex justify-between items-center hover:bg-slate-50 transition-colors group">
                  <div className="flex gap-4 items-center">
                    <div className="bg-slate-100 text-slate-500 p-2.5 rounded-xl">
                      <ICONS.Receipt className="w-5 h-5" />
                    </div>
                    <div>
                      <p className="font-bold text-slate-800">{t.description}</p>
                      <div className="flex gap-2 items-center mt-0.5">
                        <span className="text-xs font-bold text-indigo-500">{t.category}</span>
                        <span className="w-1 h-1 bg-slate-200 rounded-full"></span>
                        <span className="text-xs text-slate-400 font-medium">{acc?.name || 'Conta Removida'}</span>
                        <span className="w-1 h-1 bg-slate-200 rounded-full"></span>
                        <span className="text-xs text-slate-400 font-medium">{new Date(t.date + 'T12:00:00').toLocaleDateString('pt-BR')}</span>
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-6">
                    <span className="text-lg font-black text-rose-500">-{formatCurrency(t.amount)}</span>
                    <button 
                      onClick={() => onDeleteTransaction(t.id)}
                      className="p-2 text-slate-300 hover:text-rose-500 opacity-0 group-hover:opacity-100 transition-all"
                    >
                      <ICONS.Trash className="w-5 h-5" />
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>

      {isModalOpen && (
        <div className="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/80 backdrop-blur-md animate-fadeIn">
          <div className="bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl overflow-hidden animate-slideUp border border-white/20">
            <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
              <div className="flex items-center gap-3">
                <div className="bg-indigo-600 p-2 rounded-xl text-white">
                  <ICONS.Receipt className="w-5 h-5" />
                </div>
                <h2 className="text-xl font-bold text-slate-800">Novo Gasto Avulso</h2>
              </div>
              <button onClick={() => setIsModalOpen(false)} className="p-2 hover:bg-slate-200 rounded-full text-slate-400 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>
              </button>
            </div>
            <form onSubmit={handleSave} className="p-8 space-y-6">
              <div>
                <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Descrição</label>
                <input
                  required
                  autoFocus
                  type="text"
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  placeholder="Ex: Almoço no quilo"
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Conta Bancária</label>
                  <select
                    value={formData.accountId}
                    onChange={(e) => setFormData({ ...formData, accountId: e.target.value })}
                    className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  >
                    {accounts.map(acc => <option key={acc.id} value={acc.id}>{acc.name} ({formatCurrency(acc.balance)})</option>)}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Categoria</label>
                  <select
                    value={formData.category}
                    onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                    className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  >
                    {CATEGORIES.map(cat => <option key={cat} value={cat}>{cat}</option>)}
                  </select>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Data</label>
                  <input
                    type="date"
                    value={formData.date}
                    onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                    className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  />
                </div>
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Valor</label>
                  <div className="relative">
                    <span className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 font-bold">R$</span>
                    <input
                      type="text"
                      inputMode="numeric"
                      value={formData.amount}
                      onChange={(e) => setFormData({ ...formData, amount: formatInputToCurrency(e.target.value) })}
                      className="w-full p-4 pl-11 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                      placeholder="0,00"
                    />
                  </div>
                </div>
              </div>

              <button
                type="submit"
                className="w-full bg-indigo-600 text-white py-4 rounded-2xl font-bold text-lg shadow-xl shadow-indigo-100 hover:bg-indigo-700 transition-all active:scale-[0.98]"
              >
                Confirmar Lançamento
              </button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default AccountTransactionsManager;
