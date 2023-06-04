<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { RouterView } from 'vue-router';

import { update as updateNotificationPermission } from '@/_Compostables/Globals/useGlobalHasNotificationPermission';

const routerView = ref();

onMounted(() => {
	updateNotificationPermission();
	
	addEventListener('notificationclick', (event) => { console.log('notificationclick', event) });
});

</script>
<template>
	<q-layout view="hHh LpR fFf" class="font-sans">
		<q-page-container>
			<RouterView
				ref="routerView"
				/>
		</q-page-container>
	</q-layout>
</template>