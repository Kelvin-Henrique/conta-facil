
import React, { useState } from 'react';
import { BankAccount } from '../types';
import { ICONS } from '../constants';
import { formatCurrency } from '../utils/finance';
import { bankAccountsApi } from '../services/apiService';

interface AccountsManagerProps {
  accounts: BankAccount[];
  setAccounts: React.Dispatch<React.SetStateAction<BankAccount[]>>;
}

const AccountsManager: React.FC<AccountsManagerProps> = ({ accounts, setAccounts }) => {
  const [modalConfig, setModalConfig] = useState<{ isOpen: boolean; mode: 'add' | 'edit'; accountId?: string }>({
    isOpen: false,
    mode: 'add'
  });
  
  const [formData, setFormData] = useState({ name: '', bankName: '', balance: '' });

  const openAddModal = () => {
    setFormData({ name: '', bankName: '', balance: '0' });
    setModalConfig({ isOpen: true, mode: 'add' });
  };

  const openEditModal = (acc: BankAccount) => {
    setFormData({ name: acc.name, bankName: acc.bankName, balance: acc.balance.toString() });
    setModalConfig({ isOpen: true, mode: 'edit', accountId: acc.id });
  };

  const handleSave = async () => {
    if (!formData.name || !formData.bankName) return;
    
    const balanceNum = parseFloat(formData.balance) || 0;

    try {
      if (modalConfig.mode === 'add') {
        const newAccount = {
          name: formData.name,
          bankName: formData.bankName,
          balance: balanceNum,
        };
        const created = await bankAccountsApi.create(newAccount);
        setAccounts([...accounts, created]);
      } else {
        const updated = await bankAccountsApi.update(modalConfig.accountId!, {
          name: formData.name,
          bankName: formData.bankName,
          balance: balanceNum
        });
        setAccounts(accounts.map(acc => 
          acc.id === modalConfig.accountId ? updated : acc
        ));
      }
      
      setModalConfig({ isOpen: false, mode: 'add' });
    } catch (error) {
      console.error('Erro ao salvar conta:', error);
      alert('Erro ao salvar conta');
    }
  };

  const removeAccount = async (id: string) => {
    if (!confirm("Deseja realmente excluir esta conta?")) return;
    
    try {
      await bankAccountsApi.delete(id);
      setAccounts(accounts.filter(a => a.id !== id));
      setModalConfig({ isOpen: false, mode: 'add' });
    } catch (error) {
      console.error('Erro ao deletar conta:', error);
      alert('Erro ao deletar conta');
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-bold text-slate-800">Contas Bancárias</h2>
        <button
          onClick={openAddModal}
          className="bg-indigo-600 text-white px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-indigo-700 transition-colors shadow-sm font-bold"
        >
          <ICONS.Plus className="w-5 h-5" /> Nova Conta
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {accounts.map((acc) => (
          <button
            key={acc.id}
            onClick={() => openEditModal(acc)}
            className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100 flex justify-between items-center group hover:border-indigo-200 transition-all text-left"
          >
            <div className="flex items-center gap-4">
              <div className="bg-indigo-50 p-3 rounded-2xl text-indigo-600 group-hover:bg-indigo-100 transition-colors">
                <ICONS.Wallet className="w-8 h-8" />
              </div>
              <div>
                <div className="flex items-center gap-2">
                  <h3 className="font-bold text-slate-800 text-lg">{acc.name}</h3>
                  <ICONS.Pencil className="w-3.5 h-3.5 text-slate-300 opacity-0 group-hover:opacity-100 transition-opacity" />
                </div>
                <p className="text-sm text-slate-500">{acc.bankName}</p>
                <p className="text-xl font-bold text-indigo-600 mt-1">{formatCurrency(acc.balance)}</p>
              </div>
            </div>
          </button>
        ))}
        
        {accounts.length === 0 && (
          <div className="col-span-full py-12 text-center bg-white rounded-[2rem] border border-dashed border-slate-200">
            <p className="text-slate-400 font-medium">Nenhuma conta cadastrada ainda.</p>
          </div>
        )}
      </div>

      {/* Account Modal (Add / Edit) */}
      {modalConfig.isOpen && (
        <div className="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/80 backdrop-blur-md animate-fadeIn">
          <div className="bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl overflow-hidden animate-slideUp border border-white/20">
            <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
              <div className="flex items-center gap-3">
                <div className="bg-slate-800 p-2 rounded-xl text-white">
                  <ICONS.Wallet className="w-5 h-5" />
                </div>
                <h2 className="text-xl font-bold text-slate-800">
                  {modalConfig.mode === 'add' ? 'Nova Conta' : 'Editar Conta'}
                </h2>
              </div>
              <button onClick={() => setModalConfig({ ...modalConfig, isOpen: false })} className="p-2 hover:bg-slate-200 rounded-full text-slate-400 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>
              </button>
            </div>
            <div className="p-8 space-y-6">
              <div>
                <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Apelido da Conta</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold placeholder:text-slate-400"
                  placeholder="Ex: Minha Conta Principal"
                />
              </div>
              
              <div>
                <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Instituição Financeira</label>
                <input
                  type="text"
                  value={formData.bankName}
                  onChange={(e) => setFormData({ ...formData, bankName: e.target.value })}
                  className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold placeholder:text-slate-400"
                  placeholder="Ex: NuBank, Itaú, Inter..."
                />
              </div>

              <div>
                <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Saldo Atual</label>
                <div className="relative">
                  <span className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 font-bold">R$</span>
                  <input
                    type="number"
                    value={formData.balance}
                    onChange={(e) => setFormData({ ...formData, balance: e.target.value })}
                    className="w-full p-4 pl-11 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  />
                </div>
              </div>
              
              <div className="flex flex-col gap-3 pt-2">
                <button
                  onClick={handleSave}
                  className="w-full bg-indigo-600 text-white py-4 rounded-2xl font-bold text-lg shadow-xl shadow-indigo-100 hover:bg-indigo-700 transition-all active:scale-[0.98]"
                >
                  {modalConfig.mode === 'add' ? 'Cadastrar Conta' : 'Salvar Alterações'}
                </button>
                {modalConfig.mode === 'edit' && (
                  <button
                    onClick={() => removeAccount(modalConfig.accountId!)}
                    className="w-full bg-rose-50 text-rose-600 py-4 rounded-2xl font-bold text-lg hover:bg-rose-100 transition-all flex items-center justify-center gap-2"
                  >
                    <ICONS.Trash className="w-5 h-5" /> Excluir Conta
                  </button>
                )}
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default AccountsManager;
