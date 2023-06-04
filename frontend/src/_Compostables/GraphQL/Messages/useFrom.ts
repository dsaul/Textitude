import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';

const useFrom = (model: Ref<ISMSMessage | null>) => {
	return computed(() => {
		return model.value?.from || '?';
	})
}

export { useFrom }