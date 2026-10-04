import React from 'react'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AppLayout, AuthGuard } from '../../layouts/AppLayout'
import { HomePage } from '../../features/home/HomePage'
import { LoginPage, RegisterPage } from '../../features/auth/pages/AuthPages'
import { IssuesPage, ReportIssuePage } from '../../features/issues/pages/IssuesPages'
import { TechniciansPage } from '../../features/technicians/pages/TechniciansPages'
import { WorkOrdersPage } from '../../features/work-orders/pages/WorkOrdersPage'
import { AdminPage } from '../../features/administration/pages/AdminPage'

export const AppRouter: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout />}>
          {/* Public Routes */}
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/technicians" element={<TechniciansPage />} />

          {/* Authenticated Routes */}
          <Route
            path="/issues"
            element={
              <AuthGuard>
                <IssuesPage />
              </AuthGuard>
            }
          />
          <Route
            path="/issues/new"
            element={
              <AuthGuard>
                <ReportIssuePage />
              </AuthGuard>
            }
          />
          <Route
            path="/work-orders"
            element={
              <AuthGuard>
                <WorkOrdersPage />
              </AuthGuard>
            }
          />

          {/* Admin Routes */}
          <Route
            path="/admin"
            element={
              <AuthGuard allowedRoles={['ADMIN']}>
                <AdminPage />
              </AuthGuard>
            }
          />

          {/* Fallback */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
