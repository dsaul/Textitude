import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';

const useAttachments = (model: Ref<ISMSMessage | null>) => {
	return computed(() => {
        return model.value?.attachments || [];
    });
}

export { useAttachments }