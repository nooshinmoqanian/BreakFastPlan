'use client'

import Link from 'next/link'
import { useEffect } from 'react'

export default function Welcome() {
  // Remember the visit, so this screen only appears the first time.
  useEffect(() => localStorage.setItem('seenWelcome', '1'), [])

  return (
    <section className="welcome">
      <span className="cup">☕</span>
      <p>برنامه‌های</p>
      <h1>صبحانه</h1>
      <Link href="/" aria-label="ادامه">‹</Link>
    </section>
  )
}
