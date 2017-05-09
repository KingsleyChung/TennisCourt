var express = require('express');
var router = express.Router();

module.exports = function (db) {
    var userManager = require('../models/userManager')(db);
    var mainGameManager = require('../models/mainGameManager')(db);
    var childrenGameManager = require('../models/childrenGameManager')(db);
    childrenGameManager.returnAllGame();
    //字比赛创建相关api
    //创建一个子比赛
    router.post('/creategame', function (req, res, next) {
        var childrenGame = req.body;
        console.log(childrenGame);
        childrenGameManager.childrenGameCreate(childrenGame).then(function (value) {
            console.log(value);
            res.send(value).status(200).end();
        })
    });
    //改变比赛状态
    router.post('/changegame', function (req, res, next) {
       var matchId = req.body.matchId;
       var status = req.body.status;
       childrenGameManager.childrenGameStatusChange(matchId, status).then(function (value) {
           res.send(value).status(200).end();
       }).catch(function (error) {
           res.send(error).status(204).end();
       });
    });
    //返回当天比赛
    router.get('/returngame', function (req, res, next) {
        childrenGameManager.returnDayGame().then(function (value) {
            res.send(value).status(200).end();
        }).catch(function (error) {
            res.send(error).status(204).end();
        });
    });
    //修改比赛比分
    router.post('/changescore', function (req, res, next) {
       var matchId= req.body.matchId;
       var game= req.body.game;
       var result = req.body.result;
       var score = req.body.score;
       console.log(req.body);
       childrenGameManager.changeScore(matchId, game, result, score).then(function (value) {
          res.send(value).status(200).end();
       });
    });
    //撤回比赛最后一个比分
    router.post('/resetscore', function (req, res, next) {
       var matchId = req.body.matchId;
       var score = req.body.score;
       console.log(req.body);
       childrenGameManager.resetScore(matchId, score).then(function (value) {
           res.send(value).status(200).end();
       }).catch(function (error) {
          res.send(error).status(204).end();
       });
    });
    //放回所有赛事的子比赛
    router.get('/allgame', async function (req, res, next) {
       try {
           let value = await childrenGameManager.returnAllGame();
           res.send(value).status(200).end();
       } catch(error) {
           res.send(error).statue(204).end();
       }
    });


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
    //结束一个赛事
    router.post('/endmatch', function (req, res, next) {
       var matchId =  req.body.matchId;
       console.log(matchId);
       mainGameManager.validateEnd(matchId).then(function (value) {
           res.send(value).status(200).end();
       }).catch(function (error) {
           res.send(error).status(204).end();
       })
    });
    //返回正在进行的赛事
    router.get('/matching', function (req, res, next) {
        mainGameManager.matchReturn().then(function (value) {
            console.log(value);
            res.send(value).status(200).end();
        }).catch(function (error) {
            console.log(value);
            res.send(error).status(204).end();
        })
    });
    //返回所有赛事
    router.get('/allmatch', function (req, res, next) {
        mainGameManager.returnAllMatch().then(function (value) {
           res.send(value).status(200).end();
        }).catch(function (error) {
           res.send(error).status(204).end();
        });
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

