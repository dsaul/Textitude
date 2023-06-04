// import { ref, watch, nextTick } from 'vue';
// import * as onConnect from '@/_Compostables/MQTT/Events/onConnect';
// import { mqttClient } from '@/_Compostables/MQTT/client';
// import * as onReconnect from '@/_Compostables/MQTT/Events/onReconnect';

// const desired = ref<string[]>([]);
// const current = ref<string[]>([]);



// const attemptSubscribe = () => {
// 	nextTick(() => {
// 		if (mqttClient.value === null) {
// 			return;
// 		}
	
// 		const exclusion = desired.value.filter((foo) => {
// 			return current.value.findIndex((bar) => {
// 				return foo !== bar;
// 			});
// 		});
	
// 		for (const topic of exclusion) {
// 			mqttClient.value.subscribe(topic, (error: any) => {
// 				if (error) {
// 					console.error(topic, 'subscribe error', error);
// 					return;
// 				}
// 				console.debug('subscribed to topic', topic);
// 				current.value.push(topic);
// 			});
// 		}
// 	})
// };

// const reSubscribe = () => {
// 	current.value = [];
// 	attemptSubscribe();
// };

// onConnect.subscribe(() => reSubscribe());
// onReconnect.subscribe(() => reSubscribe());

// watch(() => desired.value, (next, last) => {
// 	attemptSubscribe();
// }, { deep: true, immediate: true });

// export {
// 	desired,
// 	reSubscribe,
// 	attemptSubscribe,
// };

export default 'asd';