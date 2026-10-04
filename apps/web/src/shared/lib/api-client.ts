const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api'

export class ApiError extends Error {
  status: number
  title: string
  detail?: string
  validationErrors?: Record<string, string[]>

  constructor(
    status: number,
    title: string,
    detail?: string,
    validationErrors?: Record<string, string[]>
  ) {
    super(detail || title)
    this.name = 'ApiError'
    this.status = status
    this.title = title
    this.detail = detail
    this.validationErrors = validationErrors
  }
}

export async function apiClient<T>(
  endpoint: string,
  options: RequestInit = {}
): Promise<T> {
  const token = localStorage.getItem('fixflow_token')

  const headers = new Headers(options.headers || {})
  if (!headers.has('Content-Type') && !(options.body instanceof FormData)) {
    headers.set('Content-Type', 'application/json')
  }

  if (token && !headers.has('Authorization')) {
    headers.set('Authorization', `Bearer ${token}`)
  }

  const url = endpoint.startsWith('http') ? endpoint : `${API_BASE_URL}${endpoint.startsWith('/') ? '' : '/'}${endpoint}`

  const response = await fetch(url, {
    ...options,
    headers,
  })

  if (!response.ok) {
    let errorDetail: { title?: string; detail?: string; errors?: Record<string, string[]> } = {}
    try {
      errorDetail = await response.json()
    } catch {
      // Non-JSON error response
    }

    throw new ApiError(
      response.status,
      errorDetail.title || `HTTP Error ${response.status}`,
      errorDetail.detail || response.statusText,
      errorDetail.errors
    )
  }

  if (response.status === 204) {
    return {} as T
  }

  return response.json() as Promise<T>
}
