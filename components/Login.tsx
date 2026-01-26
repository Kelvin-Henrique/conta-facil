
import React, { useState } from 'react';
import { ICONS } from '../constants';

interface LoginProps {
  onLogin: (email: string) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  const [isLogin, setIsLogin] = useState(true);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    // Simulação de delay de rede
    setTimeout(() => {
      onLogin(email);
      setLoading(false);
    }, 800);
  };

  return (
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-4">
      <div className="w-full max-w-md bg-white rounded-[2.5rem] shadow-2xl shadow-indigo-100/50 border border-slate-100 overflow-hidden animate-fadeIn">
        <div className="p-10">
          <div className="flex justify-center mb-8">
            <div className="bg-indigo-600 p-4 rounded-3xl shadow-lg shadow-indigo-200">
              <ICONS.Wallet className="w-10 h-10 text-white" />
            </div>
          </div>
          
          <div className="text-center mb-10">
            <h1 className="text-3xl font-black text-slate-800 tracking-tight">Conta Fácil</h1>
            <p className="text-slate-500 font-medium mt-1">
              {isLogin ? 'Bem-vindo de volta!' : 'Crie sua conta gratuita'}
            </p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">E-mail</label>
              <input
                required
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold placeholder:text-slate-400 transition-all"
                placeholder="seu@email.com"
              />
            </div>

            <div>
              <label className="block text-sm font-bold text-slate-700 mb-2 ml-1">Senha</label>
              <input
                required
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 outline-none text-slate-900 font-bold placeholder:text-slate-400 transition-all"
                placeholder="••••••••"
              />
            </div>

            {isLogin && (
              <div className="flex justify-end">
                <button type="button" className="text-xs font-bold text-indigo-600 hover:text-indigo-800 transition-colors">
                  Esqueceu a senha?
                </button>
              </div>
            )}

            <button
              disabled={loading}
              type="submit"
              className="w-full bg-indigo-600 text-white py-4 rounded-2xl font-bold text-lg shadow-xl shadow-indigo-100 hover:bg-indigo-700 transition-all active:scale-[0.98] flex items-center justify-center gap-2"
            >
              {loading ? (
                <div className="w-6 h-6 border-4 border-white/30 border-t-white rounded-full animate-spin"></div>
              ) : (
                isLogin ? 'Entrar no Sistema' : 'Criar minha Conta'
              )}
            </button>
          </form>

          <div className="mt-8 text-center">
            <button 
              onClick={() => setIsLogin(!isLogin)}
              className="text-sm font-bold text-slate-500 hover:text-indigo-600 transition-colors"
            >
              {isLogin ? 'Não tem uma conta? Cadastre-se' : 'Já possui conta? Faça login'}
            </button>
          </div>
        </div>
        
        <div className="bg-slate-50 p-6 text-center border-t border-slate-100">
          <p className="text-[10px] text-slate-400 font-bold uppercase tracking-widest">
            Conta Fácil © 2024 • Segurança de Nível Bancário
          </p>
        </div>
      </div>
    </div>
  );
};

export default Login;
