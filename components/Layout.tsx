
import React from 'react';
import { ICONS } from '../constants';

interface LayoutProps {
  children: React.ReactNode;
  activeTab: string;
  setActiveTab: (tab: string) => void;
  user: string;
  onLogout: () => void;
}

const Layout: React.FC<LayoutProps> = ({ children, activeTab, setActiveTab, user, onLogout }) => {
  const menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: ICONS.LayoutDashboard },
    { id: 'fixed-bills', label: 'Contas Fixas', icon: ICONS.Calendar },
    { id: 'transactions', label: 'Lançamentos', icon: ICONS.Receipt },
    { id: 'accounts', label: 'Contas Bancárias', icon: ICONS.Wallet },
    { id: 'cards', label: 'Cartões de Crédito', icon: ICONS.CreditCard },
  ];

  return (
    <div className="flex min-h-screen bg-slate-50">
      {/* Desktop Sidebar */}
      <aside className="w-64 bg-white border-r border-slate-200 hidden md:flex flex-col">
        <div className="p-6">
          <h1 className="text-2xl font-bold text-indigo-600 flex items-center gap-2">
            <ICONS.Wallet className="w-8 h-8" />
            Conta Fácil
          </h1>
        </div>
        
        <div className="px-6 py-2">
          <div className="bg-slate-50 p-3 rounded-2xl border border-slate-100 flex items-center gap-3">
            <div className="w-8 h-8 bg-indigo-100 rounded-xl flex items-center justify-center text-indigo-600 font-bold text-xs">
              {user.charAt(0).toUpperCase()}
            </div>
            <div className="overflow-hidden">
              <p className="text-[10px] font-bold text-slate-400 uppercase tracking-tighter leading-none">Usuário</p>
              <p className="text-xs font-bold text-slate-700 truncate">{user}</p>
            </div>
          </div>
        </div>

        <nav className="flex-1 px-4 space-y-2 mt-4">
          {menuItems.map((item) => (
            <button
              key={item.id}
              onClick={() => setActiveTab(item.id)}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all ${
                activeTab === item.id
                  ? 'bg-indigo-50 text-indigo-700 font-semibold shadow-sm'
                  : 'text-slate-600 hover:bg-slate-100'
              }`}
            >
              <item.icon className={`w-5 h-5 ${activeTab === item.id ? 'text-indigo-600' : ''}`} />
              {item.label}
            </button>
          ))}
        </nav>

        <div className="p-4 border-t border-slate-100">
          <button
            onClick={onLogout}
            className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-rose-500 hover:bg-rose-50 transition-all font-bold"
          >
            <ICONS.LogOut className="w-5 h-5" />
            Sair
          </button>
        </div>
      </aside>

      {/* Mobile Nav Top Bar */}
      <div className="md:hidden fixed top-0 left-0 right-0 bg-white border-b border-slate-200 p-4 flex justify-between items-center z-50">
          <h1 className="text-xl font-bold text-indigo-600">Conta Fácil</h1>
          <button onClick={onLogout} className="text-rose-500 p-2">
            <ICONS.LogOut className="w-6 h-6" />
          </button>
      </div>

      {/* Mobile Nav Bottom */}
      <div className="md:hidden fixed bottom-0 left-0 right-0 bg-white border-t border-slate-200 flex justify-around p-2 z-50">
        {menuItems.map((item) => (
          <button
            key={item.id}
            onClick={() => setActiveTab(item.id)}
            className={`flex flex-col items-center p-2 rounded-lg ${
              activeTab === item.id ? 'text-indigo-600' : 'text-slate-500'
            }`}
          >
            <item.icon className="w-6 h-6" />
            <span className="text-[10px] mt-1">{item.label}</span>
          </button>
        ))}
      </div>

      {/* Main Content */}
      <main className="flex-1 p-4 md:p-8 pt-20 md:pt-8 pb-24 md:pb-8 overflow-y-auto">
        <div className="max-w-5xl mx-auto">
          {children}
        </div>
      </main>
    </div>
  );
};

export default Layout;
