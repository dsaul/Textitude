import { DateTime } from 'luxon';
import { computed, type Ref } from 'vue';
import type { ISMSMessage } from './ISMSMessage';
import { useTimestampISO8601 } from './useTimestampISO8601';

const useTimestampFormatted = (model: Ref<ISMSMessage | null>,
	opts: {
		appearance?: 'vague' | 'datetime-short',
	} = {}) => {

	if (!('appearance' in opts)) {
		opts.appearance = 'datetime-short';
	}
	
	const iso8601 = useTimestampISO8601(model);
	

	return computed(() => {
		let formatted = '';

		switch (opts.appearance) {
			case 'datetime-short':
			default: {
				if (iso8601.value !== null) {
					const dt = DateTime.fromISO(iso8601.value);
					formatted = dt.toLocaleString(DateTime.DATETIME_SHORT);
				}
				break;
			}
			case 'vague': {
				if (iso8601.value !== null) {
		
					const dtMsg = DateTime.fromISO(iso8601.value);
					const dtNow = DateTime.local();
					
					const diff = dtNow.diff(dtMsg, ['years', 'months', 'weeks', 'days', 'hours', 'minutes']);
					if (diff.years >= 1) {
						formatted = dtMsg.toFormat('yyyy/MM/dd');
					} else if (diff.months >= 1) {
						formatted = dtMsg.toFormat('MMMM dd');
					} else if (diff.weeks >= 2) {
						formatted = ` > ${Math.floor(diff.weeks)} week${diff.weeks !== 1 ? 's' : ''}`;
					} else if (diff.weeks >= 1) {
						formatted = `${Math.floor((diff.weeks * 7) + diff.days)} day${(diff.weeks * 7) + diff.days !== 1 ? 's' : ''}`;
					} else if (diff.days >= 1) {
						formatted = `${dtMsg.weekdayLong}`;
					} else if (diff.hours >= 1) {
						formatted = dtMsg.toFormat('h:mm a');
					} else if (diff.minutes >= 1) {
						formatted = `${Math.floor(diff.minutes)} minute${diff.minutes !== 1 ? 's' : ''}`;
					} else {
						formatted = 'now';
					}
				}
				break;
			}
		}

		
		
		return formatted;
	})
}

export { useTimestampFormatted }