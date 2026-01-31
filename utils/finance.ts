
import { Compra, CartaoCredito, ItemFatura, FaturaMensal } from '../types';

export const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value);
};

export const getBestDayToBuy = (diaFechamento: number) => {
  // O melhor dia é o dia seguinte ao fechamento
  let bestDay = diaFechamento + 1;
  if (bestDay > 31) bestDay = 1;
  return bestDay;
};

export const calculateBills = (compras: Compra[], cartao: CartaoCredito): FaturaMensal[] => {
  const bills: { [key: string]: FaturaMensal } = {};

  compras.forEach((p) => {
    if (p.cartaoCreditoId !== cartao.id) return;

    // Parse robusto da data YYYY-MM-DD para evitar problemas de fuso horário
    const [year, month, day] = p.data.split('-').map(Number);
    
    // Lógica: Se a compra for DEPOIS do dia de fechamento, ela cai na fatura do MÊS SEGUINTE
    // Ex: Compra dia 10, Fechamento dia 5 -> startMonthOffset = 1 (próximo mês)
    // Ex: Compra dia 2, Fechamento dia 5 -> startMonthOffset = 0 (mês atual)
    let startMonthOffset = day > cartao.diaFechamento ? 1 : 0;
    
    const amountPerInstallment = p.valorTotal / p.parcelas;

    for (let i = 0; i < p.parcelas; i++) {
      // O mês da fatura alvo é: mês da compra (0-indexed) + offset de fechamento + índice da parcela
      // Não adicionamos mais o "+ 1" fixo para que compras antes do fechamento apareçam no mês atual
      const targetDate = new Date(year, (month - 1) + i + startMonthOffset, cartao.diaVencimento);
      
      const billMonth = targetDate.getMonth();
      const billYear = targetDate.getFullYear();
      const key = `${billYear}-${billMonth}`;

      if (!bills[key]) {
        bills[key] = {
          mes: billMonth,
          ano: billYear,
          total: 0,
          itens: [],
        };
      }

      bills[key].total += amountPerInstallment;
      bills[key].itens.push({
        compraId: p.id,
        descricao: p.descricao,
        categoria: p.categoria,
        numeroParcela: i + 1,
        totalParcelas: p.parcelas,
        valor: amountPerInstallment,
        data: p.data
      });
    }
  });

  // Retorna as faturas ordenadas por data
  return Object.values(bills).sort((a, b) => {
    if (a.ano !== b.ano) return a.ano - b.ano;
    return a.mes - b.mes;
  });
};
