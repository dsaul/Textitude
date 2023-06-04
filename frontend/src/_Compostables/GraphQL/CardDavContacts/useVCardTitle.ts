import { computed, type Ref } from 'vue';
import type { IVCard } from '@/_Compostables/GraphQL/CardDavContacts/IVCard';

const useVCardTitle = (model: Ref<IVCard | null>, fallback: Ref<string | null>) => {
	return computed(() => {
		return model.value?.fullName || fallback.value || '?';
	})
}

export { useVCardTitle }