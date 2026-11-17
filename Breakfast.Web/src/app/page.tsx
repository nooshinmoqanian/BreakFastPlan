'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import { api, money } from '@/lib/api'

export default function Home() {
  const router = useRouter()
  const [breakfasts, setBreakfasts] = useState<any[]>([])
  const [search, setSearch] = useState('')
  const [category, setCategory] = useState<'diet' | 'newest'>('newest')
  const [error, setError] = useState('')

  useEffect(() => {
    if (!localStorage.getItem('seenWelcome')) return router.replace('/welcome')

    api('/BreakfastItems/all-breakfast').then(setBreakfasts).catch((e) => setError(e.message))
  }, [router])

  const found = breakfasts.filter((b) => b.name.includes(search))

  const shown = category === 'diet'
    ? found.filter((b) => b.tagNames.some((t: any) => t.name === 'رژیمی'))
    : [...found].sort((a, b) => b.id - a.id)

  const lowCalorie = [...found].sort((a, b) => a.totalCalories - b.totalCalories).slice(0, 3)

  return (
    <>
      <h1>صبحانه‌ات را<br />پیدا کن</h1>
      {error && <p className="error">{error}</p>}

      <div className="search">
        <input placeholder="صبحانه‌ات را جست‌وجو کن" value={search} onChange={(e) => setSearch(e.target.value)} />
      </div>

      <h2>دسته‌بندی</h2>
      <div className="chips">
        <button className={category === 'diet' ? 'on' : ''} onClick={() => setCategory('diet')}>رژیمی</button>
        <button className={category === 'newest' ? 'on' : ''} onClick={() => setCategory('newest')}>جدیدترین</button>
      </div>

      <div className="grid">
        {shown.map((b) => (
          <Link className="food" href={`/breakfast/${b.id}`} key={b.id}>
            <span className="plate">🥞</span>
            <span className="row">
              <span>{b.name}</span>
              <span className="price">{money(b.ingredientsPrice)}</span>
            </span>
          </Link>
        ))}
      </div>
      {shown.length === 0 && !error && <p className="muted">صبحانه‌ای پیدا نشد.</p>}

      <h2>کم‌کالری‌ترین‌ها</h2>
      {lowCalorie.map((b) => (
        <Link className="list-item" href={`/breakfast/${b.id}`} key={b.id}>
          <span className="emoji">🥞</span>
          <span>
            <strong>{b.name}</strong>
            <br />
            <span className="muted">{b.totalCalories} کالری</span>
          </span>
        </Link>
      ))}
    </>
  )
}
