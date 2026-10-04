import React from 'react'
import { Outlet, Navigate, useLocation } from 'react-router-dom'
import { Navbar } from './Navbar'
import { useAuth } from '../app/providers/useAuth'
import type { UserRole } from '../shared/types'

const CURRENT_YEAR = 2026

export const Footer: React.FC = () => (
  <footer className="border-t border-gray-800 bg-gray-950 py-8 text-center text-xs text-gray-500 mt-auto">
    <div className="max-w-7xl mx-auto px-4">
      <p>&copy; {CURRENT_YEAR} FixFlow Engineering Foundation. Built for reliability & scale.</p>
    </div>
  </footer>
)

export const AppLayout: React.FC = () => (
  <div className="min-h-screen flex flex-col bg-gray-950 text-gray-100">
    <Navbar />
    <main className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <Outlet />
    </main>
    <Footer />
  </div>
)

export const AuthGuard: React.FC<{ allowedRoles?: UserRole[]; children: React.ReactNode }> = ({
  allowedRoles,
  children,
}) => {
  const { isAuthenticated, hasRole } = useAuth()
  const location = useLocation()

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />
  }

  if (allowedRoles && !allowedRoles.some(role => hasRole(role))) {
    return <Navigate to="/" replace />
  }

  return <>{children}</>
}
