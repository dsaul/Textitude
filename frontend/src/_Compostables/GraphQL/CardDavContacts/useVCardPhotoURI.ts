import { computed, type Ref } from 'vue';
import { useAttachmentURIPrefix } from '@/_Compostables/Constants/useAttachmentURIPrefix';
import type { IVCard } from '@/_Compostables/GraphQL/CardDavContacts/IVCard';

const attachmentURIPrefix = useAttachmentURIPrefix();

const useVCardPhotoURI = (model: Ref<IVCard | null>) => {
	return computed(() => {
        return `${attachmentURIPrefix.value || ''}${model.value?.photoURI || ''}`;
    });
}

export { useVCardPhotoURI }