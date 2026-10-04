import React from 'react'

export interface LoadingSpinnerProps {
  size?: 'sm' | 'md' | 'lg'
  message?: string
}

export const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({ size = 'md', message }) => {
  const sizeClasses = {
    sm: 'h-4 w-4 border-2',
    md: 'h-8 w-8 border-3',
    lg: 'h-12 w-12 border-4',
  }[size]

  return (
    <div className="flex flex-col items-center justify-center p-6 space-y-3">
      <div className={`animate-spin rounded-full border-blue-500 border-t-transparent ${sizeClasses}`} />
      {message && <p className="text-sm text-gray-400 animate-pulse">{message}</p>}
    </div>
  )
}

export interface EmptyStateProps {
  title: string
  description?: string
  action?: React.ReactNode
  icon?: React.ReactNode
}

export const EmptyState: React.FC<EmptyStateProps> = ({ title, description, action, icon }) => {
  return (
    <div className="flex flex-col items-center justify-center text-center p-8 border border-dashed border-gray-700 rounded-xl bg-gray-900/30">
      {icon && <div className="mb-3 text-gray-500">{icon}</div>}
      <h3 className="text-base font-semibold text-gray-200">{title}</h3>
      {description && <p className="text-sm text-gray-400 mt-1 max-w-sm">{description}</p>}
      {action && <div className="mt-4">{action}</div>}
    </div>
  )
}

export interface AlertProps {
  variant?: 'info' | 'success' | 'warning' | 'error'
  title?: string
  children: React.ReactNode
}

export const Alert: React.FC<AlertProps> = ({ variant = 'info', title, children }) => {
  const styles = {
    info: 'bg-blue-950/40 border-blue-800 text-blue-200',
    success: 'bg-emerald-950/40 border-emerald-800 text-emerald-200',
    warning: 'bg-amber-950/40 border-amber-800 text-amber-200',
    error: 'bg-red-950/40 border-red-800 text-red-200',
  }[variant]

  return (
    <div className={`p-4 rounded-lg border text-sm ${styles}`}>
      {title && <h4 className="font-semibold mb-1">{title}</h4>}
      <div>{children}</div>
    </div>
  )
}

interface ErrorBoundaryProps {
  children: React.ReactNode
  fallback?: React.ReactNode
}

interface ErrorBoundaryState {
  hasError: boolean
  error?: Error
}

export class ErrorBoundary extends React.Component<ErrorBoundaryProps, ErrorBoundaryState> {
  constructor(props: ErrorBoundaryProps) {
    super(props)
    this.state = { hasError: false }
  }

  static getDerivedStateFromError(error: Error): ErrorBoundaryState {
    return { hasError: true, error }
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error('ErrorBoundary caught an error:', error, errorInfo)
  }

  render() {
    if (this.state.hasError) {
      return (
        this.props.fallback || (
          <div className="p-8 text-center bg-red-950/30 border border-red-800 rounded-xl max-w-lg mx-auto my-8">
            <h2 className="text-lg font-bold text-red-300">Something went wrong</h2>
            <p className="text-sm text-gray-300 mt-2">{this.state.error?.message || 'An unexpected error occurred.'}</p>
            <button
              onClick={() => this.setState({ hasError: false })}
              className="mt-4 px-4 py-2 bg-red-700 hover:bg-red-600 text-white text-xs font-semibold rounded-lg"
            >
              Try Again
            </button>
          </div>
        )
      )
    }

    return this.props.children
  }
}
