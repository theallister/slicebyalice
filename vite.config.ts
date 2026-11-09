import { resolve } from 'node:path';
import { defineConfig } from 'vite';

export default defineConfig({
  build: {
    outDir: 'wwwroot/assets',
    emptyOutDir: true,
    manifest: false,
    rollupOptions: {
      input: {
        main: resolve(import.meta.dirname, 'src/ts/main.ts'),
        styles: resolve(import.meta.dirname, 'src/scss/main.scss')
      },
      output: {
        entryFileNames: 'js/[name].js',
        assetFileNames: assetInfo => {
          if (assetInfo.names[0]?.endsWith('.css')) {
            return 'css/[name][extname]';
          }

          return 'media/[name][extname]';
        }
      }
    }
  }
});