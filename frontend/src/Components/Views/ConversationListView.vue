<script setup lang="ts">
import { computed, ref } from 'vue';
import type { IBreadcrumb } from '@/_Compostables/Breadcrumbs/IBreadcrumb';
import isEmpty from '@/Utility/isEmpty';
import { requestNotificationPermission } from '@/Utility/requestNotificationPermission';
import { useRouter } from 'vue-router';
import ConversationList from '@/Components/Lists/ConversationList.vue';
import { useGlobalHasNotificationPermission } from '@/_Compostables/Globals/useGlobalHasNotificationPermission';
import { useGlobalLeftDrawerOpen } from '@/_Compostables/Globals/useGlobalLeftDrawerOpen';
import { SRouteNameConversationE164 } from '@/Symbols';

const router = useRouter()
// const route = useRoute()

const hasNotificationPermission = useGlobalHasNotificationPermission();
const leftDrawerOpen = useGlobalLeftDrawerOpen();

const newConversationDialogueIsOpen = ref<boolean>(false);
const inputNewConversationE164 = ref<string>('');

const toggleDrawer = () => {
	if (!leftDrawerOpen) {
		return;
	}
	leftDrawerOpen.value = !leftDrawerOpen.value
};

const breadcrumbs = computed<IBreadcrumb[]>(() => {
	return [
		{
			name: 'Textitude',
			icon: 'home',
			to: '/',
		},
	];
});

const onClickRequestNotificationPermission = () => {
	if (!hasNotificationPermission) {
		return;
	}
	requestNotificationPermission();
}

const onClickOpenNewConversationDialogue = () => {
	inputNewConversationE164.value = '';
	newConversationDialogueIsOpen.value = true;
};

const onClickSubmitNewConversationDialogue = () => {
	router.push({ name: SRouteNameConversationE164, params: { number: inputNewConversationE164.value } })
	inputNewConversationE164.value = '';
	newConversationDialogueIsOpen.value = false;
};

const onClickCancelNewConversationDialogue = () => {
	inputNewConversationE164.value = '';
	newConversationDialogueIsOpen.value = false;
};

</script>

<template>
	
	<div class="flex-1 flex flex-col">
		<q-drawer show-if-above v-model="leftDrawerOpen" side="left" bordered>
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
				<q-btn v-if="!hasNotificationPermission" @click="onClickRequestNotificationPermission" flat round dense icon="mdi-bell" class="q-mr-xs" />
			</q-toolbar>
		</q-header>
		
		<!-- Contacts -->
		<ConversationList />
		<q-page-sticky position="bottom-right" :offset="[18, 18]">
			<q-btn fab icon="add" color="primary" @click="onClickOpenNewConversationDialogue" />
		</q-page-sticky>
		<q-dialog v-model="newConversationDialogueIsOpen" persistent>
			<q-card style="min-width: 350px">
				<q-card-section>
					<div class="text-h6">E164:</div>
				</q-card-section>

				<q-card-section class="q-pt-none">
					<q-input dense v-model="inputNewConversationE164" autofocus @keyup.enter="onClickSubmitNewConversationDialogue" />
				</q-card-section>

				<q-card-actions align="right" class="text-primary">
					<q-btn flat label="Cancel" @click="onClickCancelNewConversationDialogue" />
					<q-btn flat label="Open" @click="onClickSubmitNewConversationDialogue" />
				</q-card-actions>
			</q-card>
		</q-dialog>
	</div>
	
</template>

<style scoped>

</style>