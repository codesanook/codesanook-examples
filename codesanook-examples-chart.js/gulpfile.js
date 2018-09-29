//I quite don't understand why we need to install this module
//https://www.npmjs.com/package/natives
const gulp = require('gulp');
const clean = require('gulp-clean');
const runSequence = require('run-sequence');

const browserify = require('browserify');
const source = require('vinyl-source-stream');
const tsify = require('tsify');

var paths = {
    src: 'src/**/*.ts',
    dest: 'dist',
}

gulp.task('clean', () => {
    return gulp.src(paths.dest, {
            read: false
        })
        .pipe(clean());
});

gulp.task('compile', [], () => {
    let b = browserify({
        basedir: '.',
        debug: false, //turn on/off source map 
        entries: ['./src/index.ts'],//main file
        cache: {},
        packageCache: {}
    });

    b.plugin(tsify)
        .bundle()
        .on('error', error => {
            console.log(error);
        })
        .pipe(source('bundle.js')) //output to bundle.js
        .pipe(gulp.dest(paths.dest));
});

gulp.task('build', done => {
    runSequence('clean', 'compile', () => {
        done();
    });
});

gulp.task('watch', ['build'], () => {
    gulp.watch(paths.src, ['build']);
});