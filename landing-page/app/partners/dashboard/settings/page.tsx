'use client';

import { useState } from 'react';
import { User, Building, CreditCard, Lock } from 'lucide-react';

export default function SettingsPage() {
  const [activeTab, setActiveTab] = useState('profile');

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">Configuración</h1>
        <p className="text-slate-400">Gestiona tu cuenta y preferencias</p>
      </div>

      {/* Tabs */}
      <div className="flex gap-2 mb-6 border-b border-slate-700">
        <button
          onClick={() => setActiveTab('profile')}
          className={`px-4 py-3 font-medium transition-all ${
            activeTab === 'profile'
              ? 'text-red-500 border-b-2 border-red-500'
              : 'text-slate-400 hover:text-white'
          }`}
        >
          <div className="flex items-center gap-2">
            <User className="w-4 h-4" />
            Perfil
          </div>
        </button>
        <button
          onClick={() => setActiveTab('company')}
          className={`px-4 py-3 font-medium transition-all ${
            activeTab === 'company'
              ? 'text-red-500 border-b-2 border-red-500'
              : 'text-slate-400 hover:text-white'
          }`}
        >
          <div className="flex items-center gap-2">
            <Building className="w-4 h-4" />
            Empresa
          </div>
        </button>
        <button
          onClick={() => setActiveTab('payment')}
          className={`px-4 py-3 font-medium transition-all ${
            activeTab === 'payment'
              ? 'text-red-500 border-b-2 border-red-500'
              : 'text-slate-400 hover:text-white'
          }`}
        >
          <div className="flex items-center gap-2">
            <CreditCard className="w-4 h-4" />
            Pagos
          </div>
        </button>
        <button
          onClick={() => setActiveTab('security')}
          className={`px-4 py-3 font-medium transition-all ${
            activeTab === 'security'
              ? 'text-red-500 border-b-2 border-red-500'
              : 'text-slate-400 hover:text-white'
          }`}
        >
          <div className="flex items-center gap-2">
            <Lock className="w-4 h-4" />
            Seguridad
          </div>
        </button>
      </div>

      {/* Profile Tab */}
      {activeTab === 'profile' && (
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h2 className="text-xl font-semibold text-white mb-6">
            Información Personal
          </h2>
          <form className="space-y-6">
            <div className="grid md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-slate-300 mb-2">
                  Nombre
                </label>
                <input
                  type="text"
                  defaultValue="John"
                  className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300 mb-2">
                  Apellido
                </label>
                <input
                  type="text"
                  defaultValue="Doe"
                  className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
                />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Email
              </label>
              <input
                type="email"
                defaultValue="john@example.com"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Teléfono
              </label>
              <input
                type="tel"
                defaultValue="+1 234 567 8900"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <button
              type="submit"
              className="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all"
            >
              Guardar Cambios
            </button>
          </form>
        </div>
      )}

      {/* Company Tab */}
      {activeTab === 'company' && (
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h2 className="text-xl font-semibold text-white mb-6">
            Información de la Empresa
          </h2>
          <form className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Nombre de la Empresa
              </label>
              <input
                type="text"
                defaultValue="Acme Consulting"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Sitio Web
              </label>
              <input
                type="url"
                defaultValue="https://acmeconsulting.com"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                RUC / Tax ID
              </label>
              <input
                type="text"
                defaultValue="123456789"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <button
              type="submit"
              className="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all"
            >
              Guardar Cambios
            </button>
          </form>
        </div>
      )}

      {/* Payment Tab */}
      {activeTab === 'payment' && (
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h2 className="text-xl font-semibold text-white mb-6">
            Información de Pago
          </h2>
          <form className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Método de Pago Preferido
              </label>
              <select className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none">
                <option>Transferencia Bancaria</option>
                <option>PayPal</option>
                <option>Stripe</option>
                <option>Cheque</option>
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Mínimo para Pago
              </label>
              <input
                type="number"
                defaultValue="50"
                disabled
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-slate-500"
              />
              <p className="text-sm text-slate-400 mt-1">
                Monto mínimo requerido para solicitar un pago
              </p>
            </div>
            <div className="bg-slate-900 border border-slate-600 rounded-lg p-4">
              <h3 className="text-white font-medium mb-2">Información Bancaria</h3>
              <div className="space-y-3">
                <div>
                  <label className="block text-sm text-slate-400 mb-1">
                    Nombre del Banco
                  </label>
                  <input
                    type="text"
                    placeholder="Banco Nacional"
                    className="w-full px-3 py-2 bg-slate-800 border border-slate-700 rounded text-white text-sm focus:border-red-500 focus:outline-none"
                  />
                </div>
                <div>
                  <label className="block text-sm text-slate-400 mb-1">
                    Número de Cuenta
                  </label>
                  <input
                    type="text"
                    placeholder="1234567890"
                    className="w-full px-3 py-2 bg-slate-800 border border-slate-700 rounded text-white text-sm focus:border-red-500 focus:outline-none"
                  />
                </div>
              </div>
            </div>
            <button
              type="submit"
              className="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all"
            >
              Guardar Cambios
            </button>
          </form>
        </div>
      )}

      {/* Security Tab */}
      {activeTab === 'security' && (
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h2 className="text-xl font-semibold text-white mb-6">Seguridad</h2>
          <form className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Contraseña Actual
              </label>
              <input
                type="password"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Nueva Contraseña
              </label>
              <input
                type="password"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-2">
                Confirmar Nueva Contraseña
              </label>
              <input
                type="password"
                className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
              />
            </div>
            <button
              type="submit"
              className="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all"
            >
              Cambiar Contraseña
            </button>
          </form>
        </div>
      )}
    </div>
  );
}
