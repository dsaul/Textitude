import { createRouter, createWebHistory } from 'vue-router'
import { 
	SRouteNameDashboard, 
	SRouteNameConversations, 
	SRouteNameConversationE164, 
	SRouteNameSettings 
} from "@/Symbols";

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{
			path: '/',
			name: SRouteNameDashboard,
			component: () => import('../Components/Views/DashboardView.vue'),
			props: true,
		},
		{
			path: '/conversations',
			name: SRouteNameConversations,
			component: () => import('../Components/Views/ConversationListView.vue'),
			props: true,
		},
		{
			path: '/conversations/e164/:number',
			name: SRouteNameConversationE164,
			component: () => import('../Components/Views/ConversationE164View.vue'),
			props: true,
		},
		{
			path: '/settings',
			name: SRouteNameSettings,
			component: () => import('../Components/Views/SettingsView.vue'),
			props: true,
		},
	]
})

export default router
