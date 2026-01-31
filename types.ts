
export interface Usuario {
  id: number;
  firebaseUid: string;
  email: string;
  nome: string;
  criadoEm: string;
  ultimoLoginEm?: string;
  ativo: boolean;
}

export interface ContaBancaria {
  id: string;
  nome: string;
  nomeBanco: string;
  saldo: number;
}

export interface CartaoCredito {
  id: string;
  nome: string;
  diaVencimento: number;
  diaFechamento: number;
}

export interface Compra {
  id: string;
  cartaoCreditoId: string;
  descricao: string;
  categoria: string;
  data: string; // ISO format
  valorTotal: number;
  parcelas: number;
}

export interface TransacaoConta {
  id: string;
  contaBancariaId: string;
  descricao: string;
  categoria: string;
  data: string;
  valor: number;
}

export interface ContaFixa {
  id: string;
  nome: string;
  categoria: string;
  valor: number;
  diaVencimento: number;
  mes: number;
  ano: number;
  pago: boolean;
  recorrente: boolean;
}

export interface ItemFatura {
  compraId: string;
  descricao: string;
  categoria: string;
  numeroParcela: number;
  totalParcelas: number;
  valor: number;
  data: string;
}

export interface FaturaMensal {
  mes: number;
  ano: number;
  total: number;
  itens: ItemFatura[];
}
