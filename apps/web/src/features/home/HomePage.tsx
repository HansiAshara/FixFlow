import React from 'react'
import { Link } from 'react-router-dom'
import { Button } from '../../shared/components/Button'
import { Card } from '../../shared/components/Card'
import { Sparkles, ShieldCheck, Activity, Cpu, ArrowRight } from 'lucide-react'

export const HomePage: React.FC = () => {
  return (
    <div className="space-y-12 py-8">
      {/* Hero Section */}
      <div className="text-center max-w-3xl mx-auto space-y-4">
        <div className="inline-flex items-center space-x-2 px-3 py-1 rounded-full bg-blue-950/60 border border-blue-800/80 text-blue-300 text-xs font-medium">
          <Sparkles className="h-3.5 w-3.5 text-blue-400" />
          <span>Intelligent Issue Reporting & Technician Matching</span>
        </div>
        <h1 className="text-4xl sm:text-5xl font-extrabold text-white tracking-tight leading-tight">
          Maintenance Problems Solved with <span className="text-blue-500">Intelligent Precision</span>
        </h1>
        <p className="text-base text-gray-400 leading-relaxed max-w-2xl mx-auto">
          Report maintenance issues in natural language. Local AI analyzes categories and skills, while deterministic algorithms rank verified technicians based on real performance history.
        </p>
        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 pt-4">
          <Link to="/issues/new">
            <Button size="lg" variant="primary" className="w-full sm:w-auto flex items-center space-x-2">
              <span>Report an Issue</span>
              <ArrowRight className="h-4 w-4" />
            </Button>
          </Link>
          <Link to="/technicians">
            <Button size="lg" variant="secondary" className="w-full sm:w-auto">
              Browse Technicians
            </Button>
          </Link>
        </div>
      </div>

      {/* Feature Cards Grid */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card glass className="space-y-3">
          <div className="h-10 w-10 rounded-lg bg-blue-950 flex items-center justify-center text-blue-400">
            <Cpu className="h-5 w-5" />
          </div>
          <h3 className="text-lg font-bold text-white">AI-Powered Parsing</h3>
          <p className="text-xs text-gray-400 leading-relaxed">
            Asynchronous background workers using Ollama extract required skills, severity, risk levels, and tags from natural language reports.
          </p>
        </Card>

        <Card glass className="space-y-3">
          <div className="h-10 w-10 rounded-lg bg-emerald-950 flex items-center justify-center text-emerald-400">
            <ShieldCheck className="h-5 w-5" />
          </div>
          <h3 className="text-lg font-bold text-white">Human-Verified Quality</h3>
          <p className="text-xs text-gray-400 leading-relaxed">
            Formal trade certifications, qualifications, and experience portfolios are reviewed with strict human-in-the-loop admin governance.
          </p>
        </Card>

        <Card glass className="space-y-3">
          <div className="h-10 w-10 rounded-lg bg-purple-950 flex items-center justify-center text-purple-400">
            <Activity className="h-5 w-5" />
          </div>
          <h3 className="text-lg font-bold text-white">Performance Feedback Loop</h3>
          <p className="text-xs text-gray-400 leading-relaxed">
            Response rate, resolution reliability, and verified customer reviews directly feed into future technician recommendation scores.
          </p>
        </Card>
      </div>
    </div>
  )
}
