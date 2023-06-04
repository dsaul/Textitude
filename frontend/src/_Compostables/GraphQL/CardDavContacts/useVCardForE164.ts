import { computed, type Ref } from 'vue';
import type { IVCard } from '@/_Compostables/GraphQL/CardDavContacts/IVCard';
import isEmpty from '@/Utility/isEmpty';
import { useQueryAll } from '@/_Compostables/GraphQL/CardDavContacts/useQueryAll';


const useVCardForE164 = (e164: Ref<string | null>) => {
	
	const { all } = useQueryAll();
	
	
	return computed<IVCard | null>(() => {
		
		if (all.value.length === 0) {
			return null;
		}
		
		if (isEmpty(e164.value)) {
			return null;
		}
		
		return all.value.find((vcard) => {
			
			if (!vcard.telephoneNumbers || vcard.telephoneNumbers.length === 0) {
				return false;
			}
			
			for (const num of vcard.telephoneNumbers) {
				if (num.e164 === e164.value) {
					return true;
				}
			}
			
			return false;
		}) || null;
	});
}

export { useVCardForE164 }