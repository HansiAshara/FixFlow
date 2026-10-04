import React from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../app/providers/useAuth'
import { Wrench, Shield, LogOut } from 'lucide-react'

export const Navbar: React.FC = () => {
  const { user, isAuthenticated, logout, hasRole } = useAuth()
  const navigate = useNavigate()

  return (
    <header className="border-b border-gray-800 bg-gray-900/80 backdrop-blur-md sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <div className="flex items-center space-x-8">
          <Link to="/" className="flex items-center space-x-2 text-blue-500 font-bold text-xl tracking-tight">
            <Wrench className="h-6 w-6 text-blue-500" />
            <span className="text-white">Fix<span className="text-blue-500">Flow</span></span>
          </Link>

          <nav className="hidden md:flex items-center space-x-6 text-sm font-medium">
            <Link to="/issues" className="text-gray-300 hover:text-white transition-colors">Issues</Link>
            <Link to="/technicians" className="text-gray-300 hover:text-white transition-colors">Technicians</Link>
            <Link to="/work-orders" className="text-gray-300 hover:text-white transition-colors">Work Orders</Link>
            {hasRole('ADMIN') && (
              <Link to="/admin" className="text-amber-400 hover:text-amber-300 flex items-center space-x-1">
                <Shield className="h-4 w-4" />
                <span>Admin</span>
              </Link>
            )}
          </nav>
        </div>

        <div className="flex items-center space-x-4">
          {isAuthenticated && user ? (
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-300 hidden sm:inline">{user.fullName}</span>
              <button
                onClick={() => {
                  logout()
                  navigate('/login')
                }}
                className="flex items-center space-x-1 text-sm text-gray-400 hover:text-red-400 cursor-pointer"
                title="Log out"
              >
                <LogOut className="h-4 w-4" />
                <span className="hidden sm:inline">Logout</span>
              </button>
            </div>
          ) : (
            <div className="flex items-center space-x-3">
              <Link
                to="/login"
                className="text-sm font-medium text-gray-300 hover:text-white px-3 py-1.5 rounded-lg hover:bg-gray-800 transition-colors"
              >
                Sign In
              </Link>
              <Link
                to="/register"
                className="text-sm font-medium bg-blue-600 hover:bg-blue-700 text-white px-3.5 py-1.5 rounded-lg transition-colors shadow-sm"
              >
                Get Started
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  )
}
