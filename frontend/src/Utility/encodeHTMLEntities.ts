export default (payload: string): string => {
	return payload.replace(/[\u00A0-\u9999<>&]/g, function(i) {
		return '&#'+i.charCodeAt(0)+';';
	});
};