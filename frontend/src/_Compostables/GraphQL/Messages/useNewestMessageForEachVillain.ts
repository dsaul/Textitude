// import { DateTime } from 'luxon';
// import { computed } from 'vue';
// import { useOwnNumbers } from '../../Numbers/useOwnNumbers';
// import isEmpty from '@/Utility/isEmpty';
// import type { ISMSMessage } from './ISMSMessage';


// const ownNumbers = useOwnNumbers();

// const useNewestMessageForEachVillain = () => {

// 	return computed(() => {
// 		const newestForEachVillain: Record<string, ISMSMessage> = {}; 
// 		if (Object.keys(cacheMessagesByNumber.value).length === 0) {
// 			console.debug('no messages in cache');
// 			return [];
// 		}
		
// 		for (const [number, messages] of Object.entries(cacheMessagesByNumber.value)) {
			
// 			for (const message of Object.values(messages) as ISMSMessage[]) {

				

// 				const heroNumbers: string[] = [];
// 				const villainNumbers: string[] = [];

// 				// Categorize the from number
// 				if (ownNumbers.value.findIndex((e) => {
// 					return !isEmpty(message.from) && e.E164 === message.from;
// 				}) === -1) {
// 					if (!isEmpty(message.from)) {
// 						villainNumbers.push(message.from);
// 					}
// 				} else {
// 					if (!isEmpty(message.from)) {
// 						heroNumbers.push(message.from);
// 					}
// 				}

// 				// Categorize the to number.
// 				if (ownNumbers.value.findIndex((e) => {
// 					return !isEmpty(message.to) && e.E164 === message.to;
// 				}) === -1) {
// 					if (!isEmpty(message.to)) {
// 						villainNumbers.push(message.to);
// 					}
// 				} else {
// 					if (!isEmpty(message.to)) {
// 						heroNumbers.push(message.to);
// 					}
// 				}

// 				if (heroNumbers.length === 0) {
// 					continue;
// 				}

// 				// debugger;

				
// 				// Check each villain number and replace the message if this message is newer than the one stored.
// 				for (const vNum of villainNumbers) {

// 					// No entry yet, just add!
// 					if (!(vNum in newestForEachVillain)) {
// 						if (vNum === '+12049778449') {
// 							debugger;
// 						}

// 						newestForEachVillain[vNum] = message;
// 						continue;
// 					}

// 					// Considered messages must have a timestamp or everything breaks down.
// 					if (isEmpty(message.iso8601Timestamp)) {
// 						continue;
// 					}

// 					// Just the newest pls.
// 					const dtCurrent = isEmpty(newestForEachVillain?.[vNum]?.iso8601Timestamp) ? 
// 						DateTime.invalid('no timestamp') : 
// 						DateTime.fromISO(newestForEachVillain[vNum].iso8601Timestamp as string);
// 					const dtProspect = DateTime.fromISO(message.iso8601Timestamp as string);

// 					if (+dtProspect > +dtCurrent) {
// 						if (vNum === '+12049778449') {
// 							debugger;
// 						}
// 						newestForEachVillain[vNum] = message;
// 						continue;
// 					}
// 				}

				
// 			}
// 		}

// 		return newestForEachVillain;
// 	});
// };

// export { useNewestMessageForEachVillain }

export default 'asd';