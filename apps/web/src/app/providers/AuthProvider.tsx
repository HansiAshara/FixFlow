import React, { useState } from 'react'
import { AuthContext } from './AuthContext'
import type { UserProfile, UserRole } from '../../shared/types'

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('fixflow_token'))
  const [user, setUser] = useState<UserProfile | null>(() => {
    const savedUser = localStorage.getItem('fixflow_user')
    if (savedUser) {
      try {
        return JSON.parse(savedUser)
      } catch {
        localStorage.removeItem('fixflow_user')
      }
    }
    return null
  })

  const login = (newToken: string, newUser: UserProfile) => {
    localStorage.setItem('fixflow_token', newToken)
    localStorage.setItem('fixflow_user', JSON.stringify(newUser))
    setToken(newToken)
    setUser(newUser)
  }

  const logout = () => {
    localStorage.removeItem('fixflow_token')
    localStorage.removeItem('fixflow_user')
    setToken(null)
    setUser(null)
  }

  const hasRole = (role: UserRole) => {
    return user?.roles.includes(role) ?? false
  }

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isAuthenticated: !!token && !!user,
        login,
        logout,
        hasRole,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}
