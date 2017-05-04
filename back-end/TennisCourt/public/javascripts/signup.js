$(setListeners);

function setListeners() {
	$('#signup div input:not(#passwordConfirm)').on('blur', function() {
		if (!validator.isFieldValid(this.id, $(this).val())) {
            $(this).parent().find('.error').html(validator.errMessage[this.id]);
		} else {
            $(this).parent().find('.error').html("");
		}
	});

	$('#signup #passwordConfirm').on('blur', function() {
		var password = $(this).val();
		var passwordConfirm = $('#signup #password').val();
		if (password !== passwordConfirm) {
            $(this).parent().find('.error').html("must be the same as the password above");
		} else {
            $(this).parent().find('.error').html("");
		}
	});

	$("#signup #submit").on('click', function() {
		$('#signup input:not(.button)').blur();
		if (!validator.isFormValid()) {
			return false;
		}
	});
	
	$("#signup #reset").on('click', function() {
		$(this).parent().find('.error').removeClass('red');
	});
}
