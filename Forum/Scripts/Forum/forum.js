function sendMessage(url, threadName, themeName, pageIndex) {
	var text = $("#message").val();

	$.ajax({
		url: "/Thread/SendMessage",
		type: "POST",
		data: {
			'threadName': threadName,
			'returnUrl': url,
			'themeName': themeName,
			'pageIndex': pageIndex,
			'message': text
		},
		dataType: "json",
		success: function(data) {
			putMessage(data.Time, data.Name, data.Text, threadName, themeName, pageIndex);
		}
	});
}

function putMessage(time, name, text, threadName, themeName, pageIndex) {
	var messageIndex = 0;
	while (messageIndex <= 10) {
		if ($("*").is("#" + messageIndex)) {
			messageIndex = parseInt(messageIndex, 10) + 1;
		} else {
			break;
		}
	}
	$("#send-form").before(
		  '<li id="'+messageIndex+'">						'
		+ '	<hr />											'
		+ '	<div class="message-block">						'
		+ '		<div class="forum-message forum-author">	'
		+ '			<a>' + name + '</a>						'
		+ '<span class="glyphicon glyphicon-remove" onclick="deleteMessage(\''+messageIndex+'\', \''+threadName+'\', \''+themeName+'\', \''+pageIndex+'\')"></span>'
		+ '		</div>										'
		+ '		<div class="forum-message forum-text">		'
		+ '			<a>'+text+'</a>							'
		+ '		</div>										'
		+ '		<div class="forum-message forum-time">		'
		+ '			<a>'+time+'</a>							'
		+ '		</div>										'
		+ '	</div>											'
		+ '</li>											');
}

var threadName = "";

function getAskBox(thread) {
	threadName = thread;
    $(".hidden-overlay").removeClass("hidden-overlay").addClass("visible-overlay");
    $("body").addClass("overlay");
}

function sendTheme(url, isAdmin) {
	var text = $("#ask-theme-name").val();

	$.ajax({
		url: "/Home/CreateTheme",
		type: "POST",
		data: {
			'threadName': threadName,
			'returnUrl': url,
			'themeName': text
		},
		dataType: "json",
		success: function(data) {
			if (data.Result) {
				$(".visible-overlay").addClass("hidden-overlay").removeClass("visible-overlay");
				$("body").removeClass("overlay");
				var lastSelector = isAdmin ? "span" : "a";
			    var htmlText =
			        '<a class="no-decoration" href="/Thread/Thread?threadName=' + threadName + '&themeName=' + text + '">	' +
			            '	<li class="theme-name">	' +
			            text +
			            '	</li>					' +
			            '</a>						';
			    if (isAdmin)
			        htmlText += '<span class="remove-theme-icon glyphicon glyphicon-remove" onclick="deleteTheme(\'' + threadName + '\', \'' + text + '\')"></span>';
			    $("#" + safelySelector(threadName)).find("ul").find(lastSelector).last().after(htmlText);
			} else {
				alert(data.Message);
			}
		},
		error: function() {
			alert("error");
		}
	});
}

function safelySelector(selector) {
	var goodSelector = "";
	for (var i = 0; i < selector.length; i++) {
		if (selector[i] === "+" || selector[i] === "#" || selector[i] === "[" || selector[i] === "]" || selector[i] === ".") {
			goodSelector = goodSelector + "\\" + selector[i];
		} else {
			goodSelector = goodSelector + selector[i];
		}
	}
	return goodSelector;
}

function closeAskTheme(selector) {
	$(".visible-overlay").addClass(selector).removeClass("visible-overlay");
	$("body").removeClass("overlay");
}

function getAskThreadBox() {
    $(".hidden-thread-overlay").removeClass("hidden-thread-overlay").addClass("visible-overlay");
    $("body").addClass("overlay");
}

function sendThread() {
	var text = $("#ask-thread-name").val();

	$.ajax({
		url: "/Home/CreateThread",
		type: "POST",
		data: {
			'threadName': text
		},
		dataType: "json",
		success: function(data) {
		    if (data.Result) {
		        $(".visible-overlay").addClass("hidden-thread-overlay").removeClass("visible-overlay");
		        $("body").removeClass("overlay");
				$("#create-thread").before(
					'<li id="' + text + '" class="thread-block">							' +
					'	<div class="thread-name-block">										' +
					'		<a class="message-block thread-name">							' +
					text +
                    '<span class="glyphicon glyphicon-remove" onclick="deleteThread(\'' + text +'\')"></span>'+
					'		</a>															' +
					'		<div class="create-theme" onclick="getAskBox(\'' + text + '\')">' +
					'			Create new Theme											' +
					'		</div>															' +
					'	</div>																' +
					'	<ul class="themes-block"><a></a>									' +
					'	</ul>																' +
					'</li>																	'
				);
			} else {
				alert(data.Message);
			}
		},
		error: function() {
			alert("error");
		}
	});
}

function deleteMessage(messageIndex, threadName, themeName, pageIndex) {
	$.ajax({
		url: "/Thread/DeleteMessage",
		type: "POST",
		data: {
			'messageIndex': messageIndex,
			'threadName': threadName,
			'themeName': themeName,
			'pageIndex': pageIndex
		},
		dataType: "json",
		success: function(data) {
			if (data.Result) {
				$("#" + messageIndex).remove();
				while (true) {
					messageIndex = parseInt(messageIndex, 10) + 1;
					if ($("*").is("#" + messageIndex)) {
						$("#" + messageIndex).attr("id", (parseInt(messageIndex, 10) - 1));
					} else {
						break;
					}
				}
			} else {
				alert(data.Message);
			}
		},
		error: function() {
			alert("error");
		}
	});
}

function deleteTheme(thread, theme) {
    $.ajax({
        url: "/Home/DeleteTheme",
        type: "POST",
        data: {
            'threadName': thread,
            'themeName': theme
        },
        dataType: "json",
        success: function (data) {
            if (data.Result) {
                var themeElement = $("#" + theme);
                themeElement.next().remove();
                themeElement.remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function deleteThread(thread) {
    $.ajax({
        url: "/Home/DeleteThread",
        type: "POST",
        data: {
            'threadName': thread
        },
        dataType: "json",
        success: function (data) {
            if (data.Result) {
                $("#" + thread).remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("error");
        }
    });
}