import { computed, type Ref } from 'vue';
import { useVCardForE164 } from '@/_Compostables/GraphQL/CardDavContacts/useVCardForE164';
import type { ISMSMessage } from './ISMSMessage';

const useVCard = (model: Ref<ISMSMessage | null>) => {
	return useVCardForE164(computed(() => {
		return model.value?.from || null;
	}))
}

export { useVCard }