
import React, { useState } from 'react';
import { CreditCard, Purchase, MonthlyBill } from '../types';
import { ICONS } from '../constants';
import { formatCurrency, calculateBills, getBestDayToBuy } from '../utils/finance';
import TransactionsManager from './TransactionsManager';
import { creditCardsApi } from '../services/apiService';

interface CardsManagerProps {
  cards: CreditCard[];
  setCards: React.Dispatch<React.SetStateAction<CreditCard[]>>;
  purchases: Purchase[];
  setPurchases: React.Dispatch<React.SetStateAction<Purchase[]>>;
}

const MONTHS = [
  "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
  "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
];

const CardsManager: React.FC<CardsManagerProps> = ({ cards, setCards, purchases, setPurchases }) => {
  const [cardModalConfig, setCardModalConfig] = useState<{ isOpen: boolean; mode: 'add' | 'edit'; cardId?: string }>({
    isOpen: false,
    mode: 'add'
  });
  
  const [isAddingPurchase, setIsAddingPurchase] = useState(false);
  const [selectedCardId, setSelectedCardId] = useState<string | null>(cards[0]?.id || null);
  
  const now = new Date();
  const [viewingMonthIndex, setViewingMonthIndex] = useState(now.getMonth());
  const [viewingYear, setViewingYear] = useState(now.getFullYear());

  const [cardFormData, setCardFormData] = useState({ name: '', dueDay: 10, closingDay: 1 });

  const openAddCard = () => {
    setCardFormData({ name: '', dueDay: 10, closingDay: 1 });
    setCardModalConfig({ isOpen: true, mode: 'add' });
  };

  const openEditCard = (card: CreditCard) => {
    setCardFormData({ name: card.name, dueDay: card.dueDay, closingDay: card.closingDay });
    setCardModalConfig({ isOpen: true, mode: 'edit', cardId: card.id });
  };

  const saveCard = async () => {
    if (!cardFormData.name) return;
    
    try {
      if (cardModalConfig.mode === 'add') {
        const created = await creditCardsApi.create(cardFormData);
        setCards([...cards, created]);
        setSelectedCardId(created.id);
      } else {
        const updated = await creditCardsApi.update(cardModalConfig.cardId!, cardFormData);
        setCards(cards.map(c => c.id === cardModalConfig.cardId ? updated : c));
      }
      
      setCardModalConfig({ isOpen: false, mode: 'add' });
    } catch (error) {
      console.error('Erro ao salvar cartão:', error);
      alert('Erro ao salvar cartão');
    }
  };

  const removeCard = async (id: string) => {
    if (!confirm("Tem certeza que deseja excluir este cartão? Todas as compras vinculadas serão afetadas.")) return;
    
    try {
      await creditCardsApi.delete(id);
      setCards(cards.filter(c => c.id !== id));
      if (selectedCardId === id) setSelectedCardId(null);
      setCardModalConfig({ isOpen: false, mode: 'add' });
    } catch (error) {
      console.error('Erro ao deletar cartão:', error);
      alert('Erro ao deletar cartão');
    }
  };

  const handleCardChipClick = (card: CreditCard) => {
    if (selectedCardId === card.id) {
      // If already selected, open edit modal
      openEditCard(card);
    } else {
      // Otherwise, just select it
      setSelectedCardId(card.id);
    }
  };

  const selectedCard = cards.find(c => c.id === selectedCardId);
  const allBills = selectedCard ? calculateBills(purchases, selectedCard) : [];
  
  const currentBill = allBills.find(b => b.month === viewingMonthIndex && b.year === viewingYear) || {
    month: viewingMonthIndex,
    year: viewingYear,
    total: 0,
    items: []
  };

  const nextMonth = () => {
    if (viewingMonthIndex === 11) {
      setViewingMonthIndex(0);
      setViewingYear(prev => prev + 1);
    } else {
      setViewingMonthIndex(prev => prev + 1);
    }
  };

  const prevMonth = () => {
    if (viewingMonthIndex === 0) {
      setViewingMonthIndex(11);
      setViewingYear(prev => prev - 1);
    } else {
      setViewingMonthIndex(prev => prev - 1);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-bold text-slate-800">Cartões de Crédito</h2>
        <div className="flex gap-2">
            <button
                onClick={() => setIsAddingPurchase(true)}
                disabled={!selectedCardId}
                className="bg-indigo-600 text-white px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-indigo-700 transition-colors shadow-sm disabled:opacity-50 font-bold"
            >
                <ICONS.Plus className="w-5 h-5" /> Lançar Compra
            </button>
            <button
                onClick={openAddCard}
                className="bg-slate-200 text-slate-700 px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-slate-300 transition-colors shadow-sm font-bold"
            >
                <ICONS.Plus className="w-5 h-5" /> Novo Cartão
            </button>
        </div>
      </div>

      {/* Card Selector Chips */}
      <div className="flex gap-2 overflow-x-auto pb-2 scrollbar-hide">
        {cards.map((card) => (
          <button
            key={card.id}
            onClick={() => handleCardChipClick(card)}
            title={selectedCardId === card.id ? "Clique novamente para editar" : "Clique para selecionar"}
            className={`px-6 py-3 rounded-2xl font-bold transition-all whitespace-nowrap border flex items-center gap-3 relative group ${
              selectedCardId === card.id 
                ? 'bg-slate-900 text-white border-slate-900 shadow-lg scale-[1.02]' 
                : 'bg-white text-slate-600 border-slate-100 hover:border-indigo-200'
            }`}
          >
            {card.name}
            {selectedCardId === card.id && (
              <ICONS.Pencil className="w-3.5 h-3.5 text-indigo-400 opacity-60 group-hover:opacity-100 transition-opacity" />
            )}
          </button>
        ))}
      </div>

      {selectedCardId ? (
        <div className="space-y-6 animate-fadeIn">
          {/* Header Dashboard Style */}
          <div className="bg-[#111827] text-white p-8 rounded-[2.5rem] shadow-xl border border-slate-800 relative group">
            <div className="flex flex-col md:flex-row justify-between items-center gap-6">
                <div className="flex items-center gap-4">
                    <button 
                      onClick={() => openEditCard(selectedCard!)}
                      className="bg-slate-800 p-3 rounded-2xl text-indigo-400 hover:bg-slate-700 transition-all group/btn"
                    >
                        <ICONS.Pencil className="w-6 h-6 group-hover/btn:scale-110 transition-transform" />
                    </button>
                    <div>
                        <div className="flex items-center gap-2">
                          <h3 className="text-xl font-bold">{MONTHS[viewingMonthIndex]} {viewingYear}</h3>
                          <span className="text-xs bg-slate-800 text-slate-400 px-2 py-0.5 rounded-md font-bold uppercase">{selectedCard?.name}</span>
                        </div>
                        <p className="text-indigo-400 text-sm font-semibold">Melhor dia para compra: {getBestDayToBuy(selectedCard?.closingDay || 0)}</p>
                    </div>
                </div>

                <div className="flex items-center gap-8">
                    <button onClick={prevMonth} className="p-2 hover:bg-slate-800 rounded-full transition-colors">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="m15 18-6-6 6-6"/></svg>
                    </button>
                    <span className="text-lg font-bold opacity-40 capitalize">{MONTHS[viewingMonthIndex].toLowerCase()} {viewingYear}</span>
                    <button onClick={nextMonth} className="p-2 hover:bg-slate-800 rounded-full transition-colors">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="m9 18 6-6-6-6"/></svg>
                    </button>
                </div>

                <div className="text-center md:text-right">
                    <p className="text-xs text-slate-400 uppercase tracking-widest mb-1 font-bold">Fatura ({MONTHS[viewingMonthIndex].toLowerCase()})</p>
                    <p className="text-4xl font-black">{formatCurrency(currentBill.total)}</p>
                    <p className="text-xs text-slate-500 mt-1 font-bold">DIA DE VENCIMENTO {selectedCard?.dueDay}/{String(viewingMonthIndex + 1).padStart(2, '0')}</p>
                </div>
            </div>
          </div>

          {/* List of transactions */}
          <div className="bg-white rounded-[2rem] shadow-sm border border-slate-100 overflow-hidden">
            <div className="p-6 border-b border-slate-50 flex justify-between items-center">
                <h4 className="font-bold text-slate-800">Itens da Fatura</h4>
                <span className="bg-slate-100 text-slate-500 text-xs px-3 py-1 rounded-full font-bold">{currentBill.items.length} Lançamentos</span>
            </div>
            {currentBill.items.length === 0 ? (
                <div className="p-16 text-center text-slate-400 font-medium">
                    Nenhuma parcela ou compra programada para este mês.
                </div>
            ) : (
                <div className="divide-y divide-slate-50">
                    {currentBill.items.map((item, i) => (
                        <div key={i} className="p-5 flex justify-between items-center hover:bg-slate-50 transition-colors">
                            <div className="flex gap-4 items-center">
                                <div className="bg-indigo-50 text-indigo-600 p-2.5 rounded-xl">
                                    <ICONS.History className="w-5 h-5" />
                                </div>
                                <div>
                                    <p className="font-bold text-slate-800">{item.description}</p>
                                    <div className="flex gap-2 items-center mt-0.5">
                                        <span className="text-xs text-slate-400 font-medium">Parcela {item.installmentNumber}/{item.totalInstallments}</span>
                                        <span className="w-1 h-1 bg-slate-200 rounded-full"></span>
                                        <span className="text-xs font-bold text-indigo-500">{item.category}</span>
                                    </div>
                                </div>
                            </div>
                            <span className="text-lg font-black text-slate-700">{formatCurrency(item.amount)}</span>
                        </div>
                    ))}
                </div>
            )}
          </div>
        </div>
      ) : (
        <div className="bg-white p-16 rounded-[2.5rem] shadow-sm border border-slate-100 text-center">
          <div className="bg-slate-50 w-24 h-24 rounded-full flex items-center justify-center mx-auto mb-6">
            <ICONS.CreditCard className="w-12 h-12 text-slate-300" />
          </div>
          <h3 className="text-xl font-bold text-slate-800 mb-2">Nenhum cartão selecionado</h3>
          <p className="text-slate-500 max-w-xs mx-auto font-medium">Cadastre ou selecione um cartão acima para gerenciar suas faturas.</p>
        </div>
      )}

      {/* Card Form Modal (Add / Edit) */}
      {cardModalConfig.isOpen && (
        <div className="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/80 backdrop-blur-md animate-fadeIn">
          <div className="bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl overflow-hidden animate-slideUp border border-white/20">
            <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
              <div className="flex items-center gap-3">
                <div className="bg-slate-800 p-2 rounded-xl text-white">
                  <ICONS.CreditCard className="w-5 h-5" />
                </div>
                <h2 className="text-xl font-bold text-slate-800">
                  {cardModalConfig.mode === 'add' ? 'Adicionar Cartão' : 'Editar Cartão'}
                </h2>
              </div>
              <button onClick={() => setCardModalConfig({ ...cardModalConfig, isOpen: false })} className="p-2 hover:bg-slate-200 rounded-full text-slate-400 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>
              </button>
            </div>
            <div className="p-8 space-y-6">
              <div>
                <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Nome do Cartão</label>
                <input
                  type="text"
                  value={cardFormData.name}
                  onChange={(e) => setCardFormData({ ...cardFormData, name: e.target.value })}
                  className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold placeholder:text-slate-400"
                  placeholder="Ex: Nubank Black"
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Dia Fechamento</label>
                  <input
                    type="number"
                    min="1"
                    max="31"
                    value={cardFormData.closingDay}
                    onChange={(e) => setCardFormData({ ...cardFormData, closingDay: Number(e.target.value) })}
                    className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  />
                </div>
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Dia Vencimento</label>
                  <input
                    type="number"
                    min="1"
                    max="31"
                    value={cardFormData.dueDay}
                    onChange={(e) => setCardFormData({ ...cardFormData, dueDay: Number(e.target.value) })}
                    className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  />
                </div>
              </div>
              
              <div className="flex flex-col gap-3">
                <button
                  onClick={saveCard}
                  className="w-full bg-indigo-600 text-white py-4 rounded-2xl font-bold text-lg shadow-xl shadow-indigo-100 hover:bg-indigo-700 transition-all active:scale-[0.98]"
                >
                  {cardModalConfig.mode === 'add' ? 'Cadastrar Cartão' : 'Salvar Alterações'}
                </button>
                {cardModalConfig.mode === 'edit' && (
                  <button
                    onClick={() => removeCard(cardModalConfig.cardId!)}
                    className="w-full bg-rose-50 text-rose-600 py-4 rounded-2xl font-bold text-lg hover:bg-rose-100 transition-all flex items-center justify-center gap-2"
                  >
                    <ICONS.Trash className="w-5 h-5" /> Excluir Cartão
                  </button>
                )}
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Purchase Modal */}
      <TransactionsManager 
        isOpen={isAddingPurchase} 
        onClose={() => setIsAddingPurchase(false)}
        cards={cards}
        selectedCardId={selectedCardId || undefined}
        setPurchases={setPurchases}
      />
    </div>
  );
};

export default CardsManager;
