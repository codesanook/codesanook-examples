//useful links
//https://www.typescriptlang.org/docs/handbook/migrating-from-javascript.html
//https://www.typescriptlang.org/docs/handbook/gulp.html
//https://jasmine.github.io/setup/nodejs.html
//https://www.npmjs.com/package/gulp-jasmine
//https://github.com/gulpjs/gulp/blob/master/docs/API.md

const gulp = require("gulp");
const ts = require("gulp-typescript");
const jasmine = require('gulp-jasmine');
var tsProject = ts.createProject("tsconfig.json");

var paths = {
    src: 'src/**/*.ts',
    spec: 'dist/spec/**/*.js',
    dest: 'dist'
}

gulp.task('compile', () => {
    gulp.src(paths.src)
        .pipe(tsProject())
        .pipe(gulp.dest(paths.dest));
});

gulp.task('test', () =>
    gulp.src(paths.spec)
        .pipe(jasmine())
);

gulp.task('watch', () => {
    gulp.watch(paths.src, ['compile']);
    gulp.watch(paths.spec, ['test']);
});