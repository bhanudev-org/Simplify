import babel from '@rollup/plugin-babel'
import { nodeResolve } from '@rollup/plugin-node-resolve'
import { terser } from 'rollup-plugin-terser'

export default [
  {
    input: 'src/js/app.ts',
    output: [
      {
        file: 'build/js/app.js',
        format: 'cjs',
        sourcemap: true,
      },
      {
        file: 'build/js/app.min.js',
        format: 'cjs',
        plugins: [terser()],
      },
      // { file: 'build/js/app.esm.js', format: 'esm' },
    ],
    plugins: [nodeResolve(), babel({ babelHelpers: 'bundled' })],
  },
  {
    input: 'src/js/lib/jquery.ts',
    output: [
      {
        file: 'build/js/jquery.js',
        format: 'cjs',
        sourcemap: true,
      },
      {
        file: 'build/js/jquuery.min.js',
        format: 'cjs',
        plugins: [terser()],
      },
    ],
    plugins: [nodeResolve(), babel({ babelHelpers: 'bundled' })],
  },
]
