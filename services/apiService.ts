import { ContaBancaria, CartaoCredito, Compra, TransacaoConta, ContaFixa } from '../types';

const API_BASE_URL = 'http://localhost:5005/api';

// Helper para requisições
async function fetchApi<T>(endpoint: string, options?: RequestInit): Promise<T> {
  console.log(`Fazendo requisição para: ${API_BASE_URL}${endpoint}`, options?.method || 'GET');
  
  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
    ...options,
  });

  if (!response.ok) {
    const errorText = await response.text();
    console.error(`Erro na API (${response.status}):`, errorText);
    throw new Error(`API Error ${response.status}: ${errorText || response.statusText}`);
  }

  // Se a resposta for 204 No Content, retorna undefined
  if (response.status === 204) {
    console.log('Resposta 204 No Content - operação bem-sucedida');
    return undefined as T;
  }

  return response.json();
}

// Mapear resposta do backend para o formato do frontend
function mapContaBancaria(dto: any): ContaBancaria {
  return {
    id: dto.id,
    nome: dto.nome,
    nomeBanco: dto.nomeBanco,
    saldo: dto.saldo,
  };
}

function mapCartaoCredito(dto: any): CartaoCredito {
  return {
    id: dto.id,
    nome: dto.nome,
    diaVencimento: dto.diaVencimento,
    diaFechamento: dto.diaFechamento,
  };
}

function mapCompra(dto: any): Compra {
  return {
    id: dto.id,
    cartaoCreditoId: dto.cartaoCreditoId,
    descricao: dto.descricao,
    categoria: dto.categoria,
    data: dto.data,
    valorTotal: dto.valorTotal,
    parcelas: dto.parcelas,
  };
}

function mapTransacaoConta(dto: any): TransacaoConta {
  return {
    id: dto.id,
    contaBancariaId: dto.contaBancariaId,
    descricao: dto.descricao,
    categoria: dto.categoria,
    data: dto.data,
    valor: dto.valor,
  };
}

function mapContaFixa(dto: any): ContaFixa {
  return {
    id: dto.id,
    nome: dto.nome,
    categoria: dto.categoria,
    valor: dto.valor,
    diaVencimento: dto.diaVencimento,
    mes: dto.mes,
    ano: dto.ano,
    pago: dto.pago,
    recorrente: dto.recorrente,
  };
}

// ==================== CONTAS BANCÁRIAS ====================
export const contasBancariasApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/ContasBancarias');
    return data.map(mapContaBancaria);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/ContasBancarias/${id}`);
    return mapContaBancaria(data);
  },
  
  create: async (conta: Omit<ContaBancaria, 'id'>) => {
    const data = await fetchApi<any>('/ContasBancarias', {
      method: 'POST',
      body: JSON.stringify(conta),
    });
    return mapContaBancaria(data);
  },
  
  update: async (id: string, conta: Partial<ContaBancaria>) => {
    const data = await fetchApi<any>(`/ContasBancarias/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...conta, id }),
    });
    return mapContaBancaria(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/ContasBancarias/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== CARTÕES DE CRÉDITO ====================
export const cartoesCreditoApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/CartoesCredito');
    return data.map(mapCartaoCredito);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/CartoesCredito/${id}`);
    return mapCartaoCredito(data);
  },
  
  create: async (cartao: Omit<CartaoCredito, 'id'>) => {
    const data = await fetchApi<any>('/CartoesCredito', {
      method: 'POST',
      body: JSON.stringify(cartao),
    });
    return mapCartaoCredito(data);
  },
  
  update: async (id: string, cartao: Partial<CartaoCredito>) => {
    const data = await fetchApi<any>(`/CartoesCredito/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...cartao, id }),
    });
    return mapCartaoCredito(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/CartoesCredito/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== COMPRAS ====================
export const comprasApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/Compras');
    return data.map(mapCompra);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/Compras/${id}`);
    return mapCompra(data);
  },
  
  getByCard: async (cartaoId: string) => {
    const data = await fetchApi<any[]>(`/Compras/cartao/${cartaoId}`);
    return data.map(mapCompra);
  },
  
  create: async (compra: Omit<Compra, 'id'>) => {
    // Mapear de volta para o formato do backend
    const dto = {
      cartaoCreditoId: compra.cartaoCreditoId,
      descricao: compra.descricao,
      categoria: compra.categoria,
      data: compra.data,
      valorTotal: compra.valorTotal,
      parcelas: compra.parcelas,
    };
    const data = await fetchApi<any>('/Compras', {
      method: 'POST',
      body: JSON.stringify(dto),
    });
    return mapCompra(data);
  },
  
  update: async (id: string, compra: Omit<Compra, 'id'>) => {
    const dto = {
      cartaoCreditoId: compra.cartaoCreditoId,
      descricao: compra.descricao,
      categoria: compra.categoria,
      data: compra.data,
      valorTotal: compra.valorTotal,
      parcelas: compra.parcelas,
    };
    const data = await fetchApi<any>(`/Compras/${id}`, {
      method: 'PUT',
      body: JSON.stringify(dto),
    });
    return mapCompra(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/Compras/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== TRANSAÇÕES DE CONTA ====================
export const transacoesContaApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/TransacoesConta');
    return data.map(mapTransacaoConta);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/TransacoesConta/${id}`);
    return mapTransacaoConta(data);
  },
  
  getByAccount: async (contaId: string) => {
    const data = await fetchApi<any[]>(`/TransacoesConta/conta/${contaId}`);
    return data.map(mapTransacaoConta);
  },
  
  create: async (transacao: Omit<TransacaoConta, 'id'>) => {
    // Mapear de volta para o formato do backend
    const dto = {
      contaBancariaId: transacao.contaBancariaId,
      descricao: transacao.descricao,
      categoria: transacao.categoria,
      data: transacao.data,
      valor: transacao.valor,
    };
    const data = await fetchApi<any>('/TransacoesConta', {
      method: 'POST',
      body: JSON.stringify(dto),
    });
    return mapTransacaoConta(data);
  },
  
  update: async (id: string, transacao: Partial<TransacaoConta>) => {
    const dto: any = { ...transacao };
    if (transacao.contaBancariaId) {
      dto.contaBancariaId = transacao.contaBancariaId;
      delete dto.contaBancariaId;
    }
    const data = await fetchApi<any>(`/TransacoesConta/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...dto, id }),
    });
    return mapTransacaoConta(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/TransacoesConta/${id}`, {
      method: 'DELETE',
    }),
};

// ==================== CONTAS FIXAS ====================
export const contasFixasApi = {
  getAll: async () => {
    const data = await fetchApi<any[]>('/ContasFixas');
    return data.map(mapContaFixa);
  },
  
  getById: async (id: string) => {
    const data = await fetchApi<any>(`/ContasFixas/${id}`);
    return mapContaFixa(data);
  },
  
  create: async (conta: Omit<ContaFixa, 'id'>) => {
    const data = await fetchApi<any>('/ContasFixas', {
      method: 'POST',
      body: JSON.stringify(conta),
    });
    return mapContaFixa(data);
  },
  
  update: async (id: string, conta: Partial<ContaFixa>) => {
    const data = await fetchApi<any>(`/ContasFixas/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ ...conta, id }),
    });
    return mapContaFixa(data);
  },
  
  delete: (id: string) =>
    fetchApi<void>(`/ContasFixas/${id}`, {
      method: 'DELETE',
    }),
};
