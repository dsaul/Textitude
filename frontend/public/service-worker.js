
console.log('service worker hello', self);



self.addEventListener('push', function(event) {
	if (!event.data) {
		console.warn('We got a push event without data, ignoring.');
		return;
	}
	const json = event.data.json();
	console.log('push event', json);
	
	let body = json.MessageBody;
	let image = null;
	
	if ('Attachments' in json) {
		for (const attachment in json.Attachments) {
			if (!attachment) {
				continue;
			}
			
			image = attachment?.URI || null;
			if (null === image) {
				continue;
			}
			
			break; // we only care about the first.
		}
		
		if (`${body || ''}`.trim().length === 0) {
			if (json.Attachments.length > 0) {
				body = 'Message with Attachment';
			}
		}
	}
	
	
	
	
	
	// https://web.dev/push-notifications-display-a-notification/
	const options = {
		body: body || '',
		icon: '/imgs/Logo.svg',
		badge: '/imgs/Logo.svg',
		data: json,
		image: image,
		actions: [
			{
			action: 'open-message',
			title: 'Open Message',
			icon: '/imgs/OpenMessage.svg',
			},
			// {
			// action: 'mark-message-as-read',
			// title: 'Mark as Read',
			// icon: '/imgs/MarkMessageAsRead.svg',
			// },
			// {
			// action: 'mark-message-as-spam',
			// title: 'Mark as Spam',
			// icon: '/imgs/MarkSpam.svg',
			// },
		],
	};
	
	// eslint-disable-next-line
	registration.showNotification(json.MessageFrom, options);
	
});

self.addEventListener('notificationclick', function (event) {
	const clickedNotification = event.notification;
	clickedNotification.close();
  
	const json = event.notification.data;
	console.log('click notification', event);
	
	let promiseChain = null;
	
	switch (event.action) {
		case 'mark-message-as-read': {
			console.log('mark-message-as-read');
			break;
		}
		case 'mark-message-as-spam': {
			console.log('mark-message-as-spam');
			break;
		}
		case 'open-message':
		default: {
			// eslint-disable-next-line
			promiseChain = clients.openWindow(`/conversations/e164/${json.MessageFrom}`);
			break;
		}
	}
	
	event.waitUntil(promiseChain);
});