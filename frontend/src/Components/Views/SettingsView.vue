<script setup lang="ts">
import { computed } from 'vue';
// import { useRouter } from 'vue-router';
import type { IBreadcrumb } from '@/_Compostables/Breadcrumbs/IBreadcrumb';
import isEmpty from '@/Utility/isEmpty';
import ConversationList from '@/Components/Lists/ConversationList.vue';
import { useGlobalLeftDrawerOpen } from '@/_Compostables/Globals/useGlobalLeftDrawerOpen';

// const router = useRouter()

const leftDrawerOpen = useGlobalLeftDrawerOpen();

const toggleDrawer = () => {
	leftDrawerOpen.value = !leftDrawerOpen.value;
};

const breadcrumbs = computed<IBreadcrumb[]>(() => {
	
	return [
		{
			name: '',
			icon: 'home',
			to: '/',
		},
		{
			name: `Settings`,
			to: '',
			icon: '',
		}
	];
});

</script>

<template>
	
	<div class="flex-1 flex flex-col">
		<q-drawer show-if-above v-model="leftDrawerOpen" side="left" bordered>
			<ConversationList />
			<q-separator />
			<q-item
				clickable
				v-ripple
				:to="'/settings'"
				>
				<q-item-section avatar>
					<q-icon name="settings" />
				</q-item-section>

				<q-item-section>Settings</q-item-section>
			</q-item>
		</q-drawer>
		<q-header bordered class="bg-primary text-white">
			
			<q-toolbar>
				<q-btn dense flat round icon="menu" @click="toggleDrawer" />

				<q-breadcrumbs
					class="text-white ml-4"
					style="font-size: 20px"
					active-color="white"
					>
					
					<template #separator>
						<q-icon
						size="1.5em"
						name="chevron_right"
						color="white"
						/>
					</template>
					
					<q-breadcrumbs-el
						v-for="(crumb, crumbIdx) of breadcrumbs"
						:key="crumbIdx"
						:icon="isEmpty(crumb.icon) ? undefined : crumb.icon"
						:label="isEmpty(crumb.name) ? undefined : crumb.name"
						:to="isEmpty(crumb.to) ? undefined : crumb.to"
						/>
				</q-breadcrumbs>
				<q-space />
			</q-toolbar>
		</q-header>
		
		<!-- Contacts -->
		settings
	</div>
	
</template>

<style scoped>

</style>