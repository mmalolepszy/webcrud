/// <binding BeforeBuild='make:resources:public' Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    replace = require("gulp-replace");

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.pluginExtensions = paths.webroot + "js/Application/plugins.extensions.js";
paths.pluginExtensionsDest = paths.webroot + "js/Application/plugins.extensions.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs, "!" + paths.pluginExtensions], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:js:pluginExtensions", function () {
    return gulp.src([paths.pluginExtensions], { base: "." })
        .pipe(concat(paths.pluginExtensionsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css", "min:js:pluginExtensions"]);

//replace internal with public in resource file on project build
//to make resources working on view model classes
gulp.task("make:resources:public", function () {
    return gulp.src("./Resources/Labels.Designer.cs", { base: './' })
        .pipe(replace('internal', 'public'))
        .pipe(gulp.dest("./"));
});
