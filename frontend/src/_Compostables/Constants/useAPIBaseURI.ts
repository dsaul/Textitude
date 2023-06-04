import { computed } from 'vue';

const useAPIBaseURI = () => {
	return computed(() => {
		
		if (import.meta.env.DEV) {
			//return 'http://localhost:5203/';
			return 'http://localhost:6612/';
		} else {
			return '/';
		}
	})
}

export { useAPIBaseURI }