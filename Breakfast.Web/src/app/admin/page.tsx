'use client'

import { FormEvent, useEffect, useState } from 'react'
import { api, money } from '@/lib/api'

const statuses = { Pending: 'در انتظار', Preparing: 'در حال آماده‌سازی', Delivered: 'تحویل شد', Canceled: 'لغو شد' }

const emptyBreakfast = { id: 0, name: '', description: '', price: 0, recipe: '', tags: '', ingredients: '', health: '' }
const emptyRestaurant = { id: 0, name: '', type: 'Cafe', address: '', phone: '', menu: {} as Record<number, string> }

// Each line of the text box is one row, with columns separated by "|".
const rows = (text: string) => text.split('\n').filter((l) => l.trim()).map((l) => l.split('|').map((c) => c.trim()))

export default function Admin() {
  const [breakfasts, setBreakfasts] = useState<any[]>([])
  const [restaurants, setRestaurants] = useState<any[]>([])
  const [orders, setOrders] = useState<any[]>([])
  const [breakfast, setBreakfast] = useState(emptyBreakfast)
  const [restaurant, setRestaurant] = useState(emptyRestaurant)
  const [error, setError] = useState('')

  async function load() {
    try {
      setBreakfasts(await api('/BreakfastItems/all-breakfast'))
      setRestaurants(await api('/Restaurants'))
      setOrders(await api('/Orders'))
    } catch (e: any) {
      setError(e.message)
    }
  }

  useEffect(() => { load() }, [])

  async function run(action: () => Promise<unknown>) {
    setError('')
    try {
      await action()
      await load()
    } catch (e: any) {
      setError(e.message)
    }
  }

  function editBreakfast(b: any) {
    setBreakfast({
      id: b.id, name: b.name, description: b.description, price: b.price, recipe: b.recipe,
      tags: b.tagNames.map((t: any) => t.name).join(', '),
      ingredients: b.ingredients.map((i: any) => [i.name, i.amount, i.unit, i.calories, i.price].join(' | ')).join('\n'),
      health: b.healthInfos.map((h: any) => [h.condition, h.isSuitable ? 'بله' : 'خیر', h.note].join(' | ')).join('\n'),
    })
  }

  function saveBreakfast(e: FormEvent) {
    e.preventDefault()
    const body = {
      name: breakfast.name,
      description: breakfast.description,
      price: Number(breakfast.price),
      recipe: breakfast.recipe,
      tagNames: breakfast.tags.split(',').map((t) => t.trim()).filter(Boolean).map((name) => ({ name })),
      ingredients: rows(breakfast.ingredients).map(([name, amount, unit, calories, price]) => ({
        name, amount: Number(amount) || 0, unit: unit ?? '', calories: Number(calories) || 0, price: Number(price) || 0,
      })),
      healthInfos: rows(breakfast.health).map(([condition, suitable, note]) => ({
        condition, isSuitable: suitable === 'بله', note: note ?? '',
      })),
    }
    run(async () => {
      if (breakfast.id) await api(`/BreakfastItems/${breakfast.id}`, 'PUT', body)
      else await api('/BreakfastItems/add-breakfast', 'POST', body)
      setBreakfast(emptyBreakfast)
    })
  }

  function editRestaurant(r: any) {
    setRestaurant({
      id: r.id, name: r.name, type: r.type, address: r.address, phone: r.phone,
      menu: Object.fromEntries(r.menu.map((m: any) => [m.breakfastId, String(m.price)])),
    })
  }

  function saveRestaurant(e: FormEvent) {
    e.preventDefault()
    const menu = Object.entries(restaurant.menu)
      .filter(([, price]) => price !== '')
      .map(([breakfastId, price]) => ({ breakfastId: Number(breakfastId), price: Number(price) }))
    const body = { name: restaurant.name, type: restaurant.type, address: restaurant.address, phone: restaurant.phone, menu }
    run(async () => {
      if (restaurant.id) await api(`/Restaurants/${restaurant.id}`, 'PUT', body)
      else await api('/Restaurants', 'POST', body)
      setRestaurant(emptyRestaurant)
    })
  }

  return (
    <>
      <h1>پنل ادمین</h1>
      {error && <p className="error">{error}</p>}

      <h2>صبحانه‌ها</h2>
      <form className="card" onSubmit={saveBreakfast}>
        <input placeholder="نام" required value={breakfast.name} onChange={(e) => setBreakfast({ ...breakfast, name: e.target.value })} />
        <input placeholder="توضیح کوتاه" required value={breakfast.description} onChange={(e) => setBreakfast({ ...breakfast, description: e.target.value })} />
        <label>قیمت پایه
          <input type="number" min={0} value={breakfast.price} onChange={(e) => setBreakfast({ ...breakfast, price: Number(e.target.value) })} />
        </label>
        <textarea placeholder="طرز تهیه" rows={3} value={breakfast.recipe} onChange={(e) => setBreakfast({ ...breakfast, recipe: e.target.value })} />
        <input placeholder="تگ‌ها (با ویرگول جدا کنید)" value={breakfast.tags} onChange={(e) => setBreakfast({ ...breakfast, tags: e.target.value })} />
        <textarea placeholder={'مواد اولیه، هر خط:\nنام | مقدار | واحد | کالری | قیمت'} rows={4} value={breakfast.ingredients} onChange={(e) => setBreakfast({ ...breakfast, ingredients: e.target.value })} />
        <textarea placeholder={'بیماری‌ها، هر خط:\nنام بیماری | بله یا خیر | توضیح'} rows={3} value={breakfast.health} onChange={(e) => setBreakfast({ ...breakfast, health: e.target.value })} />
        <button>{breakfast.id ? 'ذخیره‌ی تغییرات' : 'افزودن صبحانه'}</button>
      </form>
      {breakfasts.map((b) => (
        <div className="row card" key={b.id}>
          <span>{b.name}</span>
          <span>
            <button className="ghost" onClick={() => editBreakfast(b)}>ویرایش</button>{' '}
            <button onClick={() => run(() => api(`/BreakfastItems/${b.id}`, 'DELETE'))}>حذف</button>
          </span>
        </div>
      ))}

      <h2>کافه‌ها و رستوران‌ها</h2>
      <form className="card" onSubmit={saveRestaurant}>
        <input placeholder="نام" required value={restaurant.name} onChange={(e) => setRestaurant({ ...restaurant, name: e.target.value })} />
        <select value={restaurant.type} onChange={(e) => setRestaurant({ ...restaurant, type: e.target.value })}>
          <option value="Cafe">کافه</option>
          <option value="Restaurant">رستوران</option>
        </select>
        <input placeholder="آدرس" required value={restaurant.address} onChange={(e) => setRestaurant({ ...restaurant, address: e.target.value })} />
        <input placeholder="تلفن" value={restaurant.phone} onChange={(e) => setRestaurant({ ...restaurant, phone: e.target.value })} />
        <p className="muted">قیمت هر صبحانه‌ای را که این مکان دارد وارد کنید؛ بقیه را خالی بگذارید.</p>
        {breakfasts.map((b) => (
          <label key={b.id}>{b.name}
            <input type="number" min={0} value={restaurant.menu[b.id] ?? ''} onChange={(e) => setRestaurant({ ...restaurant, menu: { ...restaurant.menu, [b.id]: e.target.value } })} />
          </label>
        ))}
        <button>{restaurant.id ? 'ذخیره‌ی تغییرات' : 'افزودن مکان'}</button>
      </form>
      {restaurants.map((r) => (
        <div className="row card" key={r.id}>
          <span>{r.name}</span>
          <span>
            <button className="ghost" onClick={() => editRestaurant(r)}>ویرایش</button>{' '}
            <button onClick={() => run(() => api(`/Restaurants/${r.id}`, 'DELETE'))}>حذف</button>
          </span>
        </div>
      ))}

      <h2>سفارش‌ها</h2>
      {orders.map((o) => (
        <div className="row card" key={o.id}>
          <span>
            {o.username}: {o.breakfastName} × {o.quantity} — {o.restaurantName ?? 'مواد اولیه'} — {money(o.totalPrice)}
            <br />
            <span className="muted">{o.address}</span>
          </span>
          <span>
            <select value={o.status} onChange={(e) => run(() => api(`/Orders/${o.id}/status`, 'PUT', { status: e.target.value }))}>
              {Object.entries(statuses).map(([value, label]) => <option key={value} value={value}>{label}</option>)}
            </select>
          </span>
        </div>
      ))}
    </>
  )
}
