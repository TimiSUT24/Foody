import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import fs from 'fs'

export default defineConfig(({mode}) => {
  const isDev = mode === 'development'

  return{
    plugins: [react()],
    ...(isDev && {//development
      server: {
        https: {
          key: fs.readFileSync('./localhost-key.pem'),
          cert: fs.readFileSync('./localhost.pem'),
        },
        port: 5173,
      }
    }), 
    build:{  //Production
      outDir:'dist'
    }
  };
});
