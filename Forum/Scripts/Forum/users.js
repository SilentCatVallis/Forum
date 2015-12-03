function deleteUser(userName) {
	$.ajax({
		url: "/Home/DeleteUser",
		type: "POST",
		data: {
			'userName': userName
		},
		dataType: "json",
		success: function (data) {
			if (data.Result) {
				$("#" + userName).remove();
			} else {
				alert(data.Message);
			}
		},
		error: function () {
			alert("error");
		}
	});
}

function disableAdmin(userName) {
	$.ajax({
		url: "/Home/DisableAdmin",
		type: "POST",
		data: {
			'userName': userName
		},
		dataType: "json",
		success: function (data) {
			if (data.Result) {
				var button = $("#" + userName).find("button").last();
				button.addClass("btn-info").removeClass("btn-default");
				button.text("Enable admin");
				button.attr("onclick", "enableAdmin('"+userName+"')");
				$("#" + userName).find("span").remove();
			} else {
				alert(data.Message);
			}
		},
		error: function () {
			alert("error");
		}
	});
}

function enableAdmin(userName) {
	$.ajax({
		url: "/Home/EnableAdmin",
		type: "POST",
		data: {
			'userName': userName
		},
		dataType: "json",
		success: function (data) {
			if (data.Result) {
				var button = $("#" + userName).find("button").last();
				button.removeClass("btn-info").addClass("btn-default");
				button.text("Disable admin");
				button.attr("onclick", "disableAdmin('" + userName + "')");
				button.after('<span class="label label-success user-name">Admin</span>');
			} else {
				alert(data.Message);
			}
		},
		error: function () {
			alert("error");
		}
	});
}