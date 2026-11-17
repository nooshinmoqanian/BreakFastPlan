'use client'

import Link from 'next/link'
import { FormEvent, useState } from 'react'
import { api, login } from '@/lib/api'

export default function Login() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')

  async function submit(e: FormEvent) {
    e.preventDefault()
    try {
      const result = await api('/Auth/login', 'POST', { username, password })
      login(result.accessToken)
    } catch (e: any) {
      setError(e.message)
    }
  }

  return (
    <form className="auth" onSubmit={submit}>
      <div className="hero">🥐</div>
      <h1>خوش آمدید</h1>
      <p className="muted">وارد شوید</p>
      <input placeholder="نام کاربری" value={username} onChange={(e) => setUsername(e.target.value)} />
      <input placeholder="رمز عبور" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
      <button>ورود</button>
      {error && <p className="error">{error}</p>}
      <Link className="muted" href="/register">اکانت ندارید؟ <strong>ثبت‌نام کنید</strong></Link>
    </form>
  )
}
