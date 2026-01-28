
import React, { useState, useEffect } from 'react';
import { FixedBill } from '../types';
import { ICONS, CATEGORIES } from '../constants';
import { formatCurrency } from '../utils/finance';
import { fixedBillsApi } from '../services/apiService';

interface FixedBillsManagerProps {
  bills: FixedBill[];
  setBills: React.Dispatch<React.SetStateAction<FixedBill[]>>;
}

const MONTHS = [
  "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
  "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
];

const FixedBillsManager: React.FC<FixedBillsManagerProps> = ({ bills, setBills }) => {
  const now = new Date();
  const [viewingMonth, setViewingMonth] = useState(now.getMonth());
  const [viewingYear, setViewingYear] = useState(now.getFullYear());
  
  const [modalConfig, setModalConfig] = useState<{ 
    isOpen: boolean; 
    mode: 'add' | 'edit'; 
    billId?: string 
  }>({
    isOpen: false,
    mode: 'add'
  });

  const [formData, setFormData] = useState({
    name: '',
    category: CATEGORIES[0],
    amount: '',
    dueDay: now.getDate().toString(),
    isRecurring: true,
    isPaid: false
  });

  // Automatic Recurring Logic
  useEffect(() => {
    const currentMonthBills = bills.filter(b => b.month === viewingMonth && b.year === viewingYear);
    
    if (currentMonthBills.length === 0) {
      let prevMonth = viewingMonth - 1;
      let prevYear = viewingYear;
      if (prevMonth < 0) {
        prevMonth = 11;
        prevYear--;
      }

      const recurringFromPrev = bills.filter(b => 
        b.month === prevMonth && 
        b.year === prevYear && 
        b.isRecurring
      );

      if (recurringFromPrev.length > 0) {
        const createRecurringBills = async () => {
          try {
            const newBillsPromises = recurringFromPrev.map(b => 
              fixedBillsApi.create({
                name: b.name,
                category: b.category,
                amount: b.amount,
                dueDay: b.dueDay,
                month: viewingMonth,
                year: viewingYear,
                isPaid: false,
                isRecurring: b.isRecurring
              })
            );
            const createdBills = await Promise.all(newBillsPromises);
            setBills(prev => [...prev, ...createdBills]);
          } catch (error) {
            console.error('Erro ao criar contas recorrentes:', error);
          }
        };
        createRecurringBills();
      }
    }
  }, [viewingMonth, viewingYear]);

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

  const currentBills = bills
    .filter(b => b.month === viewingMonth && b.year === viewingYear)
    .sort((a, b) => a.dueDay - b.dueDay);

  const totalPaid = currentBills.filter(b => b.isPaid).reduce((acc, curr) => acc + curr.amount, 0);
  const totalPending = currentBills.filter(b => !b.isPaid).reduce((acc, curr) => acc + curr.amount, 0);

  const openAddModal = () => {
    setFormData({
      name: '',
      category: CATEGORIES[0],
      amount: '',
      dueDay: now.getDate().toString(),
      isRecurring: true,
      isPaid: false
    });
    setModalConfig({ isOpen: true, mode: 'add' });
  };

  const openEditModal = (bill: FixedBill) => {
    setFormData({
      name: bill.name,
      category: bill.category,
      amount: formatInputToCurrency((bill.amount * 100).toFixed(0)),
      dueDay: bill.dueDay.toString(),
      isRecurring: bill.isRecurring,
      isPaid: bill.isPaid
    });
    setModalConfig({ isOpen: true, mode: 'edit', billId: bill.id });
  };

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    const numAmount = parseCurrencyToNumber(formData.amount);
    const numDueDay = parseInt(formData.dueDay) || 1;
    if (!formData.name || numAmount <= 0) return;

    try {
      if (modalConfig.mode === 'add') {
        const newBill = {
          name: formData.name,
          category: formData.category,
          amount: numAmount,
          dueDay: numDueDay,
          month: viewingMonth,
          year: viewingYear,
          isPaid: formData.isPaid,
          isRecurring: formData.isRecurring
        };
        const created = await fixedBillsApi.create(newBill);
        setBills(prev => [...prev, created]);
      } else {
        const updated = await fixedBillsApi.update(modalConfig.billId!, {
          name: formData.name,
          category: formData.category,
          amount: numAmount,
          dueDay: numDueDay,
          isRecurring: formData.isRecurring,
          isPaid: formData.isPaid
        });
        setBills(prev => prev.map(b => b.id === modalConfig.billId ? updated : b));
      }

      setModalConfig({ ...modalConfig, isOpen: false });
    } catch (error) {
      console.error('Erro ao salvar conta fixa:', error);
      alert('Erro ao salvar conta fixa');
    }
  };

  const togglePaid = async (id: string, e: React.MouseEvent) => {
    e.stopPropagation();
    try {
      const bill = bills.find(b => b.id === id);
      if (!bill) return;
      
      const updated = await fixedBillsApi.update(id, { isPaid: !bill.isPaid });
      setBills(prev => prev.map(b => b.id === id ? updated : b));
    } catch (error) {
      console.error('Erro ao atualizar status:', error);
      alert('Erro ao atualizar status');
    }
  };

  const deleteBill = async (id: string) => {
    if (!confirm("Excluir esta conta?")) return;
    
    try {
      await fixedBillsApi.delete(id);
      setBills(prev => prev.filter(b => b.id !== id));
      setModalConfig({ ...modalConfig, isOpen: false });
    } catch (error) {
      console.error('Erro ao deletar conta fixa:', error);
      alert('Erro ao deletar conta fixa');
    }
  };

  const changeMonth = (delta: number) => {
    let m = viewingMonth + delta;
    let y = viewingYear;
    if (m > 11) { m = 0; y++; }
    if (m < 0) { m = 11; y--; }
    setViewingMonth(m);
    setViewingYear(y);
  };

  return (
    <div className="space-y-6 animate-fadeIn">
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h2 className="text-2xl font-bold text-slate-800">Contas Fixas</h2>
          <p className="text-sm text-slate-500">Gestão de gastos recorrentes e mensais</p>
        </div>
        
        <div className="flex items-center gap-3 bg-white p-2 rounded-2xl border border-slate-100 shadow-sm">
          <button onClick={() => changeMonth(-1)} className="p-2 hover:bg-slate-50 rounded-xl text-indigo-600 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><path d="m15 18-6-6 6-6"/></svg>
          </button>
          <span className="font-bold text-slate-700 min-w-[140px] text-center">{MONTHS[viewingMonth]} {viewingYear}</span>
          <button onClick={() => changeMonth(1)} className="p-2 hover:bg-slate-50 rounded-xl text-indigo-600 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><path d="m9 18 6-6-6-6"/></svg>
          </button>
        </div>

        <button
          onClick={openAddModal}
          className="bg-indigo-600 text-white px-5 py-3 rounded-2xl flex items-center gap-2 hover:bg-indigo-700 transition-all shadow-lg shadow-indigo-100 font-bold active:scale-[0.98]"
        >
          <ICONS.Plus className="w-5 h-5" /> Nova Conta
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div className="bg-emerald-50 border border-emerald-100 p-6 rounded-[2rem] flex justify-between items-center">
            <div>
                <p className="text-xs font-bold text-emerald-600 uppercase tracking-widest mb-1">Total Pago</p>
                <h4 className="text-2xl font-black text-emerald-700">{formatCurrency(totalPaid)}</h4>
            </div>
            <div className="bg-emerald-200/50 p-3 rounded-2xl text-emerald-600">
                <ICONS.CheckCircle className="w-8 h-8" />
            </div>
        </div>
        <div className="bg-amber-50 border border-amber-100 p-6 rounded-[2rem] flex justify-between items-center">
            <div>
                <p className="text-xs font-bold text-amber-600 uppercase tracking-widest mb-1">A Pagar</p>
                <h4 className="text-2xl font-black text-amber-700">{formatCurrency(totalPending)}</h4>
            </div>
            <div className="bg-amber-200/50 p-3 rounded-2xl text-amber-600">
                <ICONS.Calendar className="w-8 h-8" />
            </div>
        </div>
      </div>

      <div className="bg-white rounded-[2.5rem] shadow-sm border border-slate-100 overflow-hidden">
        {currentBills.length === 0 ? (
          <div className="p-20 text-center">
            <div className="bg-slate-50 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                <ICONS.Receipt className="w-8 h-8 text-slate-300" />
            </div>
            <p className="text-slate-400 font-medium">Nenhuma conta cadastrada para este mês.</p>
          </div>
        ) : (
          <div className="divide-y divide-slate-50">
            {currentBills.map((bill) => (
              <div 
                key={bill.id} 
                onClick={() => openEditModal(bill)}
                className="p-6 flex justify-between items-center hover:bg-slate-50 transition-colors group cursor-pointer"
              >
                <div className="flex items-center gap-5">
                  <button 
                    onClick={(e) => togglePaid(bill.id, e)}
                    title={bill.isPaid ? "Marcar como pendente" : "Marcar como pago"}
                    className={`p-3 rounded-2xl transition-all border-2 ${
                      bill.isPaid 
                        ? 'bg-emerald-500 border-emerald-500 text-white shadow-lg shadow-emerald-100 scale-110' 
                        : 'bg-white border-slate-200 text-slate-200 hover:border-indigo-300'
                    }`}
                  >
                    <ICONS.CheckCircle className="w-6 h-6" />
                  </button>
                  <div>
                    <div className="flex items-center gap-2">
                      <span className="text-xs font-black text-slate-400 w-6 text-center">Dia {bill.dueDay}</span>
                      <h4 className={`font-bold text-lg transition-all ${bill.isPaid ? 'text-slate-400 line-through' : 'text-slate-800'}`}>
                        {bill.name}
                      </h4>
                      {bill.isRecurring && (
                        <span className="text-[10px] bg-indigo-100 text-indigo-600 px-2 py-0.5 rounded-md font-bold uppercase tracking-tighter">Recorrente</span>
                      )}
                    </div>
                    <p className="text-xs font-semibold text-slate-400 uppercase tracking-wide">{bill.category}</p>
                  </div>
                </div>

                <div className="flex items-center gap-8">
                  <span className={`text-xl font-black ${bill.isPaid ? 'text-slate-400' : 'text-slate-800'}`}>
                    {formatCurrency(bill.amount)}
                  </span>
                  <div className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-all">
                    <button 
                      onClick={(e) => { e.stopPropagation(); openEditModal(bill); }}
                      className="p-2 text-slate-300 hover:text-indigo-600"
                    >
                      <ICONS.Pencil className="w-5 h-5" />
                    </button>
                    <button 
                      onClick={(e) => { e.stopPropagation(); deleteBill(bill.id); }}
                      className="p-2 text-slate-300 hover:text-rose-500"
                    >
                      <ICONS.Trash className="w-5 h-5" />
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {modalConfig.isOpen && (
        <div className="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/80 backdrop-blur-md animate-fadeIn">
          <div className="bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl overflow-hidden animate-slideUp border border-white/20">
            <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
              <div className="flex items-center gap-3">
                <div className="bg-indigo-600 p-2 rounded-xl text-white">
                  <ICONS.Calendar className="w-5 h-5" />
                </div>
                <h2 className="text-xl font-bold text-slate-800">
                  {modalConfig.mode === 'add' ? 'Agendar Conta' : 'Editar Conta'}
                </h2>
              </div>
              <button onClick={() => setModalConfig({ ...modalConfig, isOpen: false })} className="p-2 hover:bg-slate-200 rounded-full text-slate-400 transition-colors">
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
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  placeholder="Ex: Aluguel, Internet, Luz..."
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
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
                <div>
                  <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Dia do Vencimento</label>
                  <input
                    type="number"
                    min="1"
                    max="31"
                    required
                    value={formData.dueDay}
                    onChange={(e) => setFormData({ ...formData, dueDay: e.target.value })}
                    className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Valor</label>
                <div className="relative">
                  <span className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 font-bold">R$</span>
                  <input
                    type="text"
                    inputMode="numeric"
                    required
                    value={formData.amount}
                    onChange={(e) => setFormData({ ...formData, amount: formatInputToCurrency(e.target.value) })}
                    className="w-full p-4 pl-11 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold"
                    placeholder="0,00"
                  />
                </div>
              </div>

              <div className="space-y-3">
                <div className="flex items-center gap-3 p-4 bg-indigo-50 rounded-2xl border border-indigo-100">
                  <input
                    type="checkbox"
                    id="recurring"
                    checked={formData.isRecurring}
                    onChange={(e) => setFormData({ ...formData, isRecurring: e.target.checked })}
                    className="w-5 h-5 accent-indigo-600 rounded"
                  />
                  <label htmlFor="recurring" className="text-sm font-bold text-indigo-700 cursor-pointer">
                    Conta Recorrente (Repetir todo mês automaticamente)
                  </label>
                </div>

                <div className={`flex items-center gap-3 p-4 rounded-2xl border transition-all ${
                  formData.isPaid ? 'bg-emerald-50 border-emerald-100 text-emerald-700' : 'bg-slate-50 border-slate-200 text-slate-600'
                }`}>
                  <input
                    type="checkbox"
                    id="isPaid"
                    checked={formData.isPaid}
                    onChange={(e) => setFormData({ ...formData, isPaid: e.target.checked })}
                    className="w-5 h-5 accent-emerald-600 rounded"
                  />
                  <label htmlFor="isPaid" className="text-sm font-bold cursor-pointer flex items-center gap-2">
                    {formData.isPaid ? (
                      <>
                        <ICONS.CheckCircle className="w-4 h-4" />
                        Esta conta já foi PAGA
                      </>
                    ) : (
                      "Marcar como PAGA agora"
                    )}
                  </label>
                </div>
              </div>

              <div className="flex flex-col gap-3">
                <button
                  type="submit"
                  className="w-full bg-indigo-600 text-white py-4 rounded-2xl font-bold text-lg shadow-xl shadow-indigo-100 hover:bg-indigo-700 transition-all active:scale-[0.98]"
                >
                  {modalConfig.mode === 'add' ? 'Confirmar Agendamento' : 'Salvar Alterações'}
                </button>
                {modalConfig.mode === 'edit' && (
                  <button
                    type="button"
                    onClick={() => deleteBill(modalConfig.billId!)}
                    className="w-full bg-rose-50 text-rose-600 py-4 rounded-2xl font-bold text-lg hover:bg-rose-100 transition-all flex items-center justify-center gap-2"
                  >
                    <ICONS.Trash className="w-5 h-5" /> Excluir permanentemente
                  </button>
                )}
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default FixedBillsManager;
