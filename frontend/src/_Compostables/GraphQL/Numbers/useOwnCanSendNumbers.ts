import { computed } from 'vue';
import type { INumber } from './INumber';
import { useQueryAll } from '@/_Compostables/GraphQL/Numbers/useQueryAll';

const useOwnCanSendNumbers = () => {
	
	const { all } = useQueryAll();
	
	return computed<INumber[]>(() => {
		return all.value.filter((e) => {
			return e.isOwned && e.canSend;
		});
	});
}

export { useOwnCanSendNumbers }