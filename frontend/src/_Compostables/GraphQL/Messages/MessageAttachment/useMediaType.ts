import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { computed, type Ref } from 'vue';

const useMediaType = (model: Ref<IMessageAttachment | null>) => {
	return computed(() => {
		return model.value?.mediaType || null;
	})
}

export { useMediaType }