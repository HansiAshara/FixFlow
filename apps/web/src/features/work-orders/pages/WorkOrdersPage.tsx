import React from 'react'
import { Badge } from '../../../shared/components/Card'
import { EmptyState } from '../../../shared/components/FeedbackComponents'
import { ClipboardList, ArrowRight } from 'lucide-react'

export const WorkOrdersPage: React.FC = () => {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-white tracking-tight">Work Orders</h1>
        <p className="text-sm text-gray-400">Authoritative lifecycle tracking from assignment to completion & verification</p>
      </div>

      <div className="p-4 bg-gray-900 border border-gray-800 rounded-xl space-y-2">
        <h3 className="text-sm font-semibold text-gray-300">Work Order State Flow Reference:</h3>
        <div className="flex flex-wrap items-center gap-2 text-xs text-gray-400">
          <Badge variant="warning">AWAITING_RESPONSE</Badge>
          <ArrowRight className="h-3 w-3 text-gray-600" />
          <Badge variant="info">ACCEPTED</Badge>
          <ArrowRight className="h-3 w-3 text-gray-600" />
          <Badge variant="info">IN_PROGRESS</Badge>
          <ArrowRight className="h-3 w-3 text-gray-600" />
          <Badge variant="warning">AWAITING_REPORTER</Badge>
          <ArrowRight className="h-3 w-3 text-gray-600" />
          <Badge variant="success">COMPLETED</Badge>
        </div>
      </div>

      <EmptyState
        title="No Active Work Orders"
        description="Select an eligible technician on a reported issue to initiate an authoritative work order."
        icon={<ClipboardList className="h-10 w-10 text-gray-500" />}
      />
    </div>
  )
}
