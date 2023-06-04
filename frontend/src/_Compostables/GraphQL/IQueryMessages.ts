import type { ISMSMessage } from "./Messages/ISMSMessage";

interface IQueryMessages {
	latestMessages?: ISMSMessage[];
	forE164?: ISMSMessage[];
}
export { type IQueryMessages }