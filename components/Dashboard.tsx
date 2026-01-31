
import React, { useState, useEffect } from 'react';
import { ContaBancaria, CartaoCredito, Compra, ContaFixa } from '../types';
import { formatCurrency, calculateBills } from '../utils/finance';
import { ICONS } from '../constants';
// import { getFinancialAdvice } from '../services/geminiService'; // Desabilitado temporariamente
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Cell } from 'recharts';

interface DashboardProps {
  accounts: ContaBancaria[];
  cards: CartaoCredito[];
  purchases: Compra[];
  fixedBills: ContaFixa[];
}

const Dashboard: React.FC<DashboardProps> = ({ accounts, cards, purchases, fixedBills }) => {
  const [advice, setAdvice] = useState<string>("Dicas de IA desabilitadas temporariamente.");

  /* Desabilitado temporariamente
  useEffect(() => {
    const loadAdvice = async () => {
      const msg = await getFinancialAdvice(accounts, cards, purchases);
      setAdvice(msg || "Dê mais contexto para receber dicas.");
    };
    loadAdvice();
  }, [accounts, cards, purchases]);
  */

  const totalBalance = accounts.reduce((acc, curr) => acc + curr.saldo, 0);
  
  const now = new Date();
  const currentMonth = now.getMonth();
  const currentYear = now.getFullYear();

  // Pendências de Contas Fixas
  const pendingFixedBills = fixedBills
    .filter(b => b.mes === currentMonth && b.ano === currentYear && !b.pago)
    .reduce((acc, curr) => acc + curr.valor, 0);

  // Consolidar todas as faturas de todos os cartões
  const allBillsMap: { [key: string]: { total: number; name: string; sortKey: number } } = {};
  
  cards.forEach(card => {
    const cardBills = calculateBills(purchases, card);
    cardBills.forEach(bill => {
      const key = `${bill.ano}-${bill.mes}`;
      if (!allBillsMap[key]) {
        allBillsMap[key] = {
          total: 0,
          name: new Intl.DateTimeFormat('pt-BR', { month: 'short' }).format(new Date(bill.ano, bill.mes)),
          sortKey: bill.ano * 12 + bill.mes
        };
      }
      allBillsMap[key].total += bill.total;
    });
  });

  const chartData = Object.values(allBillsMap)
    .sort((a, b) => a.sortKey - b.sortKey)
    .filter(b => b.sortKey >= (currentYear * 12 + currentMonth))
    .slice(0, 6);

  const nextInvoiceTotal = chartData[0]?.total || 0;

  return (
    <div className="space-y-6 animate-fadeIn">
      <header className="flex justify-between items-center">
        <div>
          <h2 className="text-3xl font-bold text-slate-800">Resumo Geral</h2>
          <p className="text-slate-500">Acompanhe seu patrimônio e gastos</p>
        </div>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="bg-indigo-600 p-6 rounded-3xl shadow-lg shadow-indigo-200 text-white">
          <div className="flex items-center gap-2 text-indigo-100 mb-2">
            <ICONS.Wallet className="w-5 h-5" />
            <span className="font-medium">Saldo em Contas</span>
          </div>
          <div className="text-2xl font-bold">{formatCurrency(totalBalance)}</div>
        </div>

        <div className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100">
          <div className="flex items-center gap-2 text-slate-500 mb-2">
            <ICONS.CreditCard className="w-5 h-5" />
            <span className="font-medium">Próxima Fatura</span>
          </div>
          <div className="text-2xl font-bold text-rose-500">
            {formatCurrency(nextInvoiceTotal)}
          </div>
        </div>

        <div className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100">
          <div className="flex items-center gap-2 text-slate-500 mb-2">
            <ICONS.Calendar className="w-5 h-5" />
            <span className="font-medium">Contas Fixas Mês</span>
          </div>
          <div className="text-2xl font-bold text-amber-500">
            {formatCurrency(pendingFixedBills)}
          </div>
          <p className="text-[10px] text-slate-400 font-bold uppercase mt-1">Pendentes em {new Intl.DateTimeFormat('pt-BR', { month: 'long' }).format(now)}</p>
        </div>

        <div className="bg-emerald-50 p-6 rounded-3xl border border-emerald-100 relative overflow-hidden col-span-1 md:col-span-1">
          <div className="flex items-center gap-2 text-emerald-700 mb-2">
            <div className="bg-emerald-200 p-1 rounded-full">
              <ICONS.ArrowUpRight className="w-4 h-4" />
            </div>
            <span className="font-semibold text-xs">Dica da IA</span>
          </div>
          <p className="text-emerald-900 text-[11px] italic leading-tight">
            {advice.slice(0, 100)}...
          </p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100">
          <h3 className="text-lg font-bold text-slate-800 mb-4">Projeção de Faturas</h3>
          <div className="h-64 w-full">
            {chartData.length > 0 ? (
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={chartData}>
                  <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f1f5f9" />
                  <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{fill: '#94a3b8', fontSize: 12}} />
                  <YAxis hide />
                  <Tooltip 
                    cursor={{fill: '#f8fafc'}}
                    contentStyle={{ borderRadius: '12px', border: 'none', boxShadow: '0 4px 12px rgba(0,0,0,0.1)' }}
                    formatter={(value: number) => [formatCurrency(value), 'Fatura Total']}
                  />
                  <Bar dataKey="total" radius={[8, 8, 0, 0]}>
                    {chartData.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={index === 0 ? '#6366f1' : '#cbd5e1'} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            ) : (
              <div className="h-full flex items-center justify-center text-slate-400 font-medium italic">
                Sem faturas futuras projetadas.
              </div>
            )}
          </div>
        </div>

        <div className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100">
          <h3 className="text-lg font-bold text-slate-800 mb-4">Últimas Compras</h3>
          <div className="space-y-4">
            {purchases.length === 0 ? (
              <p className="text-slate-400 text-center py-12">Nenhuma compra lançada ainda.</p>
            ) : (
              [...purchases].reverse().slice(0, 4).map((p) => (
                <div key={p.id} className="flex justify-between items-center p-3 hover:bg-slate-50 rounded-xl transition-colors">
                  <div className="flex items-center gap-4">
                    <div className="bg-indigo-100 text-indigo-600 p-2 rounded-lg">
                      <ICONS.CreditCard className="w-5 h-5" />
                    </div>
                    <div>
                      <p className="font-semibold text-slate-800">{p.descricao}</p>
                      <p className="text-xs text-slate-500">{new Date(p.data + 'T12:00:00').toLocaleDateString('pt-BR')}</p>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="font-bold text-slate-800">{formatCurrency(p.valorTotal)}</p>
                    <p className="text-xs text-indigo-500">{p.parcelas}x de {formatCurrency(p.valorTotal/p.parcelas)}</p>
                  </div>
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
