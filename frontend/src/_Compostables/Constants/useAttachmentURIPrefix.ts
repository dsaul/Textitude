import { ref } from 'vue';

const model = ref<string | null>(null);
let loadPromise: Promise<void> | null = null;

if (import.meta.env.DEV) {
	model.value = import.meta.env.VITE_ATTACHMENT_URI_PREFIX;
} else {
	loadPromise = (async () => {
		model.value = await (await fetch('/env/VITE_ATTACHMENT_URI_PREFIX')).text();
	})()
}

const useAttachmentURIPrefix = () => {
	return model;
}

export { useAttachmentURIPrefix, loadPromise }