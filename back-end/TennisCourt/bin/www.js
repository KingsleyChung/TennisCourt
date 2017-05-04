#!/usr/bin/env node

/**
 * Module dependencies.
 */
var debug = require('debug')('project:server');
var http = require('http');
var MongoClient = require('mongodb').MongoClient;
var MongoUrl = 'mongodb://localhost:27017/tennis';

var server;
MongoClient.connect(MongoUrl, function (err, db) {
    if(err) {
        console.log("fail to connect mongodb: " + err);
    } else {
        console.log("Successfully connect to mongodb");
        var app = require('../app')(db);
        /**
         * Get port from environment and store in Express.
         */
        var port = normalizePort(process.env.PORT || '3000');
        app.set('port', port);

        /**
         * Create HTTP server.
         */
        server = http.createServer(app);

        /**
         * Listen on provided port, on all network interfaces.
         */
        server.listen(port);
        server.on('error', onError);
        server.on('listening', onListening);

    }
});

/**
 * Normalize a port into a number, string, or false.
 */

function normalizePort(val) {
  var port = parseInt(val, 10);

  if (isNaN(port)) {
    // named pipe
    return val;
  }

  if (port >= 0) {
    // port number
    return port;
  }

  return false;
}

/**
 * Event listener for HTTP server "error" event.
 */

function onError(error) {
  if (error.syscall !== 'listen') {
    throw error;
  }

  var bind = typeof port === 'string'
    ? 'Pipe ' + port
    : 'Port ' + port;

  // handle specific listen errors with friendly messages
  switch (error.code) {
    case 'EACCES':
      console.error(bind + ' requires elevated privileges');
      process.exit(1);
      break;
    case 'EADDRINUSE':
      console.error(bind + ' is already in use');
      process.exit(1);
      break;
    default:
      throw error;
  }
}

/**
 * Event listener for HTTP server "listening" event.
 */

function onListening() {
  var addr = server.address();
  var bind = typeof addr === 'string'
    ? 'pipe ' + addr
    : 'port ' + addr.port;
  debug('Listening on ' + bind);
}