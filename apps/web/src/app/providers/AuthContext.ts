import { createContext } from 'react'
import type { UserProfile, UserRole } from '../../shared/types'

export interface AuthContextType {
  user: UserProfile | null
  token: string | null
  isAuthenticated: boolean
  login: (token: string, user: UserProfile) => void
  logout: () => void
  hasRole: (role: UserRole) => boolean
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined)
