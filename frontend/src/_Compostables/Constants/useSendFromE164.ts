import { ref } from 'vue';

const model = ref<string | null>(null);
let loadPromise: Promise<void> = Promise.reject();

if (import.meta.env.DEV) {
	model.value = import.meta.env.VITE_SEND_FROM_E164;
} else {
	loadPromise = (async () => {
		model.value = await (await fetch('/env/VITE_SEND_FROM_E164')).text();
	})()
}

const useSendFromE164 = () => {
	return model;
}

export { useSendFromE164, loadPromise }