
// import { DateTime } from 'luxon';
// import { computed, ref } from 'vue';
// import type { IPluckedMessage } from './GraphQL/IPluckedMessage';
// import { useTimestampISO8601 } from './GraphQL/Messages/useTimestampISO8601';

// const newestMessageForEachVillain = useNewestMessageForEachVillain();

// const useMessageList = () => {

// 	return computed(() => {

// 		const plucked: IPluckedMessage[] = [];

// 		for (const [villain, message] of Object.entries(newestMessageForEachVillain.value)) {
// 			plucked.push({ E164: villain, Message: message});
// 		}


// 		// Filter all E164s that don't have a message for some reason.
// 		const notNull = plucked.filter((e) => {
// 			return e != null;
// 		});
		
// 		const sorted = notNull.sort((left, right) => {
			
// 			const leftISO8601 = useTimestampISO8601(ref(left.Message));
// 			const rightISO8601 = useTimestampISO8601(ref(right.Message));
			
// 			const leftDt = DateTime.fromISO(`${leftISO8601.value}`);
// 			const rightDt = DateTime.fromISO(`${rightISO8601.value}`);
			
// 			if (+leftDt > +rightDt) {
// 				return 1;
// 			}
// 			if (+leftDt < +rightDt) {
// 				return -1;
// 			}
// 			return 0;
// 		});
		
// 		sorted.reverse();
		
// 		return sorted;
// 	})

// }



// export { useMessageList }

export default 'asd';