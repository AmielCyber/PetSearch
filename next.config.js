/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
};

module.exports = {
  images: {
    remotePatterns: [
      {
        protocol: "https",
        hostname: "dl5zpyw5k3jeb.cloudfront.net",
        port: "",
      },
    ],
  },
};
