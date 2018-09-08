const gulp = require("gulp");
const jasmine = require("gulp-jasmine");
const clean = require("gulp-clean");
const runSequence = require("run-sequence");
const ts = require("gulp-typescript");
const tsProject = ts.createProject("tsconfig.json");

let paths = {
    src: [
        "src/**/*.ts",
    ],
    content:[
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
    return gulp.src(paths.src)
        .pipe(tsProject())
        .pipe(gulp.dest(paths.dest));
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