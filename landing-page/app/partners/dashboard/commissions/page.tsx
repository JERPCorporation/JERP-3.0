'use client';

import { useEffect, useState } from 'react';
import { Download } from 'lucide-react';

interface Commission {
  id: string;
  type: string;
  amount: number;
  status: string;
  periodStart: string;
  periodEnd: string;
  referral: {
    customerEmail: string;
    customerName?: string;
    plan?: string;
  };
  createdAt: string;
}

export default function CommissionsPage() {
  const [commissions, setCommissions] = useState<Commission[]>([]);
  const [loading, setLoading] = useState(true);
  const [typeFilter, setTypeFilter] = useState<string>('');
  const [statusFilter, setStatusFilter] = useState<string>('');

  useEffect(() => {
    fetchCommissions();
  }, [typeFilter, statusFilter]);

  const fetchCommissions = async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams();
      if (typeFilter) params.append('type', typeFilter);
      if (statusFilter) params.append('status', statusFilter);
      
      const response = await fetch(`/api/partners/commissions?${params}`);
      const data = await response.json();
      
      if (response.ok) {
        setCommissions(data.commissions || []);
      }
    } catch (error) {
      console.error('Error fetching commissions:', error);
    } finally {
      setLoading(false);
    }
  };

  const exportToCSV = () => {
    const headers = ['Tipo', 'Cliente', 'Monto', 'Estado', 'Período', 'Fecha'];
    const rows = commissions.map(c => [
      c.type,
      c.referral.customerEmail,
      c.amount.toFixed(2),
      c.status,
      `${new Date(c.periodStart).toLocaleDateString()} - ${new Date(c.periodEnd).toLocaleDateString()}`,
      new Date(c.createdAt).toLocaleDateString(),
    ]);

    const csvContent = [
      headers.join(','),
      ...rows.map(row => row.join(','))
    ].join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `commissions-${Date.now()}.csv`;
    a.click();
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'PAID':
        return 'bg-green-500/20 text-green-400 border-green-500/30';
      case 'APPROVED':
        return 'bg-blue-500/20 text-blue-400 border-blue-500/30';
      case 'PENDING':
        return 'bg-yellow-500/20 text-yellow-400 border-yellow-500/30';
      case 'REVERSED':
        return 'bg-red-500/20 text-red-400 border-red-500/30';
      default:
        return 'bg-slate-500/20 text-slate-400 border-slate-500/30';
    }
  };

  const totalCommissions = commissions.reduce((sum, c) => sum + c.amount, 0);
  const pendingTotal = commissions.filter(c => c.status === 'PENDING').reduce((sum, c) => sum + c.amount, 0);
  const approvedTotal = commissions.filter(c => c.status === 'APPROVED').reduce((sum, c) => sum + c.amount, 0);
  const paidTotal = commissions.filter(c => c.status === 'PAID').reduce((sum, c) => sum + c.amount, 0);

  return (
    <div>
      <div className="mb-8 flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-white mb-2">Comisiones</h1>
          <p className="text-slate-400">Revisa todas tus comisiones y pagos</p>
        </div>
        <button
          onClick={exportToCSV}
          disabled={commissions.length === 0}
          className="flex items-center gap-2 px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
        >
          <Download className="w-4 h-4" />
          Exportar CSV
        </button>
      </div>

      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h3 className="text-slate-400 text-sm mb-2">Total</h3>
          <p className="text-2xl font-bold text-white">${totalCommissions.toFixed(2)}</p>
        </div>
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h3 className="text-slate-400 text-sm mb-2">Pendiente</h3>
          <p className="text-2xl font-bold text-yellow-400">${pendingTotal.toFixed(2)}</p>
        </div>
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h3 className="text-slate-400 text-sm mb-2">Aprobado</h3>
          <p className="text-2xl font-bold text-blue-400">${approvedTotal.toFixed(2)}</p>
        </div>
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h3 className="text-slate-400 text-sm mb-2">Pagado</h3>
          <p className="text-2xl font-bold text-green-400">${paidTotal.toFixed(2)}</p>
        </div>
      </div>

      {/* Filters */}
      <div className="flex gap-4 mb-6">
        <select
          value={typeFilter}
          onChange={(e) => setTypeFilter(e.target.value)}
          className="px-4 py-2 bg-slate-800 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
        >
          <option value="">Todos los tipos</option>
          <option value="SIGNUP_BONUS">Bono de Registro</option>
          <option value="RECURRING">Recurrente</option>
          <option value="TIER_BONUS">Bono de Nivel</option>
        </select>

        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="px-4 py-2 bg-slate-800 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
        >
          <option value="">Todos los estados</option>
          <option value="PENDING">Pendiente</option>
          <option value="APPROVED">Aprobado</option>
          <option value="PAID">Pagado</option>
          <option value="REVERSED">Revertido</option>
        </select>
      </div>

      {/* Commissions Table */}
      <div className="bg-slate-800/50 border border-slate-700 rounded-xl overflow-hidden">
        {loading ? (
          <div className="p-8 text-center text-slate-400">Cargando...</div>
        ) : commissions.length === 0 ? (
          <div className="p-8 text-center text-slate-400">
            No tienes comisiones aún.
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-slate-900/50 border-b border-slate-700">
                <tr>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Tipo
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Cliente
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Monto
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Estado
                  </th>
                  <th className="px-6 py-4 text-left text-sm font-semibold text-slate-300">
                    Período
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-700">
                {commissions.map((commission) => (
                  <tr key={commission.id} className="hover:bg-slate-800/30">
                    <td className="px-6 py-4 text-white">
                      {commission.type === 'SIGNUP_BONUS' ? 'Bono Registro' :
                       commission.type === 'RECURRING' ? 'Recurrente' :
                       'Bono Nivel'}
                    </td>
                    <td className="px-6 py-4">
                      <div className="text-white">{commission.referral.customerEmail}</div>
                      {commission.referral.plan && (
                        <div className="text-sm text-slate-400">{commission.referral.plan}</div>
                      )}
                    </td>
                    <td className="px-6 py-4 text-white font-mono font-semibold">
                      ${commission.amount.toFixed(2)}
                    </td>
                    <td className="px-6 py-4">
                      <span className={`px-3 py-1 rounded-full text-xs font-medium border ${getStatusColor(commission.status)}`}>
                        {commission.status}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-slate-400 text-sm">
                      {new Date(commission.periodStart).toLocaleDateString()} - {new Date(commission.periodEnd).toLocaleDateString()}
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
