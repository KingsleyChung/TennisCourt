var express = require('express');
var router = express.Router();

module.exports = function (db) {
    var userManager = require('../models/userManager')(db);
    var mainGameManager = require('../models/mainGameManager')(db);

    //赛事创建相关api
    //创建一个赛事  例如2017年中东网球公开赛
    router.post('/creatematch', function (req, res, next) {
       var mainGame = req.body;
       mainGameManager.validateCreate(mainGame).then(function (value) {
           mainGameManager.validateAdd(mainGame).then(function () {
               value.error = "";
               value.ok = "1";
               res.send(value).status(200).end();
           });
       }).catch(function (error) {
           console.log(error);
           res.send(error).status(204).end();
       })
    });
    router.post('/endmatch', function (req, res, next) {
       var matchId =  req.body.matchId;
       console.log(matchId);
       mainGameManager.validateEnd(matchId).then(function (value) {
           res.send(value).status(200).end();
       }).catch(function (error) {
           res.send(error).status(204).end();
       })
    });


    //登录注册相关api
    //注册
    router.post('/regist', function (req, res, next) {
       var user = req.body;
       console.log(user);
       userManager.validateSignup(user).then(function(value) {
           console.log(user);
           userManager.addUserToDB(user).then(function () {
              delete user.password;
              user.error = "";
              user.ok = "1";
              req.session.user = user;
              res.send(user).status(200).end();
           });
       }).catch(function (error) {
           console.log(error);
           res.send(error).status(204).end();
       });
    });
    //登录
    router.post('/signin', function (req, res, next) {
       var password = req.body.password;
       var username = req.body.username;
       userManager.validateSignin(username, password).then(function (user) {
            user.error = "";
            user.ok = "1";
            delete user.password;
            console.log(user);
            res.send(user);
            res.status(200).end();
       }).catch(function (err) {
           err.ok = "0";
           console.log(err);
           res.send(err);
           res.status(204).end();
       })
    });
    return router;
};

