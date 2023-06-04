import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';

const useBody = (
	model: Ref<ISMSMessage | null>, 
	) => {
	
	return computed(() => {
		return model.value?.body || '';
	});
}

export { useBody }