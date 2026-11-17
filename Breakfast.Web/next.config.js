/** @type {import('next').NextConfig} */
const nextConfig = {
  async rewrites() {
    return [{ source: '/api/:path*', destination: 'http://localhost:5179/api/:path*' }]
  },
}

module.exports = nextConfig
