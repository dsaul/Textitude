import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';
import { useOwnNumbers } from '../Numbers/useOwnNumbers';

const useIsSentByMe = (model: Ref<ISMSMessage | null>) => {
	
	const ownNumbers = useOwnNumbers();
	
	return computed(() => {
	
		const found = ownNumbers.value.findIndex((e) => {
			return e.e164 === model.value?.from;
		});
		
		return found !== -1;
		
	});
}

export { useIsSentByMe }