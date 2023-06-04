import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';
import isEmpty from '@/Utility/isEmpty';

const useTimestampISO8601 = (
	model: Ref<ISMSMessage | null>, 
	) => {
	
	return computed(() => {
		
		if (!model.value ||
			isEmpty(model.value.iso8601Timestamp)) {
			return null;
		}
		
		return model.value.iso8601Timestamp || null;
	});
}

export { useTimestampISO8601 }