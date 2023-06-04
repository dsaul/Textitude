export default (payload: string | null | undefined | ''): payload is null | undefined | '' => {
	
	if (payload === null) {
		return true;
	}
	if (payload === undefined) {
		return true;
	}
	
	return `${payload || ''}`.trim().length === 0;
};