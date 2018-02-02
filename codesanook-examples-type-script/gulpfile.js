//useful links
//https://www.typescriptlang.org/docs/handbook/migrating-from-javascript.html
//https://www.typescriptlang.org/docs/handbook/gulp.html
//https://jasmine.github.io/setup/nodejs.html
//https://www.npmjs.com/package/gulp-jasmine
//https://github.com/gulpjs/gulp/blob/master/docs/API.md

const gulp = require("gulp");
const ts = require("gulp-typescript");
const jasmine = require('gulp-jasmine');
const tsProject = ts.createProject("tsconfig.json");
const clean = require('gulp-clean');
const runSequence = require('run-sequence');


var paths = {
    src: 'src/**/*.ts',
    spec: 'dist/spec/**/*.js',
    dest: 'dist'
}

gulp.task('clean', () => {
    return gulp.src(paths.dest, {
            read: false
        })
        .pipe(clean());
});

gulp.task('compile', () => {
    return gulp.src(paths.src)
        .pipe(tsProject())
        .pipe(gulp.dest(paths.dest));
});

//test depends on clean and compile tasks 
gulp.task('test', (done) => {
    runSequence('clean', 'compile', function () {
        gulp.src(paths.spec)
            .pipe(jasmine({
                verbose: true
            }));
        done();
    });
});

gulp.task('watch', () => {
    gulp.watch(paths.src, ['compile']);
    gulp.watch(paths.spec, ['test']);
});