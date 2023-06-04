import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { computed, type Ref } from 'vue';

const useAttachmentIsVideo = (model: Ref<IMessageAttachment | null>) => {
	return computed(() => {
		
		switch (model.value?.mediaType) {
			case 'video/mpeg':
			case 'video/mp4':
			case 'video/quicktime':
			case 'video/webm':
			case 'video/3gpp':
			case 'video/3gpp2': 
			case 'video/3gpp-tt':
			case 'video/H261':
			case 'video/H263':
			case 'video/H263-1998':
			case 'video/H263-2000':
			case 'video/H264': {
				return true;
			}
			default: {
				return false;
			}
		}
	})
}

export { useAttachmentIsVideo }