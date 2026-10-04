import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Card } from '../../../shared/components/Card'
import { Button } from '../../../shared/components/Button'
import { Input } from '../../../shared/components/Input'
import { EmptyState } from '../../../shared/components/FeedbackComponents'
import { Plus, Wrench, Sparkles } from 'lucide-react'
import { apiClient } from '../../../shared/lib/api-client'

export const IssuesPage: React.FC = () => {
  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-white tracking-tight">Reported Issues</h1>
          <p className="text-sm text-gray-400">Track and manage your maintenance problem reports</p>
        </div>
        <Link to="/issues/new">
          <Button variant="primary" className="flex items-center space-x-2">
            <Plus className="h-4 w-4" />
            <span>Report New Issue</span>
          </Button>
        </Link>
      </div>

      <EmptyState
        title="No Issues Reported Yet"
        description="Describe any home or commercial maintenance problem in natural language to let FixFlow's AI categorize and match technicians for you."
        icon={<Wrench className="h-10 w-10 text-gray-500" />}
        action={
          <Link to="/issues/new">
            <Button variant="outline" size="sm">Report First Issue</Button>
          </Link>
        }
      />
    </div>
  )
}

export const ReportIssuePage: React.FC = () => {
  const [title, setTitle] = useState('')
  const [rawDescription, setRawDescription] = useState('')
  const [location, setLocation] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)
    setError(null)

    try {
      await apiClient('/issues', {
        method: 'POST',
        body: JSON.stringify({ title, rawDescription, location }),
      })
      navigate('/issues')
    } catch (err: any) {
      setError(err.message || 'Failed to submit issue.')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-white tracking-tight">Report Maintenance Issue</h1>
        <p className="text-sm text-gray-400">FixFlow AI will automatically analyze category, skills, and priority</p>
      </div>

      {error && (
        <div className="p-3 rounded-lg bg-red-950/40 border border-red-800 text-xs text-red-300">
          {error}
        </div>
      )}

      <Card glass>
        <form onSubmit={handleSubmit} className="space-y-5">
          <Input
            label="Issue Summary / Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="e.g., Burst pipe under kitchen sink with rapid leaking"
            required
          />

          <div className="space-y-1.5">
            <label className="block text-sm font-medium text-gray-300">
              Natural Language Description
            </label>
            <textarea
              value={rawDescription}
              onChange={(e) => setRawDescription(e.target.value)}
              rows={4}
              className="w-full px-3.5 py-2 bg-gray-900 border border-gray-700 rounded-lg text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm"
              placeholder="Explain what is happening in plain English. For example: 'There is water dripping heavily from the main valve under the sink, making strange hissing sounds since this morning...'"
              required
            />
            <p className="text-xs text-gray-500 flex items-center space-x-1">
              <Sparkles className="h-3.5 w-3.5 text-blue-400" />
              <span>AI Worker will extract category, severity, risk level, and required skills asynchronously.</span>
            </p>
          </div>

          <Input
            label="Physical Location"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            placeholder="e.g., 404 Elm St, Suite 2B, Springfield"
            required
          />

          <div className="flex justify-end space-x-3 pt-4 border-t border-gray-800">
            <Button type="button" variant="ghost" onClick={() => navigate(-1)}>
              Cancel
            </Button>
            <Button type="submit" variant="primary" isLoading={isLoading}>
              Submit & Request AI Analysis
            </Button>
          </div>
        </form>
      </Card>
    </div>
  )
}
