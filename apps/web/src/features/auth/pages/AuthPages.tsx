import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Card } from '../../../shared/components/Card'
import { Input } from '../../../shared/components/Input'
import { Button } from '../../../shared/components/Button'
import { useAuth } from '../../../app/providers/useAuth'
import { apiClient } from '../../../shared/lib/api-client'
import type { UserProfile } from '../../../shared/types'

export const LoginPage: React.FC = () => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setIsLoading(true)

    try {
      const response = await apiClient<{
        id: string
        email: string
        fullName: string
        roles: ('USER' | 'TECHNICIAN' | 'ADMIN')[]
        accessToken: string
      }>('/auth/login', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
      })

      const user: UserProfile = {
        id: response.id,
        email: response.email,
        fullName: response.fullName,
        roles: response.roles,
      }

      login(response.accessToken, user)
      navigate('/issues')
    } catch (err: any) {
      setError(err.message || 'Invalid email or password.')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="max-w-md mx-auto py-12">
      <Card glass>
        <div className="text-center mb-6">
          <h1 className="text-2xl font-bold text-white">Welcome Back</h1>
          <p className="text-sm text-gray-400 mt-1">Sign in to your FixFlow account</p>
        </div>

        {error && (
          <div className="p-3 mb-4 rounded-lg bg-red-950/40 border border-red-800 text-xs text-red-300">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <Input
            label="Email Address"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="you@example.com"
            required
          />
          <Input
            label="Password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="••••••••"
            required
          />
          <Button type="submit" variant="primary" className="w-full mt-2" isLoading={isLoading}>
            Sign In
          </Button>
        </form>

        <p className="text-center text-xs text-gray-400 mt-6">
          Don't have an account?{' '}
          <Link to="/register" className="text-blue-400 hover:text-blue-300 font-medium">
            Register here
          </Link>
        </p>
      </Card>
    </div>
  )
}

export const RegisterPage: React.FC = () => {
  const [fullName, setFullName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setIsLoading(true)

    try {
      const response = await apiClient<{
        id: string
        email: string
        fullName: string
        roles: ('USER' | 'TECHNICIAN' | 'ADMIN')[]
        accessToken: string
      }>('/auth/register', {
        method: 'POST',
        body: JSON.stringify({ fullName, email, password }),
      })

      const user: UserProfile = {
        id: response.id,
        email: response.email,
        fullName: response.fullName,
        roles: response.roles,
      }

      login(response.accessToken, user)
      navigate('/issues')
    } catch (err: any) {
      setError(err.message || 'Registration failed.')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="max-w-md mx-auto py-12">
      <Card glass>
        <div className="text-center mb-6">
          <h1 className="text-2xl font-bold text-white">Create FixFlow Account</h1>
          <p className="text-sm text-gray-400 mt-1">Join the intelligent maintenance network</p>
        </div>

        {error && (
          <div className="p-3 mb-4 rounded-lg bg-red-950/40 border border-red-800 text-xs text-red-300">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <Input
            label="Full Name"
            type="text"
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
            placeholder="Jane Doe"
            required
          />
          <Input
            label="Email Address"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="you@example.com"
            required
          />
          <Input
            label="Password (min 8 chars)"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="••••••••"
            required
          />
          <Button type="submit" variant="primary" className="w-full mt-2" isLoading={isLoading}>
            Create Account
          </Button>
        </form>

        <p className="text-center text-xs text-gray-400 mt-6">
          Already have an account?{' '}
          <Link to="/login" className="text-blue-400 hover:text-blue-300 font-medium">
            Sign in
          </Link>
        </p>
      </Card>
    </div>
  )
}
