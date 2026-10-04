export type UserRole = 'USER' | 'TECHNICIAN' | 'ADMIN'

export interface UserProfile {
  id: string
  email: string
  fullName: string
  roles: UserRole[]
}

export interface ApiResponse<T> {
  data: T
  message?: string
  success: boolean
}

export interface ApiProblemDetails {
  status: number
  title: string
  detail?: string
  instance?: string
  errors?: Record<string, string[]>
}
