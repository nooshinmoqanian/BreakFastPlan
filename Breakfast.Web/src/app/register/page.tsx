'use client'

import Link from 'next/link'
import { FormEvent, useState } from 'react'
import { api, login } from '@/lib/api'

export default function Register() {
  const [form, setForm] = useState({ username: '', email: '', password: '', passwordConfirm: '' })
  const [error, setError] = useState('')

  const field = (name: keyof typeof form) => ({
    value: form[name],
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => setForm({ ...form, [name]: e.target.value }),
  })

  async function submit(e: FormEvent) {
    e.preventDefault()
    try {
      const result = await api('/Auth/register', 'POST', form)
      login(result.accessToken)
    } catch (e: any) {
      setError(e.message)
    }
  }

  return (
    <form className="auth" onSubmit={submit}>
      <div className="hero">🥐</div>
      <h1>ثبت‌نام</h1>
      <input placeholder="نام کاربری" {...field('username')} />
      <input placeholder="ایمیل" type="email" {...field('email')} />
      <input placeholder="رمز عبور (۸ تا ۲۰ حرف)" type="password" {...field('password')} />
      <input placeholder="تکرار رمز عبور" type="password" {...field('passwordConfirm')} />
      <button>ثبت‌نام</button>
      {error && <p className="error">{error}</p>}
      <Link className="muted" href="/login">اکانت دارید؟ <strong>وارد شوید</strong></Link>
    </form>
  )
}
