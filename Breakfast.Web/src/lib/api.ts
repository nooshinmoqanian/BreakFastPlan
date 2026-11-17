// All requests go through this function, so the token is attached in one place.
export async function api(path: string, method = 'GET', body?: unknown) {
  const token = localStorage.getItem('token')
  let response: Response

  try {
    response = await fetch(`/api${path}`, {
      method,
      headers: {
        'Content-Type': 'application/json',
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
      body: body === undefined ? undefined : JSON.stringify(body),
    })
  } catch {
    throw new Error('اتصال به اینترنت یا سرور برقرار نشد')
  }

  const text = await response.text()
  const data = text ? tryParse(text) : null

  // On the login page a 401 means a wrong password, not an expired session.
  if (response.status === 401 && token && !path.startsWith('/Auth/')) {
    logout()
    throw new Error('نشست شما تمام شده؛ دوباره وارد شوید')
  }

  if (!response.ok) throw new Error(errorMessage(response.status, data))

  return data
}

// Turns every error shape the server can send into one readable sentence.
function errorMessage(status: number, data: any) {
  if (data?.errors) return Object.values(data.errors).flat().join('، ')
  if (data?.message) return data.message
  if (status === 401) return 'برای این کار اول وارد شوید'
  if (status === 403) return 'اجازه‌ی این کار را ندارید'
  if (status === 404) return 'مورد خواسته‌شده پیدا نشد'
  if (status >= 500) return 'سرور در دسترس نیست؛ کمی بعد دوباره تلاش کنید'
  return 'درخواست انجام نشد'
}

function tryParse(text: string) {
  try {
    return JSON.parse(text)
  } catch {
    return text
  }
}

export function login(token: string) {
  localStorage.setItem('token', token)
  window.location.href = '/'
}

export function logout() {
  localStorage.removeItem('token')
  window.location.href = '/login'
}

// The JWT middle part is plain base64 JSON: we read the role only to show or hide menus.
// The real permission check always happens on the server.
export function currentRole(): string | null {
  const token = localStorage.getItem('token')

  if (!token) return null

  const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')

  return JSON.parse(atob(payload)).role ?? null
}

export const money = (value: number) => `${value.toLocaleString('fa-IR')} تومان`
