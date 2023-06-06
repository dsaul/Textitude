import { computed, type Ref } from "vue";
import type { ISMSMessage } from "./ISMSMessage";
import { useQueryAll as useQueryAllNumbers } from '@/_Compostables/GraphQL/Numbers/useQueryAll';

const useRemoteE164 = (model: Ref<ISMSMessage | null>) => {
	
	const { all: myNumbers } = useQueryAllNumbers();
	
	return computed(() => {

		if (!model.value) {
			return null;
		}
	
		if (myNumbers.value.findIndex((e) => e.e164 === model.value?.from) < 0) {
			return model.value.from || null;
		}
	
		if (myNumbers.value.findIndex((e) => e.e164 === model.value?.to) < 0) {
			return model.value.to || null;
		}
	
		return null;
	});
};

export { useRemoteE164 }