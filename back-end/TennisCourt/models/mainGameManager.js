/**
 * Created by weimumu on 2017/5/5.
 */
var mainGames;
var num = 0;
module.exports = function (db) {
  mainGames = db.collection('mainGames');

  var mainGameManager = {
      //增加赛事
      validateCreate : function (mainGame) {
          mainGame.matchId = "" + num;
          return mainGames.findOne({matchTitle : mainGame.matchTitle}).then(function (value) {
                return new Promise(function(resolve, reject){
                    if(value) {
                        value.error = "This Match has been created before.";
                        value.ok = "0";
                        reject(value);
                    }
                    else {
                        resolve(mainGame);
                        num++;
                    }
                });
          });
      },
      validateAdd : function (mainGame) {
          return new Promise(function (resolve, reject) {
              mainGames.insertOne(mainGame, function () {
                  resolve();
              })
          });
      },

      //结束赛事
      validateEnd: function (matchId) {
          return mainGames.updateOne({matchId : matchId}, {$set:{status:"0"}}).then(function (value) {
              return new Promise(function (resolve, reject) {
                  var endMatchError = {};
                  if(value.modifiedCount) {
                      endMatchError.ok = "1";
                      resolve(endMatchError);
                  }
                  else {
                      endMatchError.ok = "0";
                      endMatchError.error = "This match is not existing";
                      reject(endMatchError);
                  }
              });
          });
      },

      //返回正在进行的赛事
      matchReturn : function () {
          return mainGames.find({status : "1"}).toArray().then(function (value) {
              return new Promise(function (resolve, reject){
                  if(value.length) {
                      var message = {};
                      message.error = "";
                      message.ok = "1";
                      message.match = value;
                      console.log(message);
                      resolve(message);
                  }
                  else {
                      var matchReturnError = {};
                      matchReturnError.error = "NO Match";
                      matchReturnError.ok = "0";
                      reject(matchReturnError);
                  }
              });
          });
      },

      //放回所有赛事
      returnAllMatch : function () {
          return mainGames.find().toArray().then(function (value) {
              return new Promise(function (resolve, reject){
                  if(value.length) {
                      var message = {};
                      message.error = "";
                      message.ok = "1";
                      message.match = value;
                      //console.log(message);
                      resolve(message);
                  }
                  else {
                      var matchReturnError = {};
                      matchReturnError.error = "NO Match";
                      matchReturnError.ok = "0";
                      reject(matchReturnError);
                  }
              });
          });
      }
  };
  return mainGameManager;
};