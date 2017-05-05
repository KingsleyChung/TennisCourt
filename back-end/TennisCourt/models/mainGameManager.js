/**
 * Created by weimumu on 2017/5/5.
 */
var mainGames;
module.exports = function (db) {
  mainGames = db.collection('mainGames');

  var mainGameManager = {
      //增加赛事
      validateCreate : function (mainGame) {
          return mainGames.findOne({matchId : mainGame.matchId}).then(function (value) {
                return new Promise(function(resolve, reject){
                    if(value) {
                        value.error = "This Match has been created before.";
                        value.ok = "0";
                        reject(value);
                    }
                    else {
                        resolve(mainGame);
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
      }
  };
  return mainGameManager;
};