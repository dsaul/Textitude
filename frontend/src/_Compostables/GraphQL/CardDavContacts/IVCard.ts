interface IVCard {
	addresses?: Array<{
		type?: string | null;
		label?: string | null;
		addressParts?: string[];
	}>;
	birthday?: string | null;
	eMails?: Array<{
		type?: string | null;
		eMail?: string | null;
	}>;
	fullName?: string | null;
	names?: string[];
	photoURI?: string | null;
	prodId?: string | null;
	revisionTime?: string | null;
	telephoneNumbers?: Array<{
		type?: string | null;
		e164?: string | null;
	}>
	uid?: string | null;
	version?: string | null;
}

export { type IVCard }