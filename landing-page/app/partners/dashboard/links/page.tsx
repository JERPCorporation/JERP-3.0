'use client';

import { useState, useEffect } from 'react';
import { Copy, Check, QrCode } from 'lucide-react';
import QRCodeLib from 'qrcode';

export default function LinksPage() {
  const [referralCode, setReferralCode] = useState('LOADING');
  const [baseURL, setBaseURL] = useState('');
  const [utmSource, setUtmSource] = useState('partner');
  const [utmMedium, setUtmMedium] = useState('referral');
  const [utmCampaign, setUtmCampaign] = useState('');
  const [copied, setCopied] = useState(false);
  const [qrCode, setQRCode] = useState('');
  const [showQR, setShowQR] = useState(false);

  useEffect(() => {
    // Fetch partner data (mocked for now)
    setReferralCode('ABC12345');
    setBaseURL(window.location.origin);
  }, []);

  const generateReferralURL = () => {
    const params = new URLSearchParams();
    params.append('ref', referralCode);
    if (utmSource) params.append('utm_source', utmSource);
    if (utmMedium) params.append('utm_medium', utmMedium);
    if (utmCampaign) params.append('utm_campaign', utmCampaign);
    
    return `${baseURL}/signup?${params.toString()}`;
  };

  const referralURL = generateReferralURL();

  const copyToClipboard = () => {
    navigator.clipboard.writeText(referralURL);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  const generateQRCode = async () => {
    try {
      const qr = await QRCodeLib.toDataURL(referralURL, {
        width: 300,
        margin: 2,
        color: {
          dark: '#000000',
          light: '#ffffff',
        },
      });
      setQRCode(qr);
      setShowQR(true);
    } catch (error) {
      console.error('Error generating QR code:', error);
    }
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">Generador de Enlaces</h1>
        <p className="text-slate-400">Crea enlaces de referido personalizados con tracking UTM</p>
      </div>

      {/* Referral Code */}
      <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6 mb-6">
        <h2 className="text-xl font-semibold text-white mb-4">Tu Código de Referido</h2>
        <div className="flex items-center gap-4">
          <div className="flex-1 bg-slate-900 border border-slate-600 rounded-lg px-6 py-4">
            <span className="text-3xl font-mono font-bold text-white">{referralCode}</span>
          </div>
        </div>
        <p className="text-sm text-slate-400 mt-2">
          Este es tu código único de referido. Úsalo en todos tus enlaces.
        </p>
      </div>

      {/* UTM Builder */}
      <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6 mb-6">
        <h2 className="text-xl font-semibold text-white mb-4">Constructor de Enlaces UTM</h2>
        
        <div className="grid md:grid-cols-3 gap-4 mb-6">
          <div>
            <label className="block text-sm font-medium text-slate-300 mb-2">
              UTM Source
            </label>
            <input
              type="text"
              value={utmSource}
              onChange={(e) => setUtmSource(e.target.value)}
              placeholder="partner"
              className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-slate-300 mb-2">
              UTM Medium
            </label>
            <input
              type="text"
              value={utmMedium}
              onChange={(e) => setUtmMedium(e.target.value)}
              placeholder="referral"
              className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-slate-300 mb-2">
              UTM Campaign
            </label>
            <input
              type="text"
              value={utmCampaign}
              onChange={(e) => setUtmCampaign(e.target.value)}
              placeholder="spring-2024"
              className="w-full px-4 py-2 bg-slate-900 border border-slate-600 rounded-lg text-white focus:border-red-500 focus:outline-none"
            />
          </div>
        </div>

        {/* Generated URL */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-slate-300 mb-2">
            Enlace Generado
          </label>
          <div className="flex gap-2">
            <input
              type="text"
              value={referralURL}
              readOnly
              className="flex-1 px-4 py-3 bg-slate-900 border border-slate-600 rounded-lg text-white font-mono text-sm"
            />
            <button
              onClick={copyToClipboard}
              className="px-6 py-3 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all flex items-center gap-2"
            >
              {copied ? (
                <>
                  <Check className="w-4 h-4" />
                  Copiado
                </>
              ) : (
                <>
                  <Copy className="w-4 h-4" />
                  Copiar
                </>
              )}
            </button>
          </div>
        </div>

        {/* QR Code */}
        <div className="flex gap-4">
          <button
            onClick={generateQRCode}
            className="px-6 py-3 bg-slate-700 text-white rounded-lg hover:bg-slate-600 transition-all flex items-center gap-2"
          >
            <QrCode className="w-4 h-4" />
            Generar Código QR
          </button>
        </div>
      </div>

      {/* QR Code Display */}
      {showQR && qrCode && (
        <div className="bg-slate-800/50 border border-slate-700 rounded-xl p-6">
          <h2 className="text-xl font-semibold text-white mb-4">Código QR</h2>
          <div className="flex flex-col items-center">
            <div className="bg-white p-4 rounded-lg mb-4">
              <img src={qrCode} alt="QR Code" className="w-64 h-64" />
            </div>
            <p className="text-slate-400 text-sm mb-4">
              Escanea este código QR para acceder a tu enlace de referido
            </p>
            <a
              href={qrCode}
              download={`jerp-referral-${referralCode}.png`}
              className="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all"
            >
              Descargar QR Code
            </a>
          </div>
        </div>
      )}

      {/* Tips */}
      <div className="bg-blue-500/10 border border-blue-500/20 rounded-xl p-6 mt-6">
        <h3 className="text-lg font-semibold text-blue-400 mb-2">💡 Consejos de Uso</h3>
        <ul className="text-slate-300 space-y-2 text-sm">
          <li>• Usa diferentes campañas UTM para rastrear el rendimiento de cada canal</li>
          <li>• Comparte el código QR en materiales impresos y presentaciones</li>
          <li>• Incluye tu enlace en la firma de tu email profesional</li>
          <li>• Crea enlaces específicos para cada red social (utm_source=facebook, twitter, etc.)</li>
        </ul>
      </div>
    </div>
  );
}
