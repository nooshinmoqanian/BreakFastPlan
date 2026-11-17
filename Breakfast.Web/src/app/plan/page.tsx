'use client'

import { FormEvent, useEffect, useState } from 'react'
import { api, money } from '@/lib/api'

export default function Plan() {
  const [plan, setPlan] = useState<any>(null)
  const [breakfasts, setBreakfasts] = useState<any[]>([])
  const [address, setAddress] = useState('')
  const [message, setMessage] = useState('')
  const [error, setError] = useState('')

  async function load() {
    try {
      setPlan(await api('/Plan'))
    } catch (e: any) {
      setError(e.message)
    }
  }

  useEffect(() => {
    load()
    api('/BreakfastItems/all-breakfast').then(setBreakfasts).catch((e) => setError(e.message))
  }, [])

  // Choosing "no breakfast" clears the day, anything else saves it.
  async function choose(day: number, breakfastId: string) {
    setError('')
    try {
      if (breakfastId) await api(`/Plan/${day}`, 'PUT', { breakfastId: Number(breakfastId) })
      else await api(`/Plan/${day}`, 'DELETE')
      await load()
    } catch (e: any) {
      setError(e.message)
    }
  }

  async function order(e: FormEvent) {
    e.preventDefault()
    setError('')
    setMessage('')
    try {
      const saved = await api('/Plan/order', 'POST', { address })
      setMessage(`سفارش لیست خرید ثبت شد. مبلغ: ${money(saved.totalPrice)}`)
    } catch (e: any) {
      setError(e.message)
    }
  }

  if (!plan) return <p className={error ? 'error' : ''}>{error || 'در حال بارگذاری...'}</p>

  return (
    <>
      <h1>برنامه‌ی هفتگی</h1>
      {error && <p className="error">{error}</p>}

      {plan.days.map((d: any) => (
        <div className="card row" key={d.day}>
          <strong>{d.dayName}</strong>
          <select style={{ maxWidth: 260 }} value={d.breakfast?.id ?? ''} onChange={(e) => choose(d.day, e.target.value)}>
            <option value="">بدون صبحانه</option>
            {breakfasts.map((b) => <option key={b.id} value={b.id}>{b.name}</option>)}
          </select>
          {d.breakfast && (
            <span className="muted" style={{ width: '100%' }}>
              {d.breakfast.calories} کالری
              {d.breakfast.unsuitableFor.length > 0 && (
                <span className="no"> · نامناسب برای {d.breakfast.unsuitableFor.join('، ')}</span>
              )}
            </span>
          )}
        </div>
      ))}

      <p>میانگین کالری صبحانه: <strong>{plan.averageCalories}</strong></p>

      <h2>لیست خرید هفته</h2>
      {plan.shoppingList.length === 0 ? (
        <p className="muted">برای روزها صبحانه انتخاب کنید تا لیست خرید ساخته شود.</p>
      ) : (
        <section className="card">
          <table>
            <thead>
              <tr><th>ماده</th><th>مقدار</th><th>قیمت</th></tr>
            </thead>
            <tbody>
              {plan.shoppingList.map((i: any) => (
                <tr key={i.name + i.unit}>
                  <td>{i.name}</td>
                  <td>{i.amount.toLocaleString('fa-IR')} {i.unit}</td>
                  <td>{money(i.price)}</td>
                </tr>
              ))}
            </tbody>
          </table>
          <p><strong>جمع: {money(plan.totalPrice)}</strong></p>
          <form onSubmit={order}>
            <input placeholder="آدرس تحویل" required value={address} onChange={(e) => setAddress(e.target.value)} />
            <button>سفارش کل لیست خرید</button>
          </form>
          {message && <p className="yes">{message}</p>}
        </section>
      )}
    </>
  )
}
