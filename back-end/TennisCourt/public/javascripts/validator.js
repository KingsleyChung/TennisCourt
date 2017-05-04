var validator = {
	status: {
		username: false,
		password: false
	},

	regex: {
		username: /^[a-zA-Z][0-9a-zA-Z_]{5,15}$/,
		password: /^[0-9a-zA-Z_-]{6,12}/
	},

	errMessage: {
		username: "6~18 characters, digits or underlines, begin with character",
		password: "6~12 characters, digits or underlines, dash"
	},

	isUsernameValid: function(username) {
		return this.status.username = this.regex.username.test(username);
	},

	isPasswordValid: function(password) {
		return this.status.password = this.regex.password.test(password);
	},

	isPasswordConfirmValid: function(password, passwordConfirm) {
		return this.status.passwordConfirm = password === passwordConfirm;
	},

	isFieldValid: function(fieldname, value) {
		var method = "is" + fieldname[0].toUpperCase() + fieldname.slice(1, fieldname.length) + "Valid";
		return this[method](value);
	},

	isFormValid: function() {
		return this.status.username
			&& this.status.password;
	},

	isUserValid: function(user) {
		var err;
		for (var key in this.errMessage) {
			if (!this.isFieldValid(key, user[key])) {
				if (!err) {
					err = {};
				}
				err[key] = this.errMessage[key];
			}
		}
		return err;
	}
};

if (typeof module == 'object') {
	module.exports = validator;
}