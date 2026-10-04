import React from 'react'
import { Card } from '../../../shared/components/Card'
import { ShieldCheck, UserCheck, FileText } from 'lucide-react'

export const AdminPage: React.FC = () => {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-white tracking-tight">Admin Verification Portal</h1>
        <p className="text-sm text-gray-400">Strict human-in-the-loop verification of technician qualification evidence</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card glass>
          <div className="flex items-center space-x-3">
            <UserCheck className="h-8 w-8 text-amber-400" />
            <div>
              <div className="text-xl font-bold text-white">0</div>
              <div className="text-xs text-gray-400">Pending Reviews</div>
            </div>
          </div>
        </Card>
        <Card glass>
          <div className="flex items-center space-x-3">
            <ShieldCheck className="h-8 w-8 text-emerald-400" />
            <div>
              <div className="text-xl font-bold text-white">2</div>
              <div className="text-xs text-gray-400">Active Verified Techs</div>
            </div>
          </div>
        </Card>
        <Card glass>
          <div className="flex items-center space-x-3">
            <FileText className="h-8 w-8 text-blue-400" />
            <div>
              <div className="text-xl font-bold text-white">100%</div>
              <div className="text-xs text-gray-400">Audit Compliance</div>
            </div>
          </div>
        </Card>
      </div>

      <Card glass>
        <h2 className="text-base font-semibold text-white mb-3">Verification Rule Enforcement</h2>
        <p className="text-xs text-gray-300 leading-relaxed">
          In FixFlow, AI is NEVER permitted to autonomously declare a technician professionally verified. Every formal diploma, trade certification, or experience portfolio must be explicitly approved by an authorized administrator with an immutable audit log entry.
        </p>
      </Card>
    </div>
  )
}
