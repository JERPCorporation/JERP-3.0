'use client';

import { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { ClientDetailModal} from '@/Components/ClientDetailModal';
interface Client {
  id: string;
  name: string;
  business: string;
  status: 'ok' | 'warning' | 'urgent';
  pendingReview: number;
  claudeSuggestions: string[];
  nextDeadline: string;
  monthlyFee: number;
}

interface ClaudeSuggestion {
  clientId: string;
  type: 'warning' | 'tip' | 'error';
  message: string;
  confidence: number;
}

export default function AccountantDashboard() {
  const [clients, setClients] = useState<Client[]>([]);
  const [selectedClient, setSelectedClient] = useState<Client | null>(null);
  const [pendingApprovals, setPendingApprovals] = useState(0);

  useEffect(() => {
    fetchClients();
  }, []);

  const fetchClients = async () => {
    try {
    const response = await fetch('/api/accountant/clients');
    if (!response.ok) throw new Error(await response.text());
    const data = await response.json();
    setClients(data);
    setPendingApprovals(data.reduce((sum: number, c: Client) => sum + c.pendingReview, 0));
    } catch (error) {
      console.error('Failed to load clients',error);
      // show toast or UI msg to user
    }
  };

  const approveTransactions = async (clientId: string) => {
    await fetch(`/api/accountant/approve/${clientId}`, { method: 'POST' });
    fetchClients();
  };

  const requestClaudeAnalysis = async (clientId: string) => {
    const response = await fetch(`/api/accountant/claude-analysis/${clientId}`, {
      method: 'POST'
    });
    const analysis = await response.json();
    alert(analysis.summary);
  };

  return (
    <div className="p-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="mb-6">
        <h1 className="text-3xl font-bold">Panel del Contador</h1>
        <p className="text-gray-600">
          {pendingApprovals} transacciones pendientes de revisión
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-gray-600">
              Total Clientes
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold">{clients.length}</div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-gray-600">
              Pendientes Revisión
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold text-orange-500">
              {pendingApprovals}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-gray-600">
              Alertas Claude
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold text-red-500">
              {clients.reduce((sum, c) => sum + c.claudeSuggestions.length, 0)}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-gray-600">
              Ingresos Mensuales
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold text-green-500">
              Q{clients.reduce((sum, c) => sum + c.monthlyFee, 0).toLocaleString()}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Clients List */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {clients.map(client => (
          <Card 
            key={client.id}
            className={`cursor-pointer hover:shadow-lg transition-shadow ${
              client.status === 'urgent' ? 'border-red-500' :
              client.status === 'warning' ? 'border-orange-500' :
              'border-green-500'
            }`}
            onClick={() => setSelectedClient(client)}
          >
            <CardHeader>
              <div className="flex justify-between items-start">
                <div>
                  <CardTitle className="text-lg">{client.name}</CardTitle>
                  <p className="text-sm text-gray-600">{client.business}</p>
                </div>
                <Badge variant={
                  client.status === 'urgent' ? 'destructive' :
                  client.status === 'warning' ? 'secondary' :
                  'default'
                }>
                  {client.status === 'ok' ? '✓ Al día' :
                   client.status === 'warning' ? '⚠ Revisar' :
                   '🚨 Urgente'}
                </Badge>
              </div>
            </CardHeader>
            <CardContent>
              {/* Pending Reviews */}
              {client.pendingReview > 0 && (
                <div className="mb-3">
                  <Badge variant="outline" className="bg-orange-50">
                    {client.pendingReview} pendientes
                  </Badge>
                </div>
              )}

              {/* Claude Suggestions */}
              {client.claudeSuggestions.length > 0 && (
                <div className="space-y-1 mb-3">
                  {client.claudeSuggestions.slice(0, 2).map((suggestion, idx) => (
                    <p key={idx} className="text-xs text-gray-700 flex items-start">
                      <span className="mr-1">🤖</span>
                      {suggestion}
                    </p>
                  ))}
                  {client.claudeSuggestions.length > 2 && (
                    <p className="text-xs text-blue-600">
                      +{client.claudeSuggestions.length - 2} más...
                    </p>
                  )}
                </div>
              )}

              {/* Next Deadline */}
              <p className="text-xs text-gray-500">
                📅 Próximo: {client.nextDeadline}
              </p>

              {/* Actions */}
              <div className="mt-3 flex gap-2">
                <Button 
                  size="sm" 
                  onClick={(e) => {
                    e.stopPropagation();
                    approveTransactions(client.id);
                  }}
                  disabled={client.pendingReview === 0}
                >
                  Aprobar ({client.pendingReview})
                </Button>
                <Button 
                  size="sm" 
                  variant="outline"
                  onClick={(e) => {
                    e.stopPropagation();
                    requestClaudeAnalysis(client.id);
                  }}
                >
                  🤖 Analizar
                </Button>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Client Detail Modal */}
      {selectedClient && (
        <ClientDetailModal 
          client={selectedClient}
          onClose={() => setSelectedClient(null)}
          onApprove={() => {
            approveTransactions(selectedClient.id);
            setSelectedClient(null);
          }}
        />
      )}
    </div>
  );
}

function ClientDetailModal({ 
  client, 
  onClose, 
  onApprove 
}: { 
  client: Client; 
  onClose: () => void;
  onApprove: () => void;
}) {
  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <Card className="w-full max-w-2xl max-h-[90vh] overflow-y-auto">
        <CardHeader>
          <div className="flex justify-between items-start">
            <div>
              <CardTitle>{client.name}</CardTitle>
              <p className="text-sm text-gray-600">{client.business}</p>
            </div>
            <Button variant="ghost" onClick={onClose}>✕</Button>
          </div>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {/* Claude Suggestions */}
            <div>
              <h3 className="font-semibold mb-2">🤖 Sugerencias de Claude</h3>
              {client.claudeSuggestions.map((suggestion, idx) => (
                <div key={idx} className="bg-blue-50 p-3 rounded mb-2">
                  <p className="text-sm">{suggestion}</p>
                </div>
              ))}
            </div>

            {/* Pending Transactions */}
            <div>
              <h3 className="font-semibold mb-2">
                📝 Transacciones Pendientes ({client.pendingReview})
              </h3>
              {/* Transaction list would go here */}
            </div>

            {/* Actions */}
            <div className="flex gap-2">
              <Button onClick={onApprove} className="flex-1">
                ✓ Aprobar Todo
              </Button>
              <Button variant="outline" onClick={onClose} className="flex-1">
                Revisar Individualmente
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
