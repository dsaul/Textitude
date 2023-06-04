import { computed, type Ref } from 'vue';
const uriRegex = /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=+$,\w]+@)?[A-Za-z0-9.-]+|(?:www\.|[-;:&=+$,\w]+@)[A-Za-z0-9.-]+)((?:\/[+~%/.\w\-_]*)?\??(?:[-+=&;%@.\w_]*)#?(?:[.!/\\\w]*))?)/g;

const useDataDetectToHTML = (model: Ref<string>) => {
	
	return computed(() => {
		return model.value.replace(uriRegex, (match) => {
			
			const aEl = document.createElement('a');
			aEl.classList.add('underline')
			aEl.innerText = match;
			aEl.href = match;
			aEl.target = '_blank';
			return aEl.outerHTML;
		}); 
	});
};

export { useDataDetectToHTML }