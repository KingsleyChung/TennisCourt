var express = require('express');
var router = express.Router();

module.exports = function (db) {
    var userManager = require('../models/userManager')(db);
    router.get('/', function (req, res, next) {
        if(req.session.user) {
            if(req.query.username && req.query.username != req.session.user.username) {
                req.session.err = {};
                req.session.err.message = "you can only access your detail";
            }
            res.redirect('/detail');
        }
        else {
            res.redirect('/signin');
        }
    });

    router.get('/regist', function (req, res, next) {
        res.render('signup', {title : 'Signup', user : {}, err : {}});
    });

    router.get('/signin', function (req, res, next) {
        var a = {};
        a.username = "weimumu";
        res.send(a);
        res.status(200).end();
       /* req.session.user
        ? res.redirect('/detail')
        : res.render('signin', {title : 'Signin', user : {}, err : {}});*/
    });

    router.get('/detail', function (req, res, next) {
        var err = req.session.err;
        if(!err) {
            err = {};
        }
        delete req.session.err;
        req.session.user
        ? res.render('detail', {title : 'Detail', user : req.session.user, err : err})
        : res.redirect('/signin');
    });

    router.get('/signout', function (req, res, next) {
        req.session.destroy(function (err) {
            res.clearCookie("connect.sid");
            res.redirect('/signin');
        })
    });

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

