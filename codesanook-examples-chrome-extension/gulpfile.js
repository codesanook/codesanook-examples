const gulp = require("gulp");
const ts = require("gulp-typescript");
const jasmine = require('gulp-jasmine');
const tsProject = ts.createProject("tsconfig.json");
const clean = require('gulp-clean');
const runSequence = require('run-sequence');

let paths = {
    src: 'src/**/*.ts',
    spec: 'dist/spec/**/*.js',
    dest: 'dist',
};

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

//test task depends on clean and compile tasks 
gulp.task('test', done => {
    //start with clean, compile and test respectively 
    runSequence('clean', 'compile', () => {
        gulp.src(paths.spec)
            .pipe(jasmine({
                verbose: true
            }));
        done();
    });
});

gulp.task('watch', ['test'], () => {
    gulp.watch(paths.src, ['test']);
});