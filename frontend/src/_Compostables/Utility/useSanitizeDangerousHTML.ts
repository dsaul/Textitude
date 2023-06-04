import { computed, type Ref } from 'vue';

const useSanitizeDangerousHTML = (model: Ref<string>) => {
	
	return computed(() => {
		const parser = new DOMParser();
		const parseDoc = parser.parseFromString(model.value, "text/html");
		
		// Basic Santization
		for (const e of parseDoc.querySelectorAll('script')) {
			e.remove();
		}
		
		// Find inserted body element and get content of that.
		const bodyEl = parseDoc.documentElement.querySelector('body');
		if (bodyEl) {
			return bodyEl.innerHTML;
		} else { 
			return '';
		}
		
	});
};

export { useSanitizeDangerousHTML }