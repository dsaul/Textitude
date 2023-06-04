// import { computed, type Ref } from 'vue';
// import { DateTime } from 'luxon';
// import type { ISMSMessage } from './ISMSMessage';

// const ownNumbers = useOwnNumbers();

// const useMessagesForNumber = (e164: Ref<string | null>) => {
// 	return computed<ISMSMessage[]>(() => {
		
// 		return [];
		// if (e164.value === null) {
		// 	return [];
		// }

		// const assembled: ISMSMessage[] = [];
		
		// const cache = cacheMessagesByNumber?.value || {};
		
		// if (e164.value in cache) {
		// 	for (const message of Object.values(cache[e164.value])) {
		// 		assembled.push(message);
		// 	}
		// }
		
		// for (const num of ownNumbers.value) {
		// 	if (num.E164 in cache) {
		// 		for (const message of Object.values(cache[num.E164])) {
					
		// 			if (`${message.to}`.trim() === e164.value) {
		// 				assembled.push(message);
		// 			}
		// 		}
		// 	}
			
		// }
		
		// assembled.sort((a, b) => {
			
		// 	const aDt = DateTime.fromISO(a.iso8601Timestamp!);
		// 	const bDt = DateTime.fromISO(b.iso8601Timestamp!);
			
		// 	if (+aDt > +bDt) {
		// 		return 1;
		// 	}
		// 	if (+aDt < +bDt) {
		// 		return -1;
		// 	}
		// 	return 0;
			
		// });
		
		// return assembled;
		
		
// 	});
// }

// export { useMessagesForNumber }

export default 'asd';