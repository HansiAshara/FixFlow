import React from 'react'

export interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  glass?: boolean
}

export const Card: React.FC<CardProps> = ({ children, glass = false, className = '', ...props }) => {
  const bgClass = glass ? 'glass-card' : 'bg-gray-800 border border-gray-700/60'
  return (
    <div className={`rounded-xl p-6 shadow-lg ${bgClass} ${className}`} {...props}>
      {children}
    </div>
  )
}

export interface BadgeProps {
  children: React.ReactNode
  variant?: 'default' | 'success' | 'warning' | 'danger' | 'info'
  className?: string
}

export const Badge: React.FC<BadgeProps> = ({
  children,
  variant = 'default',
  className = '',
}) => {
  const variantStyles = {
    default: 'bg-gray-700 text-gray-200 border-gray-600',
    success: 'bg-emerald-950/60 text-emerald-300 border-emerald-700/50',
    warning: 'bg-amber-950/60 text-amber-300 border-amber-700/50',
    danger: 'bg-red-950/60 text-red-300 border-red-700/50',
    info: 'bg-blue-950/60 text-blue-300 border-blue-700/50',
  }[variant]

  return (
    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${variantStyles} ${className}`}>
      {children}
    </span>
  )
}
