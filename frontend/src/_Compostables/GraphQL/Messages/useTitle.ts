import { computed, type Ref } from 'vue';
import { useVCard } from './useVCard';
import type { ISMSMessage } from './ISMSMessage';

const useTitle = (
	model: Ref<ISMSMessage | null>, opts: { includeTo?: boolean } = {}
	) => {
	if (!('includeTo' in opts)) {
		opts.includeTo = false;
	}

	const vcard = useVCard(model);
	
	return computed(() => {

		const fromString = vcard.value?.fullName || model.value?.from || '?';

		let ret = `${fromString || ''}`;
		if (opts.includeTo) {
			ret += ` to ${model.value?.to || '?'}`;
		}
		return ret;
	})
}

export { useTitle }