import { useGlobalHasNotificationPermission } from '@/_Compostables/Globals/useGlobalHasNotificationPermission';

const hasNotificationPermission = useGlobalHasNotificationPermission();

const requestNotificationPermission =  async () => {
	if (hasNotificationPermission.value === null) {
		
		const permission = await Notification.requestPermission();
		
		switch (permission)
		{
			case 'granted': {
				hasNotificationPermission.value = true;
				break;
			}
			case 'denied': {
				hasNotificationPermission.value = false;
				break;
			}
			default: {
				hasNotificationPermission.value = null;
				break;
			}
		}
	}
};

export { requestNotificationPermission }