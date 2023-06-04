import { ref } from 'vue';

const model = ref<string | null>(null);
let loadPromise: Promise<void> | null = null;

if (import.meta.env.DEV) {
	loadPromise = (async () => {
		model.value = import.meta.env.VITE_DEBUG_WEB_PUSH_APPLICATION_SERVER_KEY;
	})()
} else {
	loadPromise = (async () => {
		model.value = await (await fetch('/env/WEB_PUSH_APPLICATION_SERVER_KEY')).text();
	})()
}

const useWebPushApplicationServerKey = () => {
	return model;
}

export { useWebPushApplicationServerKey, loadPromise }