import { computed, type Ref } from 'vue';
import { useBody } from './useBody';
import type { ISMSMessage } from './ISMSMessage';
import { useOwnNumbers } from '../Numbers/useOwnNumbers';

const ownNumbers = useOwnNumbers();

const useBodyForConversationsList = (
	model: Ref<ISMSMessage | null>, 
	) => {
	
    const messageBody = useBody(model);
        
	return computed(() => {
        let ret = '';
        if (ownNumbers.value.findIndex((e) => {
            return e.e164 === model.value?.from;
        }) !== -1) {
            ret += 'Me: ';
        }
        ret += messageBody.value || '';
        return ret;
    });
}

export { useBodyForConversationsList }