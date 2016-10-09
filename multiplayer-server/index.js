var express = require('express');
var app = express();

// Setup the server for heroku cloud application
var server = require('http').createServer(app);
var io = require('socket.io').listen(server);

app.set('port', process.env.PORT || 3000);

var clients = [];

// The client connects using this method
io.on("connection", function(socket) {
	var currentUser;
	
	// Get the brodcast messages comming from the client
	socket.on("USER_CONNECT", function() {
		console.log("User Connected");
		
		for(let client of clients) {
			// Send each player data to the client
			socket.emit("USER_CONNECTED", { name: client.name, position: client.position });
			
			console.log("User Name " + client + " is connected");
		}
	});
	
    // A new user wants to play. Add them to the server list
	socket.on("PLAY", function(data) {
		
		console.log(data);
		
		currentUser = {
			name: data.name,
			position: data.position
		};
		
        // Add the currentUser to the clients list
		clients.push(currentUser);
		// Send a message back to the client
        // indicating that a user has been connected
        socket.emit("USER_CONNECTED", currentUser);
	});
	
	socket.on("MOVE", function(data) {
		currentUser.position = data.position;
		socket.emit("MOVE", currentUser);
		socket.broadcast.emit("MOVE", currentUser);
		console.log(currentUser.name + " has moved to " + currentUser.position);
	});
	
	socket.on("disconnect", function() {
		socket.broadcast.emit("USER_DISCONNECTED", currentUser);
		for(var i = 0; i < clients.length; i++) {
			if(clients[i].name === currentUser.name) {
				console.log(clients[i].name + " has disconnected");
				clients.splice(i, 1);
			}
		}
	});
	
});

server.listen(app.get('port'), function() {
	console.log("Server is listening on port 3000")
});
