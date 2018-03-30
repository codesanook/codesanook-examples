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
const YAML = require('yamljs');
const fs = require('fs');


var paths = {
    src: 'src/**/*.ts',
    spec: 'dist/spec/**/*.js',
    dest: 'dist',
    travis: "src/travis.json"
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

gulp.task('js2Yaml', () => {
    try {
        var content = fs.readFileSync(paths.travis).toString();
        var jsonObject = JSON.parse(content);
        yamlString = YAML.stringify(jsonObject, 8, 2); //indent 2 space
        console.log(`\n${yamlString}\n`);
    } catch (ex) {
        console.log(ex);
    }
});

//gulp watchJS2Yaml
gulp.task('watchJS2Yaml', ['js2Yaml'], () => {
    gulp.watch(paths.travis, ["js2Yaml"]);
});