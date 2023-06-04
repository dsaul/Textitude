interface INumber {
	id?: string | null;
	e164?: string | null;
	canSend?: boolean | null;
	isOwned?: boolean | null;
	isFallback?: boolean | null;
}

export { type INumber }