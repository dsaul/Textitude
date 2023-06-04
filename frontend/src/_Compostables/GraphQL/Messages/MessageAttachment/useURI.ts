import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { computed, type Ref } from 'vue';
import { useAttachmentURIPrefix } from '@/_Compostables/Constants/useAttachmentURIPrefix';

const attachmentURIPrefix = useAttachmentURIPrefix();

const useURI = (model: Ref<IMessageAttachment | null>) => {
	return computed(() => {
		return `${attachmentURIPrefix.value || ''}${model.value?.uri || ''}`;
	})
}

export { useURI }