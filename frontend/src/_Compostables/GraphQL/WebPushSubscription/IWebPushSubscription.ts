interface IWebPushSubscription {
	endpoint?: string | null;
	expirationTime?: string | null;
	keys: {
		auth?: string | null;
		p256dh?: string | null;
	}
}

export { type IWebPushSubscription }