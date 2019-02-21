//useful links
//https://www.typescriptlang.org/docs/handbook/migrating-from-javascript.html
//https://www.typescriptlang.org/docs/handbook/gulp.html
//https://jasmine.github.io/setup/nodejs.html
//https://www.npmjs.com/package/gulp-jasmine
//https://github.com/gulpjs/gulp/blob/master/docs/API.md
const gulp = require('gulp');
const ts = require('gulp-typescript');
const jasmine = require('gulp-jasmine');
const tsProject = ts.createProject('tsconfig.json');
const clean = require('gulp-clean');
const runSequence = require('run-sequence');
const YAML = require('yamljs');
const fs = require('fs');

const paths = {
    src: 'src/**/*.ts',
    spec: 'dist/spec/**/*.js',
    dest: 'dist'
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

gulp.task('toYaml', () => {
    try {
        const content = fs.readFileSync(getFilePath()).toString();
        const jsonObject = JSON.parse(content);
        const yamlString = YAML.stringify(jsonObject, 8, 2); //indent 2 space
        console.log(`\n${yamlString}\n`);
    } catch (ex) {
        console.log(ex);
    }
});

gulp.task('toJson', () => {
    try {
        const content = fs.readFileSync(getFilePath()).toString();
        const jsonObject = YAML.parse(content);
        const jsonString = JSON.stringify(jsonObject, null, 2); //indent 2 space
        console.log(`\n${jsonString}\n`);
    } catch (ex) {
        console.log(ex);
    }
});

const getFilePath = () => {
    const regex = /--file\s*=\s*([\w\.]+)/;
    const matches = regex.exec(process.argv);
    if (matches && matches.length > 1) {
        return `./src/files/${matches[1]}`; //group capture environment name
    } else {
        throw 'please provide a file with --file=file-name argument'
    }
};