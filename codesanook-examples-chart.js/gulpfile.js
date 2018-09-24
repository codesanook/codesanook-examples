const gulp = require("gulp");
const ts = require("gulp-typescript");
const tsProject = ts.createProject("tsconfig.json");
const clean = require('gulp-clean');
const runSequence = require('run-sequence');

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

gulp.task('compile', () => {
    return gulp.src(paths.src)
        .pipe(tsProject())
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