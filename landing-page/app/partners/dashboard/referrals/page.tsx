'use client';

import { useEffect, useState } from 'react';
import { Search, Filter } from 'lucide-react';

interface Referral {
  id: string;
  customerEmail: string;
  customerName?: string;
  plan?: string;
  status: string;
  mrr: number;
  signupDate?: string;
  createdAt: string;
}

export default function ReferralsPage() {
  const [referrals, setReferrals] = useState<Referral[]>([]);
  const [loading, setLoading] = useState(true);
  const [statusFilter, setStatusFilter] = useState<string>('');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    fetchReferrals();
  }, [statusFilter]);

  const fetchReferrals = async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams();
      if (statusFilter) params.append('status', statusFilter);
      
      const response = await fetch(`/api/partners/referrals?${params}`);
      const data = await response.json();
      
      if (response.ok) {
        setReferrals(data.referrals || []);
      }
    } catch (error) {
      console.error('Error fetching referrals:', error);
    } finally {
      setLoading(false);
    }
  };

  const filteredReferrals = referrals.filter(ref =>
    ref.customerEmail.toLowerCase().includes(searchTerm.toLowerCase()) ||
    ref.customerName?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'ACTIVE':
        return 'bg-green-500/20 text-green-400 border-green-500/30';
      case 'TRIAL':
        return 'bg-yellow-500/20 text-yellow-400 border-yellow-500/30';
      case 'CANCELLED':
        return 'bg-red-500/20 text-red-400 border-red-500/30';
      case 'LEAD':
        return 'bg-blue-500/20 text-blue-400 border-blue-500/30';
      default:
        return 'bg-slate-500/20 text-slate-400 border-slate-500/30';
    }
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">Referidos</h1>
        <p className="text-slate-400">Gestiona y rastrea todos tus referidos</p>
      </div>

      {/* Filters */}
      <div className="flex flex-wrap gap-4 mb-6">
        <div className="flex-1 min-w-[200px]">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-slate-400 w-5 h-5" />
            <input
              type="text"
              placeholder="Buscar por email o nombre..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 bg-slate-800 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
            />
          </div>
        </div>

        <div className="flex gap-2">
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="px-4 py-2 bg-slate-800 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
          >
            <option value="">Todos los estados</option>
            <option value="LEAD">Lead</option>
            <option value="TRIAL">Trial</option>
            <option value="ACTIVE">Activo</option>
            <option value="CANCELLED">Cancelado</option>
          </select>
        </div>
      </div>

      {/* Referrals Table */}
      <div className="bg-slate-800/50 border border-slate-700 rounded-xl overflow-hidden">
        {loading ? (
          <div className="p-8 text-center text-slate-400">Cargando...</div>
        ) : filteredReferrals.length === 0 ? (
          <div className="p-8 text-center text-slate-400">
            No se encontraron referidos. ¡Comienza a compartir tu enlace!
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-slate-900/50 border-b border-slate-700">
                <tr>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Cliente
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Estado
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Plan
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    MRR
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Fecha
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-700">
                {filteredReferrals.map((referral) => (
                  <tr key={referral.id} className="hover:bg-slate-800/30">
                    <td className="px-6 py-4">
                      <div className="text-white font-medium">{referral.customerEmail}</div>
                      {referral.customerName && (
                        <div className="text-sm text-slate-400">{referral.customerName}</div>
                      )}
                    </td>
                    <td className="px-6 py-4">
                      <span className={`px-3 py-1 rounded-full text-xs font-medium border ${getStatusColor(referral.status)}`}>
                        {referral.status}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-white">
                      {referral.plan || '-'}
                    </td>
                    <td className="px-6 py-4 text-white font-mono">
                      ${referral.mrr.toFixed(2)}
                    </td>
                    <td className="px-6 py-4 text-slate-400 text-sm">
                      {new Date(referral.createdAt).toLocaleDateString()}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
