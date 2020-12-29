'use strict'

import { src, dest, task, watch, parallel, series } from 'gulp'
import del = require('del')
import babel = require('gulp-babel')
import concat = require('gulp-concat')
import uglify = require('gulp-uglify')
import browserSync = require('browser-sync')
import header = require('gulp-header')
import postcss = require('gulp-postcss')
import rename = require('gulp-rename')

var bsServer = browserSync.create()
const minHeader = `/* Compiled on ${new Date().toUTCString()} */`

var paths = {
  styles: {
    src: ['styles/app.css'],
    dest: 'dist/css',
  },
  scripts: {
    app: {
      src: ['scripts/app.js'],
      dest: 'dist/js',
    },
    lib: {
      src: ['scripts/lib/**/*.js'],
      dest: 'dist/js',
    },
    vendor: {
      src: ['scripts/vendor.js'],
      dest: 'dist/js',
    },
  },
  copy: {
    src: ['assets/**/*.{html,png,gif,jpg,jpeg,webp,svg,eot,ttf,woff,woff2,ico}'],
    destClean: ['dist/assets/**/*.{html,png,gif,jpg,jpeg,webp,svg,eot,ttf,woff,woff2}'],
    dest: 'dist/assets',
  },
}

const doClean = (target: string | string[]) => del(target)

const doWatch = (srcFiles: string | string[], run: Function) => watch(srcFiles, run)

const doCopy = (srcFiles: string | string[], destination: string) => src(srcFiles).pipe(dest(destination))

const doBabelJs = (srcFiles: string | string[], destination: string, bundleName: string) => {
  return src(srcFiles).pipe(babel()).pipe(uglify()).pipe(concat(bundleName)).pipe(header(minHeader)).pipe(dest(destination))
}

const doJs = (srcFiles: string | string[], destination: string, bundleName: string) => {
  if (bundleName && bundleName.trim().length > 0) {
    return src(srcFiles).pipe(uglify()).pipe(concat(bundleName)).pipe(header(minHeader)).pipe(dest(destination))
  } else {
    return src(srcFiles)
      .pipe(dest(destination))
      .pipe(uglify())
      .pipe(
        rename({
          extname: '.min.js',
        })
      )
      .pipe(dest(destination))
  }
}

/*  Clean  */

const cleanCSS = () => doClean(paths.styles.dest)
task('css:clean', cleanCSS)

const cleanJS = () => doClean([paths.scripts.vendor.dest, paths.scripts.app.dest])
task('js:clean', cleanJS)

const cleanCopy = () => doClean(paths.copy.destClean)
task('copy:clean', cleanCopy)

/*  Compile  */

const compileCSS = () => src(paths.styles.src).pipe(postcss()).pipe(concat('app.min.css')).pipe(header(minHeader)).pipe(dest(paths.styles.dest))
task('css:compile', compileCSS)

const compileJsVendor = () => doBabelJs(paths.scripts.vendor.src, paths.scripts.vendor.dest, 'vendor.min.js')
task('js:vendor', compileJsVendor)

const compileJsLib = () => doJs(paths.scripts.lib.src, paths.scripts.lib.dest, null)
task('js:lib', compileJsLib)

const compileJsApp = () => doBabelJs(paths.scripts.app.src, paths.scripts.app.dest, 'app.min.js')
task('js:app', compileJsApp)

// const compileJS = series(compileJsApp, compileJsLib, compileJsVendor)
const compileJS = series(compileJsLib, compileJsVendor)
task('js', compileJS)

const copyFiles = () => doCopy(paths.copy.src, paths.copy.dest)
task('copy', copyFiles)

const appWatch = () => {
  doWatch(paths.scripts.vendor.src, series(compileJsLib, bsReload))
  doWatch(paths.scripts.app.src, series(compileJsApp, bsReload))

  doWatch(paths.styles.src, series(compileCSS, bsReload))
  doWatch(paths.copy.src, series(copyFiles, bsReload))
  doWatch('src/**/*.html', bsReload)
}
task('watch', appWatch)

function bsReload(done: () => void) {
  bsServer.reload()
  done()
}

function bsServe(done: () => void) {
  bsServer.init({
    server: {
      baseDir: './src',
    },
  })
  done()
}

export const css = series(cleanCSS, compileCSS)
export const js = series(cleanJS, compileJS)
export const build = parallel(css, js, copyFiles)
export const clean = parallel(cleanCSS, cleanJS, cleanCopy)
export const debug = series(build, parallel(appWatch, bsServe))

export default build
