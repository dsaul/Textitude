<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import MDIAttachment from '@/Components/SVG/Icons/MDIAttachment.vue';
import ImageWithFallback from '@/Components/ImageWithFallback.vue';
import MDIAccount from '@/Components/SVG/Icons/MDIAccount.vue';
import { useTimestampFormatted } from '@/_Compostables/GraphQL/Messages/useTimestampFormatted';
import { useAttachments } from '@/_Compostables/GraphQL/Messages/useAttachments';
import { useBodyForConversationsList } from '@/_Compostables/GraphQL/Messages/useBodyForConversationsList';
import { useVCardPhotoURI } from '@/_Compostables/GraphQL/CardDavContacts/useVCardPhotoURI';
import { useVCardForE164 } from '@/_Compostables/GraphQL/CardDavContacts/useVCardForE164';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import type { ISMSMessage } from '@/_Compostables/GraphQL/Messages/ISMSMessage';
import { useRouter } from 'vue-router';
import { useVCardTitle } from '@/_Compostables/GraphQL/CardDavContacts/useVCardTitle';
import { useRemoteE164 } from '@/_Compostables/GraphQL/Messages/useRemoteE164';
import RemoveConversationModal from '../Dialogues/RemoveConversationModal.vue';

const props = defineProps<{
	modelValue?: ISMSMessage,
	value?: ISMSMessage,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: ISMSMessage | null): void,
	(e: 'on-value-changed', payload: ISMSMessage | null): void,
}>();

const model = useModelValue(emit, props, null as ISMSMessage | null);


const e164 = useRemoteE164(model);
const body = useBodyForConversationsList(model);
const attachments = useAttachments(model);
const vcard = useVCardForE164(e164);
const avatarURI = useVCardPhotoURI(vcard);
const title = useVCardTitle(vcard, e164);
const contextMenuEl = ref();
const removeConversationModalEl = ref();
const timestampDisplayInterval = ref<number | null>(null);
const timestampDisplay = ref<string>();


onMounted(() => {
	timestampDisplayInterval.value = setTimeout(() => {

		const timestampFormatted = useTimestampFormatted(model, {
			appearance: 'vague',
		});

		timestampDisplay.value = timestampFormatted.value;
	}, 1000);
});

onUnmounted(() => {
	if (timestampDisplayInterval.value) {
		clearInterval(timestampDisplayInterval.value);
		timestampDisplayInterval.value = null;
	}
});

const onActivateRemoveFromAndTo = () => {
	// console.log('onActivateRemove', e164.value);
	
	if (removeConversationModalEl.value) {
		removeConversationModalEl.value.resetAndOpen();
	}
	if (contextMenuEl.value) {
		contextMenuEl.value.hide();
	}
};

</script>
<template>
	<q-item clickable v-ripple :to="`/conversations/e164/${e164 || ''}`">
		<q-item-section avatar>
			<ImageWithFallback :src="avatarURI" classNames="w-12 h-12" rounded="xl">
				<template #fallback>
					<div class="h-full bg-navy-900 text-navy-0 flex items-center justify-center rounded-xl">
						<MDIAccount class="w-10 h-10" />
					</div>
				</template>
			</ImageWithFallback>
		</q-item-section>
		
		<RemoveConversationModal ref="removeConversationModalEl" v-model="model" />
		<q-menu ref="contextMenuEl" touch-position context-menu>

			<q-list dense style="min-width: 100px">
				<!-- <q-separator /> -->
				<q-item clickable @click="onActivateRemoveFromAndTo">
					<q-item-section>Remove&hellip;</q-item-section>
				</q-item>
			</q-list>

		</q-menu>

		<q-item-section>
			<q-item-label lines="1">
				<div class="flex flex-row items-center">
					<div class="flex-1">{{ title }}</div>
					<div class="text-xs">{{ timestampDisplay }}</div>
				</div>
			</q-item-label>
			<q-item-label caption lines="2" style="overflow-wrap: anywhere;">
				<MDIAttachment v-if="attachments && attachments.length > 0" class="w-6 h-6" />{{ body }}
			</q-item-label>
		</q-item-section>
	</q-item>
</template>
