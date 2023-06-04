<script setup lang="ts">
import { onMounted, onUnmounted, computed, ref, watch, type Ref, type ComputedRef } from 'vue';
import { useRoute } from 'vue-router';
import MessageRow from '@/Components/Rows/MessageRow.vue';
import type { IBreadcrumb } from '@/_Compostables/Breadcrumbs/IBreadcrumb';
import { VuemojiPicker } from 'vuemoji-picker';
import type { EmojiClickEventDetail } from 'vuemoji-picker';
import { useQuasar } from 'quasar'
import isEmpty from '@/Utility/isEmpty';
import { requestNotificationPermission } from '@/Utility/requestNotificationPermission';
import ConversationList from '@/Components/Lists/ConversationList.vue';
import { useGlobalHasNotificationPermission } from '@/_Compostables/Globals/useGlobalHasNotificationPermission';
import { useGlobalLeftDrawerOpen } from '@/_Compostables/Globals/useGlobalLeftDrawerOpen';
import { useQueryForE164 } from '@/_Compostables/GraphQL/Messages/useQueryForE164';
import { useOwnCanSendNumbers } from '@/_Compostables/GraphQL/Numbers/useOwnCanSendNumbers';
import { useVCardForE164 } from '@/_Compostables/GraphQL/CardDavContacts/useVCardForE164';
import { useOwnFallbackNumbers } from '@/_Compostables/GraphQL/Numbers/useOwnFallbackNumbers';
import { useQueryAll } from '@/_Compostables/GraphQL/Numbers/useQueryAll';
import { useMutationDispatchFromE164ToE164 } from '@/_Compostables/GraphQL/Messages/useMutationDispatchFromE164ToE164';

const route = useRoute();
const elInput = ref();

const hasNotificationPermission = useGlobalHasNotificationPermission();
const leftDrawerOpen = useGlobalLeftDrawerOpen();
const ownCanSendNumbers = useOwnCanSendNumbers();
const ownFallbackNumbers = useOwnFallbackNumbers();
const { allOnResult: numbersOnResult } = useQueryAll();

const quasar = useQuasar();

const sendFromE164 = ref<string | null>(null);
const showEmojiMenu = ref<boolean>(false);
const inputMessage = ref<string>('');
const shouldAutoScroll = ref<boolean>(true);
const { dispatch } = useMutationDispatchFromE164ToE164();

const routeParamNumber = computed<string | null>(() => {
	const param = route?.params?.number;
	if (Array.isArray(param)) {
		if (param.length === 0) {
			return null;
		}
		return param[0].trim();
	} else if ((param as any) instanceof String || typeof param === 'string') { 
		return param?.trim() || null;
	} 
	else {
		return param || null;
	}
});




const routeParamNumberVCard = useVCardForE164(routeParamNumber);

const { forE164: messagesForE164, forE164OnResult, forE164ReFetch } = useQueryForE164(routeParamNumber);

watch(() => routeParamNumber.value, () => {
	forE164ReFetch();
});

forE164OnResult(() => {
	// console.log('forE164OnResult', result);
	
	if (shouldAutoScroll.value) {
		requestAnimationFrame(() => {
			scrollToBottom();
		});
	}
	
	
	ensureSendFromIsSet();
});

const ensureSendFromIsSet = () => {
	// set the send from to the number that last recieved a message if possible.
	do {
		// if there are no messages already, select the fallback if possible.
		if (messagesForE164.value.length === 0) {
			setSendFromE164ToFallback();
			break;
		}
		
		for (const msg of messagesForE164.value) {
			const toE164 = msg.to;
			
			if (ownCanSendNumbers.value.findIndex((e) => e.e164 === toE164) !== -1) {
				sendFromE164.value = toE164 || null;
				break;
			}
		}
		
		if (!sendFromE164.value) {
			setSendFromE164ToFallback();
		}
		break;
		
		// eslint-disable-next-line
	} while (false);
};

const setSendFromE164ToFallback = () => {
	if (ownFallbackNumbers.value.length === 0) {
		console.debug('There are no fallback numbers! #1');
		return;
	}
	
	sendFromE164.value = ownFallbackNumbers.value[0].e164 || null;
};

numbersOnResult(() => {
	if (sendFromE164.value === null) {
		ensureSendFromIsSet();
	}
});

const breadcrumbs = computed<IBreadcrumb[]>(() => {

	const ret: IBreadcrumb[] = [];

	ret.push({
		name: '',
		icon: 'home',
		to: '/',
	});
	
	if (!isEmpty(routeParamNumber.value)) {

		const vcard = routeParamNumberVCard.value;

		ret.push({
			name: vcard?.fullName || routeParamNumber.value || "--",
			to: '',
			icon: '',
		});
	}

	return ret;
});

onMounted(() => {
	// On Desktops we should focus the input field, on 
	// mobiles, if we did this it would pull open the keyboard.
	if (elInput.value && !quasar.platform.is.mobile) {
		elInput.value.focus();
	}
	
	window.addEventListener('scroll', onScroll)
	window.addEventListener('resize', onResize)
});

onUnmounted(() => {
	window.removeEventListener('scroll', onScroll)
	window.removeEventListener('resize', onResize)
});

const onScroll = () => {
	const scrollEl = document.scrollingElement || document.body;
	const nearBottom = (scrollEl.scrollTop + window.innerHeight) >= (scrollEl.scrollHeight - 150);
	
	shouldAutoScroll.value = nearBottom;
};

const onResize = () => {
	if (shouldAutoScroll.value) {
		requestAnimationFrame(() => {
			scrollToBottom();
		});
	}
}

// Methods

const scrollToBottom = () => {
	
	const scrollEl = document.scrollingElement || document.body;
	scrollEl.scrollTop = scrollEl.scrollHeight;
}

const onSendText = () => {
	const body = inputMessage.value;
	
	if (body.length === 0) {
		console.warn('not sending, length is 0');
		return;
	}
	
	if (isEmpty(sendFromE164.value)) {
		console.error('isEmpty(sendFromE164.value)');
		return;
	}
	
	if (isEmpty(routeParamNumber.value)) {
		console.error('isEmpty(routeParamNumber.value)');
		return;
	}
	
	dispatch(sendFromE164 as Ref<string>, routeParamNumber as ComputedRef<string>, inputMessage);
	
	inputMessage.value = '';
	
	requestAnimationFrame(() => {
		scrollToBottom();
	});
};

const handleEmojiClick = (detail: EmojiClickEventDetail) => {
	// console.log('handleEmojiClick', detail);
	
	showEmojiMenu.value = false;
	
	inputMessage.value = `${inputMessage.value.trim()} ${detail.unicode} `.trimStart();
	
	requestAnimationFrame(() => {
		if (elInput.value) {
			elInput.value.focus();
		}
	})
}

const toggleDrawer = () => {
	leftDrawerOpen!.value = !leftDrawerOpen!.value;
};

const onClickRequestNotificationPermission = () => {
	return (async () => {
		await requestNotificationPermission();
	})();
}

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
				<q-btn v-if="!hasNotificationPermission" @click="onClickRequestNotificationPermission" flat round dense icon="mdi-bell" class="q-mr-xs" />
			</q-toolbar>
		</q-header>
		
		
		<MessageRow
			v-for="(message, messageIdx) in messagesForE164"
			:key="message.id || `idx${messageIdx}`" 
			:value="message"
			/>
		
		<q-footer bordered class="bg-white">
			
			<div class="flex flex-col gap-y-2 p-2">
				<div class="flex flex-row gap-x-2 select-none select-none-important">
					<q-select
						dense
						v-model="sendFromE164"
						:options="ownCanSendNumbers"
						label="Send From"
						option-value="e164"
						option-label="e164"
						/>
					
				</div>
				<div class="flex flex-row gap-x-2" >
					
					<q-input
						ref="elInput"
						dense
						filled
						
						v-model="inputMessage"
						class="flex-1"
						@keyup.enter="onSendText"
						>
						<template #append>
							<q-btn v-if="!quasar.platform.is.mobile" color="" icon="mdi-emoticon-happy-outline" flat dense>
								<q-menu v-model="showEmojiMenu">
									<VuemojiPicker @emojiClick="handleEmojiClick" />
								</q-menu>
							</q-btn>
						</template>
						<template #after>
							<q-btn
								flat
								dense
								color="primary"
								icon="mdi-send"
								@click="onSendText"
								/>
						</template>
						
					</q-input>
					
				</div>
			</div>
			
		</q-footer>
		
		
		
	</div>
		
</template>

<style scoped>
</style>