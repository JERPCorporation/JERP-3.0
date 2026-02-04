'use client';

import { Play, CheckCircle, Book, Award } from 'lucide-react';
import { useState } from 'react';

const courses = [
  {
    id: 1,
    title: 'Introducción a JERP',
    duration: '5 min',
    completed: true,
    lessons: [
      'Visión general del producto',
      'Características principales',
      'Precios y planes',
    ],
  },
  {
    id: 2,
    title: 'Técnicas de Venta',
    duration: '15 min',
    completed: false,
    lessons: [
      'Identificación de prospectos',
      'Manejo de objeciones',
      'Cierre de ventas',
      'Seguimiento efectivo',
    ],
  },
  {
    id: 3,
    title: 'Posicionamiento Competitivo',
    duration: '10 min',
    completed: false,
    lessons: [
      'JERP vs. QuickBooks',
      'JERP vs. Gusto',
      'Ventajas únicas de JERP',
      'Casos de uso ideales',
    ],
  },
  {
    id: 4,
    title: 'Mejores Prácticas',
    duration: '12 min',
    completed: false,
    lessons: [
      'Estrategias de socios exitosos',
      'Maximizando comisiones',
      'Networking efectivo',
      'Construyendo relaciones a largo plazo',
    ],
  },
];

const faqs = [
  {
    question: '¿Cuándo recibiré mis comisiones?',
    answer: 'Las comisiones se procesan el día 15 de cada mes para todas las comisiones aprobadas del mes anterior. Necesitas alcanzar un mínimo de $50 para solicitar un pago.',
  },
  {
    question: '¿Qué pasa si un cliente cancela?',
    answer: 'Si un cliente cancela dentro de los primeros 30 días, el bono de registro se revertirá. Las comisiones recurrentes se detienen cuando el cliente cancela su suscripción.',
  },
  {
    question: '¿Puedo referir a empresas grandes?',
    answer: 'Sí, puedes referir empresas de cualquier tamaño. Para empresas con más de 100 empleados, contacta a tu gerente de socios para negociar comisiones personalizadas.',
  },
  {
    question: '¿Cómo funciona el sistema de niveles?',
    answer: 'Los niveles se actualizan automáticamente cada mes basándose en tu número de clientes activos. Tu tasa de comisión aumenta automáticamente cuando alcanzas un nuevo nivel.',
  },
];

export default function AcademyPage() {
  const [expandedFAQ, setExpandedFAQ] = useState<number | null>(null);

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">Academia de Socios</h1>
        <p className="text-slate-400">
          Aprende todo lo necesario para ser un socio exitoso de JERP
        </p>
      </div>

      {/* Progress Overview */}
      <div className="bg-gradient-to-r from-red-500/20 to-orange-500/20 border border-red-500/30 rounded-xl p-6 mb-8">
        <div className="flex items-center justify-between mb-4">
          <div>
            <h2 className="text-2xl font-bold text-white mb-1">Tu Progreso</h2>
            <p className="text-slate-300">
              {courses.filter(c => c.completed).length} de {courses.length} cursos completados
            </p>
          </div>
          <Award className="w-16 h-16 text-yellow-400" />
        </div>
        <div className="w-full bg-slate-700 rounded-full h-3">
          <div
            className="bg-gradient-to-r from-red-500 to-orange-500 h-3 rounded-full transition-all"
            style={{
              width: `${(courses.filter(c => c.completed).length / courses.length) * 100}%`,
            }}
          />
        </div>
      </div>

      {/* Courses */}
      <div className="mb-8">
        <h2 className="text-2xl font-bold text-white mb-6">Cursos de Capacitación</h2>
        <div className="space-y-4">
          {courses.map((course) => (
            <div
              key={course.id}
              className="bg-slate-800/50 border border-slate-700 rounded-xl p-6"
            >
              <div className="flex items-start justify-between mb-4">
                <div className="flex items-start gap-4">
                  <div
                    className={`p-3 rounded-lg ${
                      course.completed
                        ? 'bg-green-500/20 text-green-400'
                        : 'bg-red-500/20 text-red-400'
                    }`}
                  >
                    {course.completed ? (
                      <CheckCircle className="w-6 h-6" />
                    ) : (
                      <Play className="w-6 h-6" />
                    )}
                  </div>
                  <div>
                    <h3 className="text-xl font-semibold text-white mb-1">
                      {course.title}
                    </h3>
                    <p className="text-slate-400 text-sm">{course.duration}</p>
                  </div>
                </div>
                <button className="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-all">
                  {course.completed ? 'Revisar' : 'Comenzar'}
                </button>
              </div>
              <ul className="space-y-2 ml-16">
                {course.lessons.map((lesson, idx) => (
                  <li key={idx} className="text-slate-300 text-sm flex items-center gap-2">
                    <span className="w-1.5 h-1.5 bg-red-500 rounded-full" />
                    {lesson}
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>

      {/* FAQs */}
      <div>
        <h2 className="text-2xl font-bold text-white mb-6">Preguntas Frecuentes</h2>
        <div className="space-y-4">
          {faqs.map((faq, idx) => (
            <div
              key={idx}
              className="bg-slate-800/50 border border-slate-700 rounded-xl overflow-hidden"
            >
              <button
                onClick={() => setExpandedFAQ(expandedFAQ === idx ? null : idx)}
                className="w-full flex items-center justify-between p-6 text-left hover:bg-slate-800/30 transition-all"
              >
                <div className="flex items-center gap-3">
                  <Book className="w-5 h-5 text-red-400 flex-shrink-0" />
                  <h3 className="text-white font-medium">{faq.question}</h3>
                </div>
                <span className="text-slate-400 text-2xl">
                  {expandedFAQ === idx ? '−' : '+'}
                </span>
              </button>
              {expandedFAQ === idx && (
                <div className="px-6 pb-6">
                  <p className="text-slate-300 ml-8">{faq.answer}</p>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>

      {/* Support */}
      <div className="bg-blue-500/10 border border-blue-500/20 rounded-xl p-6 mt-8">
        <h3 className="text-lg font-semibold text-blue-400 mb-2">
          ¿Necesitas más ayuda?
        </h3>
        <p className="text-slate-300 mb-4">
          Nuestro equipo de soporte está disponible para ayudarte con cualquier
          pregunta o necesidad especial.
        </p>
        <button className="px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition-all">
          Contactar Soporte de Socios
        </button>
      </div>
    </div>
  );
}
