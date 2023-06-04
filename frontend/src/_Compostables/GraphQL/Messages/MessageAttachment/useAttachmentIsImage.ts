import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { computed, type Ref } from 'vue';

const useAttachmentIsImage = (model: Ref<IMessageAttachment | null>) => {
	return computed(() => {
		
		switch (model.value?.mediaType) {
			case 'image/jpeg':
			case 'image/jpg':
			case 'image/gif':
			case 'image/png':
			case 'image/bmp':
			case 'image/tiff':
			case 'image/webp': {
				return true;
			}
			default: {
				return false;
			}
		}

	})
}

export { useAttachmentIsImage }