const gulp = require("gulp");
const clean = require("gulp-clean");
const runSequence = require("run-sequence");
const source = require("vinyl-source-stream");
const buffer = require("vinyl-buffer");
const browserify = require("browserify");
const tsify = require("tsify");
const eventStream = require('event-stream');
const rename = require("gulp-rename");

let paths = {
    src: [
        "src/**/*.ts",
    ],
    mainFiles : [
        "src/extension/content.ts",
        "src/extension/background.ts",
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
    let tasks = paths.mainFiles.map(file => {
        return browserify({
                entries: file,
                debug: false //turn on/off source map 
            })
            .plugin(tsify)
            .bundle()
            //http://maximilianschmitt.me/posts/prevent-gulp-js-from-crashing-on-error/
            .on("error", error => {
                console.log(error);
            })
            .pipe(source(file))
            .pipe(rename({
                dirname: ".",
                extname: '.js'
            }))
            .pipe(buffer())
            .pipe(gulp.dest("./dist/extension/"));
    });

    return eventStream.merge.apply(null, tasks);
});

gulp.task("deploy", done => {
    //start with clean, compile and test respectively 
    runSequence("clean", "compile", "copy", () => {
        done();
    });
});

gulp.task("watch", ["deploy"], () => {
    gulp.watch(paths.src.concat(paths.content), ["deploy"]);
});