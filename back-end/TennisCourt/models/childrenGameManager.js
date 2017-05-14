/**
 * Created by weimumu on 2017/5/7.
 */


var childrenGames;
var num = 0;
var type1 = ['manS', 'womenS', 'manD', 'womenD', 'Double'];
var type = ['男单', '女单', '男双', '女双', '混双'];
function returnDate(mode) {
    var date = new Date();
    if(mode == 0) {
        var year = date.getFullYear();
        var month = (date.getMonth() + 1) < 10 ? ("-0" +  (date.getMonth() + 1)) : "-" + (date.getMonth() + 1);
        var day = date.getDate() < 10 ? ("-0" +  date.getDate()) : "-" + date.getDate();
        return year + month + day;
    }
    else {
        var hour = (date.getHours() < 10) ? "0" + date.getHours() : date.getHours();
        var minute = (date.getMinutes() < 10) ? ":0" + date.getMinutes() : ":" + date.getMinutes();
        return hour + minute;
    }
}
module.exports = function (db) {

    var mainGameManager = require('../models/mainGameManager')(db);
    childrenGames = db.collection('childrenGames');
    function returnMatchName() {
        return mainGameManager.returnAllMatch().then(function (value) {
            var tasks = [];
            for(var i in value.match) {
                tasks.push(value.match[i].matchId);
            }
            return new Promise(function (resolve, reject) {
                resolve(tasks);
            });
        })
    }

    async function returnOneMatch(parentId) {
        var message = {};
        for(var j in type) {
            let returnValue = await childrenGames.find({parentId : parentId, catagory : type[j]}).toArray();
            message[type1[j]] = returnValue;
        }
        return new Promise(function (resolve, reject) {
            resolve(message);
        });
    }

    var childrenGamesManager = {
        returnAllGame : async function () {
            try {
                let res = await returnMatchName();
                var returnMatchMessage = {};
                for(var i in res) {
                    let message = await returnOneMatch(res[i]);
                    returnMatchMessage[res[i]] = message;
                }
                return new Promise(function (resolve, reject) {
                   resolve(returnMatchMessage);
                });
            } catch(err) {
                console.log(err);
            }
        },

        childrenGameCreate : function (childrenGame) {
            var str = returnDate(0);
            var alresult = [];
            childrenGame.date = str;
            childrenGame.status = "1";
            childrenGame.parentId = childrenGame.matchId;
            childrenGame.matchId += "/" + num;
            childrenGame.result = "0-0";
            alresult.push("0-0");
            childrenGame.allresult = alresult;
            return new Promise(function (resolve, reject) {
                childrenGames.insertOne(childrenGame, function () {
                    num++;
                    resolve(childrenGame);
                })
            });
        },

        childrenGameStatusChange : function (matchId, status, flag) {
            var str = new Date();
            if(status == "0") {
                return childrenGames.updateOne({matchId: matchId}, {$set: {status: status, startTime : str.toLocaleString(),  flag : flag}}).then(function (value) {
                    return new Promise(function (resolve, reject) {
                        var changeError = {};
                        console.log(value);
                        if (value.modifiedCount) {
                            changeError.ok = "1";
                            resolve(changeError);
                        }
                        else {
                            changeError.ok = "0";
                            changeError.error = "Change Status Error";
                            reject(changeError);
                        }
                    });
                });
            }
            else {
                return childrenGames.updateOne({matchId: matchId}, {$set: {status: status, endTime : str.toLocaleString()}}).then(function (value) {
                    return new Promise(function (resolve, reject) {
                        console.log(value);
                        var changeError = {};
                        if (value.modifiedCount) {
                            changeError.ok = "1";
                            resolve(changeError);
                        }
                        else {
                            changeError.ok = "0";
                            changeError.error = "Change Status Error";
                            reject(changeError);
                        }
                    });
                });
            }
        },

        returnDayGame : function () {
            var str = returnDate(0);
            console.log(str);
            return childrenGames.find({date : returnDate(0)}).sort({status : 1}).toArray().then(function (value) {
                return new Promise(function (resolve, reject){
                    if(value.length) {
                        var message = {};
                        message.games = value;
                        message.error = "";
                        message.ok = "1";
                        console.log(message);
                        resolve(message);
                    }
                    else {
                        var returnDayGameError = {};
                        returnDayGameError.ok = "0";
                        returnDayGameError.error = "No Game Today";
                        reject(returnDayGameError);
                    }
                });
            });
        },
        
        changeScore : function (matchId, game, result, score) {
            var str = "game" + game;
            return childrenGames.findOne({matchId : matchId}).then(function (value) {
                if(!value) {
                    var resetError = {};
                    resetError.ok = "0";
                    resetError.error = "The Database don't has this game";
                    return new Promise(function (resolve, reject) {
                        reject(resetError);
                    })
                }
                value.totalGames = game;
                value.result = result;
                value.finalscore = score;
                console.log(value);
                if(value.hasOwnProperty(str)) {
                    value[str].push(score);
                    return childrenGames.findOneAndReplace({matchId : matchId}, value).then(function () {
                       return new Promise(function (resolve, reject) {
                          resolve(value);
                       });
                    });
                }
                else {
                    var array = [];
                    array.push(score);
                    value[str] = array;
                    if(result != "0-0") {
                    value.allresult.push(result);
                }
                    return childrenGames.findOneAndReplace({matchId : matchId}, value).then(function () {
                        return new Promise(function (resolve, reject) {
                            resolve(value);
                        });
                    });
                }
            });
        },
        
        resetScore : function (matchId, score) {
            return childrenGames.findOne({matchId : matchId}).then(function (value) {
               var num = 1;
               var str = "game" + num;
               if(!value) {
                   return new Promise(function (resolve, reject) {
                       var resetError = {};
                       resetError.ok = "0";
                       resetError.error = "The Database don't has this game";
                       reject(resetError);
                   })
               }
               while(value.hasOwnProperty(str)) {
                   num++;
                   str = "game" + num;
               }
               var gamenum = "game" + (num - 1);
               var item = value[gamenum];
               if(item[item.length - 1] == score) {
                   value[gamenum].pop();
                   value.finalscore = value[gamenum][value[gamenum].length - 1];
                   return childrenGames.findOneAndReplace({matchId : matchId}, value).then(function () {
                       return new Promise(function (resolve, result) {
                          resolve(value);
                       });
                   })
               }
               else {
                   return new Promise(function (resolve, reject) {
                       var resetError = {};
                       resetError.ok = "0";
                       resetError.error = "Reset score error";
                       return new Promise(function (resolve, result) {
                           reject(resetError);
                       })
                   })
               }
            });
        },

        returnOneMatchGames : async function (matchId) {
            var returnValue = {};
            let message = await childrenGames.find({parentId : matchId}).toArray();
            return new Promise(function (resolve, reject) {
                if(message.length) {
                    returnValue.games = message;
                    returnValue.ok = "1";
                    returnValue.error = "";
                    resolve(returnValue);
                }
                else {
                    returnValue.error = "This Match has no games";
                    returnValue.ok = "0";
                    reject(returnValue);
                }
            })
        }
    };
    return childrenGamesManager;
};