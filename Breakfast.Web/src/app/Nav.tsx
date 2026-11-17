'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { useEffect, useState } from 'react'
import { currentRole, logout } from '@/lib/api'

export default function Nav() {
  const pathname = usePathname()
  const [role, setRole] = useState<string | null>(null)

  useEffect(() => setRole(currentRole()), [pathname])

  if (pathname === '/welcome') return null

  const item = (href: string, icon: string, label: string) => (
    <Link href={href} className={pathname === href ? 'active' : ''}>
      <span className="icon">{icon}</span>
      {label}
    </Link>
  )

  return (
    <nav className="nav">
      {item('/', '⌂', 'خانه')}
      {role && item('/plan', '📅', 'برنامه')}
      {role && item('/orders', '🛒', 'سفارش‌ها')}
      {role === 'Admin' && item('/admin', '⚙', 'ادمین')}
      {role ? (
        <button onClick={logout}>
          <span className="icon">↩</span>
          خروج
        </button>
      ) : (
        item('/login', '👤', 'ورود')
      )}
    </nav>
  )
}
