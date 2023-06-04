import type { IQueryCardDavContacts } from "./IQueryCardDavContacts";
import type { IQueryMessages } from "./IQueryMessages";
import type { IQueryNumbers } from "./IQueryNumbers";

interface IQueryResponse
{
	messages?: IQueryMessages;
	numbers?: IQueryNumbers;
	cardDavContacts?: IQueryCardDavContacts;
}

export type { IQueryResponse }