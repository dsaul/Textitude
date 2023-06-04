import { createApp, ref } from 'vue';
import { Quasar } from 'quasar';
import { useWebPushApplicationServerKey, loadPromise } from '@/_Compostables/Constants/useWebPushApplicationServerKey'
import { useMutationNewSubscription } from '@/_Compostables/GraphQL/WebPushSubscription/useMutationNewSubscription';

// Import icon libraries
import '@quasar/extras/roboto-font-latin-ext/roboto-font-latin-ext.css'
import '@quasar/extras/material-icons/material-icons.css'
import '@quasar/extras/mdi-v6/mdi-v6.css'
import 'quasar/src/css/index.sass'

import App from './App.vue'
import router from './router'

import urlBase64ToUint8Array from './Utility/urlBase64ToUint8Array'; 

import './assets/main.css'
import isEmpty from './Utility/isEmpty';

const webPushApplicationServerKey = useWebPushApplicationServerKey();
const { newSubscription } = useMutationNewSubscription();

(async () => {
	const app = createApp(App)
	app.config.performance  = true;
	
	app.use(router)
	app.use(Quasar)
	
	// Try to register service worker.
	try {
		if (!('serviceWorker' in navigator)) {
			throw new Error('Service Worker isn\'t supported on this browser.')
		}
	
		if (!('PushManager' in window)) {
			throw new Error('Push isn\'t supported on this browser.');
		}
		
		const registration = await navigator.serviceWorker.register('/service-worker.js');
		
		// console.log('Service worker successfully registered.', webPushApplicationServerKey.value);
		
		await loadPromise;
		if (isEmpty(webPushApplicationServerKey.value)) {
			throw new Error('isEmpty(webPushApplicationServerKey.value)');
		}
		
		const pushSubscription = await registration.pushManager.subscribe({
			userVisibleOnly: true,
			applicationServerKey: urlBase64ToUint8Array(
				webPushApplicationServerKey.value,
			),
		});
		
		const sub = JSON.parse(JSON.stringify(pushSubscription));
		// console.log(
		// 	'Received PushSubscription: ',
		// 	sub,
		// );
		
		newSubscription(ref(sub));
		
		
		
	} catch (e) {
		console.error('Error setting up Push Subscriptions', e);
	}
	
	
	app.mount('#app')
})();



