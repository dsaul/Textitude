import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { computed, type Ref } from 'vue';

const useAttachmentIsAudio = (model: Ref<IMessageAttachment | null>) => {
	return computed(() => {
		switch (model.value?.mediaType) {
			case 'audio/basic':
			case 'audio/L24':
			case 'audio/mp4':
			case 'audio/mpeg':
			case 'audio/ogg':
			case 'audio/vnd.rn-realaudio': 
			case 'audio/vnd.wave':
			case 'audio/3gpp':
			case 'audio/3gpp2':
			case 'audio/ac3':
			case 'audio/webm':
			case 'audio/amr-nb':
			case 'audio/amr':
			case 'audio/aac':
			case 'audio/ogg; codecs=opus': {
				return true;
			}
			default: {
				return false;
			}
		}

	})
}

export { useAttachmentIsAudio }