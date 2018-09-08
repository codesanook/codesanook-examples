const gulp = require("gulp");
const jasmine = require("gulp-jasmine");
const clean = require("gulp-clean");
const runSequence = require("run-sequence");
const ts = require("gulp-typescript");
const source = require("vinyl-source-stream");
const browserify = require("browserify");
const tsify = require("tsify");

let paths = {
    src: [
        "src/**/*.ts",
    ],
    content: [
        "src/**/*.json",
        "src/**/*.html",
        "src/**/*.css",
        "src/**/*.png",
    ],
    spec: "dist/spec/**/*.js",
    dest: "dist",
};

gulp.task("clean", () => {
    return gulp.src(paths.dest, {
            read: false
        })
        .pipe(clean());
});

gulp.task("copy", () => {
    return gulp.src(paths.content)
        .pipe(gulp.dest(paths.dest));
});


gulp.task("compile", () => {

    let b = browserify({
        basedir: ".",
        debug: false, //turn on/off source map 
        entries: [
            "./src/extension/Content.ts"
        ],
        cache: {},
        packageCache: {}
    });

    //b.ignore("jquery"); //and include it directly
    //http://maximilianschmitt.me/posts/prevent-gulp-js-from-crashing-on-error/
    b.plugin(tsify)
        .bundle()
        .on("error", error => {
            console.log(error);
        })
        .pipe(source("Content.js"))
        .pipe(gulp.dest("./dist/extension/"));
});

//test task depends on clean and compile tasks 
gulp.task("test", done => {
    //start with clean, compile and test respectively 
    runSequence("clean", "compile", "copy", () => {
        gulp.src(paths.spec)
            .pipe(jasmine({
                verbose: true
            }));
        done();
    });
});

gulp.task("watch", ["test"], () => {
    gulp.watch(paths.src.concat(paths.content), ["test"]);
});