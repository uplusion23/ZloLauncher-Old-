var serverTable = $(".main .multiplayer .servers table");
var returnToMenu = $(".backToMenu");
var menu = $(".menu");
var main = $(".main");
var login = $(".log-in");

var isLoggedIn = false;
var rememberInfo = false;


var dummyNames = [
  '[EU] Clan 4 Server (2x)',
  '[NZ] Southwest New Zeal',
  '[US-W] Another Fake Server',
  '[RU] ZloFenix Stock Server',
  'uplusion23s Funhouse',
  '4x Experience, Knife Only Server | TLD Gamers',
  'Sith\'s NA Server'
];



$(document).ready(function () {
  if (rememberInfo) {
    $(".log-in .dialog .switch input[type=\"checkbox\"]").prop('checked', true);
    app.Remember();
  }

  $(".dev").click(function () {
    addDummyServer();
  });

  if (isLoggedIn) {
    main.removeClass("hidden");
    login.addClass("hidden");
  }

  $("#signin").click(function () {
    app.Login($("#email").val(), $("#pass").val());
  });

  $("#singleplayer").hover(function () {
    $(".tooltip").html("Launch single player.");
  });
  $("#multiplayer").hover(function () {
    $(".tooltip").html("Play with other players in gamemodes.");
  });
  $("#mysoldier").hover(function () {
    $(".tooltip").html("View your stats.");
  });
  $("#settings").hover(function () {
    $(".tooltip").html("Configure the game.");
  });
  $("#credits").hover(function () {
    $(".tooltip").html("View the credits.");
  });
  $("#quit").hover(function () {
    $(".tooltip").html("Exit the launcher.");
  });

  $("#singleplayer").click(function () {
    app.SinglePlayer();
  });

  $("#multiplayer").click(function () {
    menu.addClass("hidden");
    $(".multiplayer").addClass("shown");
  });

  $("#mysoldier").click(function () {
    menu.addClass("hidden");
    $(".mysoldier").addClass("shown");
  });

  $("#settings").click(function () {
    menu.addClass("hidden");
    $(".settings").addClass("shown");
  });

  $("#credits").click(function () {
    menu.addClass("hidden");
    $(".credits").addClass("shown");
  });

  $("#quit").click(function () {
    app.Exit();
  });

  returnToMenu.click(function () {
    menu.removeClass("hidden");
    $(".shown").removeClass("shown");
  });

  $('.main .settings .container .row .item').click(function(){
		var tab_id = $(this).attr('data-tab');

		$('.main .settings .container .row .item.active').removeClass('active');
		$('.main .settings .container .settingsTab').removeClass('active');

		$(this).addClass('active');
		$("#"+tab_id).addClass('active');
	})
});

function LoggedIn(username, clantag) {
  isLoggedIn = true;
  main.removeClass("hidden");
  login.addClass("hidden");
  if (clantag != null) {
    $("#username").html("[" + clantag + "] " + username);
  } else {
    $("#username").html(username);
  }
}

function addDummyServer() {
  var randomNumber = Math.floor(Math.random()*dummyNames.length);
  addServerToList("", dummyNames[randomNumber], (Math.floor(Math.random() * 32) + 1), 32, "Ranked", (Math.floor(Math.random() * 160) + 1  ), "Conquest", "Tremors");
}

function addServerToList(fav, name, currentPlayers, maxPlayers, type, ping, gamemode, map) {
  var serverVar;
  serverVar +=  '<tr>';
  serverVar +=    '<td>★</td>';
  serverVar +=    '<td>';
  serverVar +=      '<span class="servername">' + name + '</span>';
  serverVar +=      gamemode + ' | ' + map;
  serverVar +=    '</td>';
  serverVar +=    '<td>' + currentPlayers + '/' + maxPlayers + '</td>';
  serverVar +=    '<td>' + type + '</td>';
  serverVar +=    '<td>' + ping + '</td>';
  serverVar +=  '</tr>';
  $(serverTable).append(serverVar);
}
