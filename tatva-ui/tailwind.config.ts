import type { Config } from "tailwindcss";

export default {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        slate: {
          950: "#0f172a",
          900: "#1e293b",
          800: "#334155",
          700: "#475569",
          400: "#94a3b8",
          300: "#64748b",
        },
        emerald: {
          400: "#10b981",
          600: "#047857",
          700: "#065f46",
        },
      },
    },
  },
  plugins: [],
} satisfies Config;
