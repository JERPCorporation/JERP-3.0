import Hero from '@/components/Hero';
import Features from '@/components/Features';
import Demo from '@/components/Demo';
import Pricing from '@/components/Pricing';
import SocialProof from '@/components/SocialProof';
import FAQ from '@/components/FAQ';
import Footer from '@/components/Footer';

export default function Home() {
  return (
    <main className="min-h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900">
      <Hero />
      <Demo />
      <Features />
      <SocialProof />
      <Pricing />
      <FAQ />
      <Footer />
    </main>
  );
}
