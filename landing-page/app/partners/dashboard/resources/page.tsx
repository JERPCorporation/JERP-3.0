'use client';

import { Download, FileText, Image, Video, Mail, Share2 } from 'lucide-react';

const resources = [
  {
    category: 'Marca & Logos',
    icon: Image,
    items: [
      { name: 'JERP Logo (SVG)', format: 'SVG', size: '2 KB' },
      { name: 'JERP Logo (PNG - Alta resolución)', format: 'PNG', size: '45 KB' },
      { name: 'Guía de marca', format: 'PDF', size: '1.2 MB' },
    ],
  },
  {
    category: 'Materiales de Marketing',
    icon: FileText,
    items: [
      { name: 'Brochure del producto', format: 'PDF', size: '2.5 MB' },
      { name: 'Comparación con competidores', format: 'PDF', size: '890 KB' },
      { name: 'Casos de éxito', format: 'PDF', size: '1.8 MB' },
      { name: 'Hoja de especificaciones técnicas', format: 'PDF', size: '450 KB' },
    ],
  },
  {
    category: 'Plantillas de Email',
    icon: Mail,
    items: [
      { name: 'Email de introducción', format: 'HTML', size: '15 KB' },
      { name: 'Seguimiento post-demo', format: 'HTML', size: '12 KB' },
      { name: 'Recordatorio de trial', format: 'HTML', size: '10 KB' },
      { name: 'Email de renovación', format: 'HTML', size: '14 KB' },
    ],
  },
  {
    category: 'Redes Sociales',
    icon: Share2,
    items: [
      { name: 'Posts para LinkedIn (5 variantes)', format: 'ZIP', size: '2.1 MB' },
      { name: 'Stories para Instagram', format: 'ZIP', size: '4.5 MB' },
      { name: 'Banners para Facebook', format: 'ZIP', size: '3.2 MB' },
      { name: 'Tweets pre-escritos', format: 'TXT', size: '5 KB' },
    ],
  },
  {
    category: 'Videos & Demos',
    icon: Video,
    items: [
      { name: 'Video demo del producto (5 min)', format: 'MP4', size: '45 MB' },
      { name: 'Tutorial rápido', format: 'MP4', size: '12 MB' },
      { name: 'Testimonios de clientes', format: 'MP4', size: '28 MB' },
    ],
  },
];

export default function ResourcesPage() {
  const handleDownload = (resourceName: string) => {
    // In production, this would trigger actual file download
    alert(`Descargando: ${resourceName}`);
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">Recursos de Marketing</h1>
        <p className="text-slate-400">
          Materiales listos para ayudarte a promocionar JERP
        </p>
      </div>

      <div className="space-y-6">
        {resources.map((category, idx) => (
          <div key={idx} className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
            <div className="flex items-center gap-3 mb-4">
              <category.icon className="w-6 h-6 text-red-500" />
              <h2 className="text-xl font-semibold text-white">{category.category}</h2>
            </div>

            <div className="grid md:grid-cols-2 gap-4">
              {category.items.map((item, itemIdx) => (
                <div
                  key={itemIdx}
                  className="flex items-center justify-between p-4 bg-slate-900/50 rounded-lg hover:bg-slate-900 transition-all"
                >
                  <div className="flex-1">
                    <h3 className="text-white font-medium mb-1">{item.name}</h3>
                    <p className="text-sm text-slate-400">
                      {item.format} • {item.size}
                    </p>
                  </div>
                  <button
                    onClick={() => handleDownload(item.name)}
                    className="p-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all"
                  >
                    <Download className="w-5 h-5" />
                  </button>
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>

      {/* ROI Calculator Widget */}
      <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6 mt-6">
        <h2 className="text-xl font-semibold text-white mb-4">
          Widget de Calculadora ROI
        </h2>
        <p className="text-slate-300 mb-4">
          Embebe esta calculadora interactiva en tu sitio web para ayudar a tus
          clientes a calcular el ahorro que obtendrán con JERP.
        </p>
        <div className="bg-slate-900 border border-slate-600 rounded-lg p-4 font-mono text-sm text-slate-300 overflow-x-auto">
          <code>
            {`<iframe src="${typeof window !== 'undefined' ? window.location.origin : ''}/calculator?ref=YOUR_CODE" width="100%" height="600" frameborder="0"></iframe>`}
          </code>
        </div>
        <button className="mt-4 px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all">
          Copiar Código
        </button>
      </div>

      {/* Usage Tips */}
      <div className="bg-blue-500/10 border border-blue-500/20 rounded-xl p-6 mt-6">
        <h3 className="text-lg font-semibold text-blue-400 mb-2">📚 Guía de Uso</h3>
        <ul className="text-slate-300 space-y-2 text-sm">
          <li>• Todos los materiales están pre-aprobados y listos para usar</li>
          <li>• Personaliza los emails con tu información de contacto</li>
          <li>• Usa las imágenes en alta resolución para impresiones</li>
          <li>• Los videos se pueden compartir directamente o embeber</li>
          <li>• Contacta a tu gerente de socios para materiales personalizados</li>
        </ul>
      </div>
    </div>
  );
}
