/// <reference types="vitest" />
/// <reference types="vite/client" />

import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  test: {
    // No need to import it, describe, test, ...etc
    globals: true,
    // Test web application.
    environment: "jsdom",
    setupFiles: "./src/test/setup.ts",
    // you might want to disable it, if you don't have tests that rely on CSS
    // since parsing CSS is slow
    css: true,
  },
  // Send build static files to our server to serve.
  build: {
    outDir: "../PetSearch.API/wwwroot",
    emptyOutDir: true,
  },
});
