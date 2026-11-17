'use client'

import { useEffect, useState } from 'react'
import { api, money } from '@/lib/api'

const statusNames: Record<string, string> = {
  Pending: 'در انتظار',
  Preparing: 'در حال آماده‌سازی',
  Delivered: 'تحویل شد',
  Canceled: 'لغو شد',
}

export default function Orders() {
  const [orders, setOrders] = useState<any[]>([])
  const [error, setError] = useState('')

  useEffect(() => {
    api('/Orders/mine').then(setOrders).catch((e) => setError(e.message))
  }, [])

  return (
    <>
      <h1>سفارش‌های من</h1>
      {error && <p className="error">{error}</p>}
      {orders.length === 0 && !error && <p>هنوز سفارشی ندارید.</p>}
      {orders.map((o) => (
        <div className="card" key={o.id}>
          <div className="row">
            <strong>{o.breakfastName} × {o.quantity}</strong>
            <span>{statusNames[o.status]}</span>
          </div>
          <p className="muted">
            {o.restaurantName ?? 'خرید مواد اولیه'} · {money(o.totalPrice)}
          </p>
        </div>
      ))}
    </>
  )
}
