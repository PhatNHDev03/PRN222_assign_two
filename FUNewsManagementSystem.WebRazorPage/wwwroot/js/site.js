// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();


connection.on("LoadAllTags", function(){
    location.href = '/Tags/Index';
});
connection.on("LoadAllNewArticles", function () {
    location.href = '/NewsArticles/Index';
});
/*connection.on("LoadHomePage", function () {
    location.href = '/Index';
});*/

connection.start().catch(function (err) {
    return console.error(err.toString());
});