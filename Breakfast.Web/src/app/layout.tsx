import type { Metadata, Viewport } from 'next'
import { Vazirmatn } from 'next/font/google'
import './globals.css'
import Nav from './Nav'

const vazirmatn = Vazirmatn({ subsets: ['arabic'] })

export const metadata: Metadata = {
  title: 'برنامه صبحانه',
  description: 'انتخاب صبحانه، خرید مواد اولیه و سفارش از کافه',
}

export const viewport: Viewport = { width: 'device-width', initialScale: 1 }

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="fa" dir="rtl">
      <body className={vazirmatn.className}>
        <Nav />
        <main className="container">{children}</main>
      </body>
    </html>
  )
}
