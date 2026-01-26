
export interface BankAccount {
  id: string;
  name: string;
  bankName: string;
  balance: number;
}

export interface CreditCard {
  id: string;
  name: string;
  dueDay: number;
  closingDay: number;
}

export interface Purchase {
  id: string;
  cardId: string;
  description: string;
  category: string;
  date: string; // ISO format
  totalAmount: number;
  installments: number;
}

export interface AccountTransaction {
  id: string;
  accountId: string;
  description: string;
  category: string;
  date: string;
  amount: number;
}

export interface FixedBill {
  id: string;
  name: string;
  category: string;
  amount: number;
  dueDay: number;
  month: number;
  year: number;
  isPaid: boolean;
  isRecurring: boolean;
}

export interface BillItem {
  purchaseId: string;
  description: string;
  category: string;
  installmentNumber: number;
  totalInstallments: number;
  amount: number;
  date: string;
}

export interface MonthlyBill {
  month: number;
  year: number;
  total: number;
  items: BillItem[];
}
