/**
 * Created by weimumu on 2017/5/7.
 */
var childrenGames;
var num = 0;
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
    childrenGames = db.collection('childrenGames');
    var childrenGamesManager = {
        childrenGameCreate : function (childrenGame) {
            childrenGame.date = returnDate(0);
            childrenGame.status = "-1";
            childrenGame.matchId += "/" + str + "/" + num;
            childrenGame.result = "0-0";
            console.log(str);
            return new Promise(function (resolve, reject) {
                childrenGames.insertOne(childrenGame, function () {
                    num++;
                    resolve(childrenGame);
                })
            });
        },

        childrenGameStatusChange : function (matchId, status) {
            var str = returnDate(1);
            if(status == "0") {
                return childrenGames.updateOne({matchId: matchId}, {$set: {status: status, startTime : str}}).then(function (value) {
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
                return childrenGames.updateOne({matchId: matchId}, {$set: {status: status, endTime : str}}).then(function (value) {
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
            return childrenGames.find({date : returnDate(0)}).toArray().then(function (value) {
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
                value.totalGames = game;
                value.result = result;
                console.log(value);
                if(value.hasOwnProperty(str)) {
                    value[str] += score + ",";
                    return childrenGames.findOneAndReplace({matchId : matchId}, value).then(function () {
                       return new Promise(function (resolve, reject) {
                          resolve(value);
                       });
                    });
                }
                else {
                    value[str] = score + ",";
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
               while(value.hasOwnProperty(str)) {
                   num++;
                   str = "game" + num;
               }
               var gamenum = "game" + (num - 1);
               var testscore = value[gamenum].substr(0, value[gamenum].length - 1);
               var tot = testscore.lastIndexOf(",");
               console.log(testscore);
               console.log(testscore.substr(tot + 1));
               if(testscore.substr(tot + 1) == score) {
                   value[gamenum] = value[gamenum].substr(0, tot + 1);
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
        }
    };
    return childrenGamesManager;
};