'use client'

import { useParams } from 'next/navigation'
import { useEffect, useState } from 'react'
import { api, currentRole, money } from '@/lib/api'

export default function BreakfastDetail() {
  const { id } = useParams()
  const [breakfast, setBreakfast] = useState<any>(null)
  const [quantity, setQuantity] = useState(1)
  const [address, setAddress] = useState('')
  const [message, setMessage] = useState('')

  useEffect(() => {
    api(`/BreakfastItems/${id}`).then(setBreakfast).catch((e) => setMessage(e.message))
  }, [id])

  async function order(orderType: string, restaurantId?: number) {
    if (!currentRole()) return setMessage('برای سفارش اول وارد شوید')
    if (!address) return setMessage('آدرس را وارد کنید')

    try {
      const saved = await api('/Orders', 'POST', { breakfastId: breakfast.id, orderType, restaurantId, quantity, address })
      setMessage(`سفارش ثبت شد. مبلغ: ${money(saved.totalPrice)}`)
    } catch (e: any) {
      setMessage(e.message)
    }
  }

  if (!breakfast) return <p>{message || 'در حال بارگذاری...'}</p>

  return (
    <>
      <div className="hero-image">🥞</div>
      <div className="sheet">
        <h1>{breakfast.name}</h1>
        <p>{breakfast.description}</p>
      </div>

      <section className="card">
        <h2>طرز تهیه</h2>
        <p className="recipe">{breakfast.recipe}</p>
      </section>

      <section className="card">
        <h2>مواد اولیه و کالری</h2>
        <table>
          <thead>
            <tr><th>ماده</th><th>مقدار</th><th>کالری</th><th>قیمت</th></tr>
          </thead>
          <tbody>
            {breakfast.ingredients.map((i: any) => (
              <tr key={i.name}>
                <td>{i.name}</td><td>{i.amount.toLocaleString('fa-IR')} {i.unit}</td><td>{i.calories}</td><td>{money(i.price)}</td>
              </tr>
            ))}
          </tbody>
        </table>
        <p>جمع کالری: {breakfast.totalCalories}</p>
      </section>

      <section className="card">
        <h2>مناسب برای بیماری‌ها</h2>
        {breakfast.healthInfos.map((h: any) => (
          <p key={h.condition} className={h.isSuitable ? 'yes' : 'no'}>
            {h.isSuitable ? '✓ مناسب' : '✗ نامناسب'} برای {h.condition} {h.note && `— ${h.note}`}
          </p>
        ))}
        <p className="muted">این اطلاعات جای توصیه‌ی پزشک را نمی‌گیرد.</p>
      </section>

      <section className="card">
        <h2>سفارش</h2>
        <form onSubmit={(e) => e.preventDefault()}>
          <label>تعداد
            <input type="number" min={1} max={20} value={quantity} onChange={(e) => setQuantity(Number(e.target.value))} />
          </label>
          <label>آدرس تحویل
            <input value={address} onChange={(e) => setAddress(e.target.value)} />
          </label>
        </form>
        <div className="row">
          <p>خودم درست می‌کنم: خرید مواد اولیه ({money(breakfast.ingredientsPrice)})</p>
          <button onClick={() => order('Ingredients')}>افزودن مواد به سفارش</button>
        </div>
        {breakfast.restaurants.map((r: any) => (
          <div className="row" key={r.restaurantId}>
            <p>
              {r.restaurantName} ({r.type === 'Cafe' ? 'کافه' : 'رستوران'}) — {money(r.price)}
              <br />
              <span className="muted">{r.address} · {r.phone}</span>
            </p>
            <button onClick={() => order('Restaurant', r.restaurantId)}>سفارش از اینجا</button>
          </div>
        ))}
        {message && <p>{message}</p>}
      </section>
    </>
  )
}
