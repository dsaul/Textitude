import { ref } from 'vue';

let initialPerm: boolean | null = null;
if (("Notification" in window)) {
	initialPerm = Notification.permission === "granted";
}

const global = ref<boolean | null>(initialPerm);

const update = () => {
	if (!("Notification" in window)) {
		console.warn("This browser does not support notifications");
		global.value = null;
		return;
	} 
	
	switch (Notification.permission)
	{
		case 'granted': {
			global.value = true;
			break;
		}
		case 'denied': {
			global.value = false;
			break;
		}
		default: {
			global.value = null;
			break;
		}
	}
};

setInterval(() => {
	update();
}, 1000);




const useGlobalHasNotificationPermission = () => {
	return global;
};

export { useGlobalHasNotificationPermission, update }