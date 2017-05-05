/**
 * Created by weimumu on 2017/4/30.
 */
var validator = require('../public/javascripts/validator');
var bcrypt = require('bcrypt-nodejs');

var users;

module.exports = function (db) {
    users = db.collection('users');
    var userManager = {
        validateSignin : function (username, passsword) {
             return users.findOne({username : username}).then(function(user){
                 return new Promise(function (resolve, reject) {
                     var errInSignin = {};
                    if(!user) {
                        errInSignin.error = "The account doesn't exist";
                        reject(errInSignin);
                    }
                    var encodedPassword = user.password;
                    bcrypt.compare(passsword, encodedPassword, function (err, result) {
                        if(!result) {
                            errInSignin.error = "password is wrong";
                            reject(errInSignin);
                        }else {
                            console.log(user.mode);
                            resolve(user);
                        }
                    });
                 });
             });
        },
        
        validateSignup : function (user) {
            var errInSignup = "";
            var tasks = [];
            var query = {};
            for(let key in user) {
                if(key == "password" || key == "mode") {
                    continue;
                }
                query[key] = user[key];
                tasks.push(users.findOne(query).then(function(user){
                    if(user) {
                        errInSignup += "This " + key + " is already used.";
                    }
                }));
                delete query[key];
            }
            return Promise.all(tasks).then(function () {
               if(!errInSignup) {
                   return Promise.resolve();
               }
               else {
                   delete user.password;
                   user.error = errInSignup;
                   user.ok = "0";
                   return Promise.reject(user);
               }
            });
            /*return users.findOne({username : user.username}).then(function (user) {
                return new Promise(function (resolve, reject) {
                    if(user) {
                        delete user.password;
                        user.error = "This account has existed";
                        user.ok = "0";
                        reject(user);
                    }
                    else {
                        resolve();
                    }
                })
            })*/
        },
        
        addUserToDB : function (user) {
            return new Promise(function (resolve, reject) { 
                bcrypt.genSalt(10, function (err, salt) {
                    bcrypt.hash(user.password, salt, null, function (err, hash) {
                        user.password = hash;
                        users.insertOne(user, function () {
                            resolve();
                        })
                    });
                });
            });
        }
    };
    return userManager;
};
