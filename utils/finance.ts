
import { Purchase, CreditCard, BillItem, MonthlyBill } from '../types';

export const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value);
};

export const getBestDayToBuy = (closingDay: number) => {
  // O melhor dia é o dia seguinte ao fechamento
  let bestDay = closingDay + 1;
  if (bestDay > 31) bestDay = 1;
  return bestDay;
};

export const calculateBills = (purchases: Purchase[], card: CreditCard): MonthlyBill[] => {
  const bills: { [key: string]: MonthlyBill } = {};

  purchases.forEach((p) => {
    if (p.cardId !== card.id) return;

    // Parse robusto da data YYYY-MM-DD para evitar problemas de fuso horário
    const [year, month, day] = p.date.split('-').map(Number);
    
    // Lógica: Se a compra for DEPOIS do dia de fechamento, ela cai na fatura do MÊS SEGUINTE
    // Ex: Compra dia 10, Fechamento dia 5 -> startMonthOffset = 1 (próximo mês)
    // Ex: Compra dia 2, Fechamento dia 5 -> startMonthOffset = 0 (mês atual)
    let startMonthOffset = day > card.closingDay ? 1 : 0;
    
    const amountPerInstallment = p.totalAmount / p.installments;

    for (let i = 0; i < p.installments; i++) {
      // O mês da fatura alvo é: mês da compra (0-indexed) + offset de fechamento + índice da parcela
      // Não adicionamos mais o "+ 1" fixo para que compras antes do fechamento apareçam no mês atual
      const targetDate = new Date(year, (month - 1) + i + startMonthOffset, card.dueDay);
      
      const billMonth = targetDate.getMonth();
      const billYear = targetDate.getFullYear();
      const key = `${billYear}-${billMonth}`;

      if (!bills[key]) {
        bills[key] = {
          month: billMonth,
          year: billYear,
          total: 0,
          items: [],
        };
      }

      bills[key].total += amountPerInstallment;
      bills[key].items.push({
        purchaseId: p.id,
        description: p.description,
        category: p.category,
        installmentNumber: i + 1,
        totalInstallments: p.installments,
        amount: amountPerInstallment,
        date: p.date
      });
    }
  });

  // Retorna as faturas ordenadas por data
  return Object.values(bills).sort((a, b) => {
    if (a.year !== b.year) return a.year - b.year;
    return a.month - b.month;
  });
};
