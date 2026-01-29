
import React, { useState } from 'react';
import { ICONS } from '../constants';
import { loginUser } from '../services/authService';

interface LoginProps {
  onLogin: (userData: any) => void;
  onRegisterClick: () => void;
  onForgotPasswordClick: () => void;
}

// Timestamp para forçar atualização: 2026-01-28 23:45
const Login: React.FC<LoginProps> = ({ onLogin, onRegisterClick, onForgotPasswordClick }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  console.log('Login component renderizado - Props:', { onRegisterClick, onForgotPasswordClick });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const userData = await loginUser(email, password);
      localStorage.setItem('user', JSON.stringify(userData));
      onLogin(userData);
    } catch (err: any) {
      setError(err.message || 'Erro ao fazer login. Tente novamente.');
    } finally {
      setLoading(false);
    }
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
            <p className="text-slate-500 font-medium mt-1">Bem-vindo de volta!</p>
          </div>

          {error && (
            <div className="mb-6 p-4 bg-red-50 border border-red-200 rounded-2xl">
              <p className="text-sm font-bold text-red-600 text-center">{error}</p>
            </div>
          )}

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

            {true && (
              <div className="flex justify-end">
                <button 
                  type="button" 
                  onClick={() => {
                    console.log('Botão Esqueceu a senha clicado!');
                    onForgotPasswordClick();
                  }}
                  className="text-xs font-bold text-indigo-600 hover:text-indigo-800 transition-colors"
                >
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
                'Entrar no Sistema'
              )}
            </button>
          </form>

          <div className="mt-8 text-center">
            <button 
              type="button"
              onClick={() => {
                console.log('Botão Cadastre-se clicado!');
                onRegisterClick();
              }}
              className="text-sm font-bold text-slate-500 hover:text-indigo-600 transition-colors"
            >
              Não tem uma conta? <span className="underline">Cadastre-se</span>
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
