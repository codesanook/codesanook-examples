//https://www.typescriptlang.org/docs/handbook/migrating-from-javascript.html
//https://www.typescriptlang.org/docs/handbook/gulp.html
const gulp = require("gulp");
const ts = require("gulp-typescript");

var tsProject = ts.createProject("tsconfig.json");

gulp.task("compile",  () => {
 return tsProject.src()
        .pipe(tsProject())
        .js.pipe(gulp.dest("spec"));

});
//https://jasmine.github.io/setup/nodejs.html