import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';

const useIsSpam = (model: Ref<ISMSMessage | null>) => {
	return computed((): 'spam' | 'ham' | 'unknown' => {
        if (model.value?.isHumanConfirmedSpam !== null) {
            return model.value?.isHumanConfirmedSpam ? 'spam' : 'ham';
        } else if (model.value.isAIDetectedSpam !== null) {
            return model.value.isAIDetectedSpam ? 'spam' : 'ham';
        }
        return 'unknown';
    });
}

export { useIsSpam }