<script setup lang="ts">
import { computed } from 'vue';
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
import { useQueryAll as useQueryAllNumbers } from '@/_Compostables/GraphQL/Numbers/useQueryAll';
import { useVCardTitle } from '@/_Compostables/GraphQL/CardDavContacts/useVCardTitle';
const props = defineProps<{
	modelValue?: ISMSMessage,
	value?: ISMSMessage,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: ISMSMessage | null): void,
	(e: 'on-value-changed', payload: ISMSMessage | null): void,
}>();

const model = useModelValue(emit, props, null as ISMSMessage | null);

const { all: myNumbers } = useQueryAllNumbers();

const e164 = computed(() => {
	
	if (!model.value) {
		return null;
	}
	
	if (myNumbers.value.findIndex((e) => e.e164 === model.value?.from) < 0) {
		return model.value.from || null;
	}
	
	if (myNumbers.value.findIndex((e) => e.e164 === model.value?.to) < 0) {
		return model.value.to || null;
	}
	
	return null;
});

const body = useBodyForConversationsList(model);
const attachments = useAttachments(model);
const vcard = useVCardForE164(e164);
const avatarURI = useVCardPhotoURI(vcard);
const title = useVCardTitle(vcard, e164);
const timestampFormatted = useTimestampFormatted(model, {
	appearance: 'vague',
});

</script>
<template>
	
	<q-item
		clickable
		v-ripple
		:to="`/conversations/e164/${e164 || ''}`"
		>
		<q-item-section avatar>
			<ImageWithFallback
				:src="avatarURI"
				classNames="w-12 h-12"
				rounded="xl"
				>
				<template #fallback>
					<div class="h-full bg-navy-900 text-navy-0 flex items-center justify-center rounded-xl">
						<MDIAccount class="w-10 h-10" />
					</div>
				</template>
			</ImageWithFallback>
		</q-item-section>

		<q-item-section>
			<q-item-label lines="1">
				<div class="flex flex-row items-center">
					<div class="flex-1">{{ title }}</div>
					<div class="text-xs">{{timestampFormatted}}</div>
				</div>
			</q-item-label>
			<q-item-label caption lines="2" style="overflow-wrap: anywhere;">
				<MDIAttachment v-if="attachments && attachments.length > 0" class="w-6 h-6" />{{ body }}
			</q-item-label>
		</q-item-section>
	</q-item>
</template>
