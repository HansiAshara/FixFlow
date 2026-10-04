import React from 'react'
import { Card, Badge } from '../../../shared/components/Card'
import { Button } from '../../../shared/components/Button'
import { Star, Clock, Award } from 'lucide-react'

export const TechniciansPage: React.FC = () => {
  const sampleTechnicians = [
    {
      id: '1',
      name: 'Marcus Vance',
      specialty: 'Master Electrician & Smart Home',
      experienceYears: 12,
      rating: 4.95,
      completedJobs: 138,
      status: 'VERIFIED',
      skills: ['Circuit Diagnosis', 'Panel Replacement', 'Wiring', 'Emergency Surge'],
    },
    {
      id: '2',
      name: 'Elena Rostova',
      specialty: 'HVAC & Refrigeration Specialist',
      experienceYears: 8,
      rating: 4.88,
      completedJobs: 94,
      status: 'VERIFIED',
      skills: ['Heat Pump Repair', 'Compressor Diagnostic', 'Duct Sealing'],
    },
    {
      id: '3',
      name: 'Darius Thorne',
      specialty: 'Plumbing & Hydraulic Systems',
      experienceYears: 4,
      rating: 4.75,
      completedJobs: 26,
      status: 'PROBATIONARY',
      skills: ['Pipe Leak Repair', 'Water Heaters', 'Drain Snaking'],
    },
  ]

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-white tracking-tight">Verified Technicians</h1>
          <p className="text-sm text-gray-400">All technicians undergo human-in-the-loop admin verification & continuous performance scoring</p>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {sampleTechnicians.map((tech) => (
          <Card key={tech.id} glass className="flex flex-col justify-between">
            <div>
              <div className="flex items-start justify-between">
                <div>
                  <h3 className="text-lg font-bold text-white">{tech.name}</h3>
                  <p className="text-xs text-blue-400 mt-0.5">{tech.specialty}</p>
                </div>
                <Badge variant={tech.status === 'VERIFIED' ? 'success' : 'warning'}>
                  {tech.status}
                </Badge>
              </div>

              <div className="grid grid-cols-3 gap-2 my-4 p-3 bg-gray-900/50 rounded-lg text-center text-xs">
                <div>
                  <div className="text-gray-400 flex items-center justify-center space-x-1">
                    <Star className="h-3.5 w-3.5 text-amber-400 fill-amber-400" />
                    <span>Rating</span>
                  </div>
                  <div className="font-semibold text-white mt-1">{tech.rating}</div>
                </div>
                <div>
                  <div className="text-gray-400 flex items-center justify-center space-x-1">
                    <Award className="h-3.5 w-3.5 text-blue-400" />
                    <span>Jobs</span>
                  </div>
                  <div className="font-semibold text-white mt-1">{tech.completedJobs}</div>
                </div>
                <div>
                  <div className="text-gray-400 flex items-center justify-center space-x-1">
                    <Clock className="h-3.5 w-3.5 text-emerald-400" />
                    <span>Exp</span>
                  </div>
                  <div className="font-semibold text-white mt-1">{tech.experienceYears} yrs</div>
                </div>
              </div>

              <div className="space-y-1.5">
                <span className="text-xs font-medium text-gray-400">Verified Skills:</span>
                <div className="flex flex-wrap gap-1.5">
                  {tech.skills.map((skill) => (
                    <span key={skill} className="px-2 py-0.5 text-xs bg-gray-800 text-gray-300 rounded border border-gray-700">
                      {skill}
                    </span>
                  ))}
                </div>
              </div>
            </div>

            <div className="mt-6 pt-4 border-t border-gray-800">
              <Button variant="secondary" size="sm" className="w-full">
                View Performance History
              </Button>
            </div>
          </Card>
        ))}
      </div>
    </div>
  )
}
