
import React, { useState, useEffect } from 'react';
import { Purchase, CreditCard } from '../types';
import { ICONS, CATEGORIES } from '../constants';
import { purchasesApi } from '../services/apiService';

interface TransactionsManagerProps {
  isOpen: boolean;
  onClose: () => void;
  cards: CreditCard[];
  selectedCardId?: string;
  setPurchases: React.Dispatch<React.SetStateAction<Purchase[]>>;
}

const TransactionsManager: React.FC<TransactionsManagerProps> = ({ isOpen, onClose, cards, selectedCardId, setPurchases }) => {
  const [formData, setFormData] = useState({
    description: '',
    category: CATEGORIES[0],
    date: new Date().toISOString().split('T')[0],
    totalAmount: '', // Stored as formatted string "0,00"
    installments: 1
  });

  const [feedback, setFeedback] = useState<string | null>(null);

  if (!isOpen) return null;

  // Helper to convert formatted string "1.234,56" to number 1234.56
  const parseCurrencyToNumber = (value: string): number => {
    const cleanValue = value.replace(/\./g, '').replace(',', '.');
    return parseFloat(cleanValue) || 0;
  };

  // Helper to format raw digits to "0,00" style
  const formatInputToCurrency = (value: string) => {
    // Remove non-digits
    let digits = value.replace(/\D/g, '');
    
    // If empty or just zeros
    if (!digits || parseInt(digits) === 0) return "0,00";

    // Pad with leading zeros if needed
    while (digits.length < 3) {
      digits = "0" + digits;
    }

    const integerPart = digits.slice(0, -2);
    const decimalPart = digits.slice(-2);

    // Add thousands separator
    const formattedInteger = parseInt(integerPart).toLocaleString('pt-BR');

    return `${formattedInteger},${decimalPart}`;
  };

  const handleAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const rawValue = e.target.value;
    const formatted = formatInputToCurrency(rawValue);
    setFormData({ ...formData, totalAmount: formatted });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const targetCardId = selectedCardId || cards[0]?.id;
    const numericAmount = parseCurrencyToNumber(formData.totalAmount);

    if (!formData.description || !targetCardId || numericAmount <= 0) return;

    try {
      const newPurchase = {
        description: formData.description,
        cardId: targetCardId,
        category: formData.category,
        date: formData.date,
        totalAmount: numericAmount,
        installments: formData.installments
      };

      const created = await purchasesApi.create(newPurchase);
      setPurchases(prev => [...prev, created]);
      setFeedback("Compra lançada com sucesso!");
      setTimeout(() => {
          setFeedback(null);
          onClose();
          setFormData({
            description: '',
            category: CATEGORIES[0],
            date: new Date().toISOString().split('T')[0],
            totalAmount: '',
            installments: 1
          });
      }, 1200);
    } catch (error) {
      console.error('Erro ao criar compra:', error);
      setFeedback("Erro ao lançar compra");
      setTimeout(() => setFeedback(null), 2000);
    }
  };

  const adjustInstallments = (delta: number) => {
    setFormData(prev => ({
      ...prev,
      installments: Math.max(1, Math.min(24, prev.installments + delta))
    }));
  };

  const inputClass = "w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-semibold placeholder:text-slate-400 transition-all";
  const labelClass = "block text-sm font-bold text-slate-700 mb-2 ml-1";

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-slate-900/80 backdrop-blur-md animate-fadeIn">
      <div className="bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl overflow-hidden animate-slideUp border border-white/20">
        <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <div className="flex items-center gap-3">
            <div className="bg-indigo-600 p-2 rounded-xl text-white shadow-lg shadow-indigo-200">
              <ICONS.Plus className="w-5 h-5" />
            </div>
            <div>
              <h2 className="text-xl font-bold text-slate-800 leading-tight">Novo Lançamento</h2>
              <p className="text-[10px] text-slate-400 font-bold uppercase tracking-widest">
                Vinculado a: {cards.find(c => c.id === selectedCardId)?.name || 'Cartão Selecionado'}
              </p>
            </div>
          </div>
          <button onClick={onClose} className="p-2 hover:bg-slate-200 rounded-full text-slate-400 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-8 space-y-6">
          <div>
            <label className={labelClass}>Descrição</label>
            <input
              autoFocus
              required
              type="text"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              className={inputClass}
              placeholder="Ex: Supermercado"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className={labelClass}>Categoria</label>
              <select
                value={formData.category}
                onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                className={inputClass}
              >
                {CATEGORIES.map(cat => <option key={cat} value={cat}>{cat}</option>)}
              </select>
            </div>
            <div>
              <label className={labelClass}>Data da Compra</label>
              <input
                type="date"
                required
                value={formData.date}
                onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                className={`${inputClass} cursor-pointer`}
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className={labelClass}>Valor Total</label>
              <div className="relative">
                <span className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 font-bold">R$</span>
                <input
                  type="text"
                  inputMode="numeric"
                  required
                  placeholder="0,00"
                  value={formData.totalAmount}
                  onChange={handleAmountChange}
                  className={`${inputClass} pl-11`}
                />
              </div>
            </div>
            <div>
              <label className={labelClass}>Parcelamento</label>
              <div className="flex items-center bg-slate-50 border border-slate-200 rounded-2xl h-[58px] overflow-hidden">
                <button
                  type="button"
                  onClick={() => adjustInstallments(-1)}
                  className="w-12 h-full flex items-center justify-center text-indigo-600 hover:bg-slate-200 active:bg-slate-300 transition-colors border-r border-slate-200"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><path d="M5 12h14"/></svg>
                </button>
                <div className="flex-1 text-center font-bold text-slate-900 text-lg">
                  {formData.installments}x
                </div>
                <button
                  type="button"
                  onClick={() => adjustInstallments(1)}
                  className="w-12 h-full flex items-center justify-center text-indigo-600 hover:bg-slate-200 active:bg-slate-300 transition-colors border-l border-slate-200"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><path d="M5 12h14"/><path d="M12 5v14"/></svg>
                </button>
              </div>
            </div>
          </div>

          {formData.installments > 1 && formData.totalAmount && (
            <div className="bg-indigo-50 p-3 rounded-xl border border-indigo-100 flex justify-between items-center">
                <span className="text-xs font-bold text-indigo-600 uppercase tracking-tighter">Valor da Parcela</span>
                <span className="text-sm font-black text-indigo-700">
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(parseCurrencyToNumber(formData.totalAmount) / formData.installments)}
                </span>
            </div>
          )}

          <button
            type="submit"
            disabled={!!feedback}
            className={`w-full py-4 rounded-2xl font-bold text-lg transition-all shadow-xl flex items-center justify-center gap-2 ${
              feedback 
                ? 'bg-emerald-500 text-white shadow-emerald-100' 
                : 'bg-indigo-600 text-white hover:bg-indigo-700 hover:shadow-indigo-200 active:scale-[0.98]'
            }`}
          >
            {feedback ? (
              <>
                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M5 13l4 4L19 7"></path></svg>
                {feedback}
              </>
            ) : "Confirmar Lançamento"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default TransactionsManager;
