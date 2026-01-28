import { BankAccount, CreditCard, Purchase, AccountTransaction, FixedBill } from '../types';

const API_BASE_URL = 'http://localhost:5005/api';

// Helper para requisições
async function fetchApi<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
    ...options,
  });

  if (!response.ok) {
    throw new Error(`API Error: ${response.statusText}`);
  }

  return response.json();
}

// Mapear resposta do backend para o formato do frontend
function mapBankAccount(dto: any): BankAccount {
  return {
    id: dto.id,
    name: dto.name,
    bankName: dto.bankName,
    balance: dto.balance,
  };
}

function mapCreditCard(dto: any): CreditCard {
  return {
    id: dto.id,
    name: dto.name,
    dueDay: dto.dueDay,
    closingDay: dto.closingDay,
  };
}

function mapPurchase(dto: any): Purchase {
  return {
    id: dto.id,
    cardId: dto.creditCardId,
    description: dto.description,
    category: dto.category,
    date: dto.date,
    totalAmount: dto.totalAmount,
    installments: dto.installments,
  };
}

function mapAccountTransaction(dto: any): AccountTransaction {
  return {
    id: dto.id,
    accountId: dto.bankAccountId,
    description: dto.description,
    category: dto.category,
    date: dto.date,
    amount: dto.amount,
  };
}

function mapFixedBill(dto: any): FixedBill {
  return {
    id: dto.id,
    name: dto.name,
    category: dto.category,
    amount: dto.amount,
    dueDay: dto.dueDay,
    month: dto.month,
    year: dto.year,
    isPaid: dto.isPaid,
    isRecurring: dto.isRecurring,
  };
}

// ==================== BANK ACCOUNTS ====================
export const bankAccountsApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/BankAccounts');
    return data.map(mapBankAccount);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/BankAccounts/${id}`);
    return mapBankAccount(data);
  },
  
  create: async (account: Omit<BankAccount, 'id'>) => {
    const data = await fetchApi<any>('/BankAccounts', {
      method: 'POST',
      body: JSON.stringify(account),
    });
    return mapBankAccount(data);
  },
  
  update: async (id: string, account: Partial<BankAccount>) => {
    const data = await fetchApi<any>(`/BankAccounts/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...account, id }),
    });
    return mapBankAccount(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/BankAccounts/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== CREDIT CARDS ====================
export const creditCardsApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/CreditCards');
    return data.map(mapCreditCard);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/CreditCards/${id}`);
    return mapCreditCard(data);
  },
  
  create: async (card: Omit<CreditCard, 'id'>) => {
    const data = await fetchApi<any>('/CreditCards', {
      method: 'POST',
      body: JSON.stringify(card),
    });
    return mapCreditCard(data);
  },
  
  update: async (id: string, card: Partial<CreditCard>) => {
    const data = await fetchApi<any>(`/CreditCards/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...card, id }),
    });
    return mapCreditCard(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/CreditCards/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== PURCHASES ====================
export const purchasesApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/Purchases');
    return data.map(mapPurchase);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/Purchases/${id}`);
    return mapPurchase(data);
  },
  
  getByCard: async (cardId: string) => {
    const data = await fetchApi<any[]>(`/Purchases/card/${cardId}`);
    return data.map(mapPurchase);
  },
  
  create: async (purchase: Omit<Purchase, 'id'>) => {
    // Mapear de volta para o formato do backend
    const dto = {
      creditCardId: purchase.cardId,
      description: purchase.description,
      category: purchase.category,
      date: purchase.date,
      totalAmount: purchase.totalAmount,
      installments: purchase.installments,
    };
    const data = await fetchApi<any>('/Purchases', {
      method: 'POST',
      body: JSON.stringify(dto),
    });
    return mapPurchase(data);
  },
  
  update: async (id: string, purchase: Partial<Purchase>) => {
    const dto: any = { ...purchase };
    if (purchase.cardId) {
      dto.creditCardId = purchase.cardId;
      delete dto.cardId;
    }
    const data = await fetchApi<any>(`/Purchases/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...dto, id }),
    });
    return mapPurchase(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/Purchases/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== ACCOUNT TRANSACTIONS ====================
export const accountTransactionsApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/AccountTransactions');
    return data.map(mapAccountTransaction);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/AccountTransactions/${id}`);
    return mapAccountTransaction(data);
  },
  
  getByAccount: async (accountId: string) => {
    const data = await fetchApi<any[]>(`/AccountTransactions/account/${accountId}`);
    return data.map(mapAccountTransaction);
  },
  
  create: async (transaction: Omit<AccountTransaction, 'id'>) => {
    // Mapear de volta para o formato do backend
    const dto = {
      bankAccountId: transaction.accountId,
      description: transaction.description,
      category: transaction.category,
      date: transaction.date,
      amount: transaction.amount,
    };
    const data = await fetchApi<any>('/AccountTransactions', {
      method: 'POST',
      body: JSON.stringify(dto),
    });
    return mapAccountTransaction(data);
  },
  
  update: async (id: string, transaction: Partial<AccountTransaction>) => {
    const dto: any = { ...transaction };
    if (transaction.accountId) {
      dto.bankAccountId = transaction.accountId;
      delete dto.accountId;
    }
    const data = await fetchApi<any>(`/AccountTransactions/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...dto, id }),
    });
    return mapAccountTransaction(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/AccountTransactions/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== FIXED BILLS ====================
export const fixedBillsApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/FixedBills');
    return data.map(mapFixedBill);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/FixedBills/${id}`);
    return mapFixedBill(data);
  },
  
  create: async (bill: Omit<FixedBill, 'id'>) => {
    const data = await fetchApi<any>('/FixedBills', {
      method: 'POST',
      body: JSON.stringify(bill),
    });
    return mapFixedBill(data);
  },
  
  update: async (id: string, bill: Partial<FixedBill>) => {
    const data = await fetchApi<any>(`/FixedBills/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...bill, id }),
    });
    return mapFixedBill(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/FixedBills/${id}`, {
      method: 'DELETE',
    }),
};
